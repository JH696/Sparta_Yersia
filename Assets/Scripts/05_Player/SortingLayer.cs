using UnityEngine;
using UnityEngine.Tilemaps;
using System.Collections.Generic;

public class SortAnchor : MonoBehaviour
{
    public Transform anchor;   // 바닥에 닿는 실제 지점(빈 자식)
    public float yOffset = 0f; // 필요시 미세보정
}

public class SortingLayer : MonoBehaviour
{
    [Header("대상 Sorting Layer 이름")]
    public string targetLayerName = "Default";

    [Header("플레이어 발 위치 오프셋(피벗이 발이 아니면 보정)")]
    public float feetYOffset = 0f;

    [Header("가로 영향 거리(발-기준 X). 이 값보다 멀면 무시")]
    public float xRangeMargin = 0.5f;

    [Header("플레이어 바운드 내 샘플 개수(세로)")]
    [Range(2, 20)] public int verticalSamples = 20;

    private SpriteRenderer playerSR;
    private SpriteRenderer[] otherSRs;
    private Tilemap[] tilemaps;
    private TilemapRenderer[] tilemapRenderers;

    void Awake()
    {
        playerSR = GetComponent<SpriteRenderer>();

        // 같은 SortingLayer 의 SpriteRenderer(플레이어 제외)
        var allSR = FindObjectsOfType<SpriteRenderer>();
        var listSR = new List<SpriteRenderer>(allSR.Length);
        foreach (var sr in allSR)
            if (sr != playerSR && sr.sortingLayerName == targetLayerName)
                listSR.Add(sr);
        otherSRs = listSR.ToArray();

        // 같은 SortingLayer 의 Tilemap
        var allTM = FindObjectsOfType<Tilemap>();
        var listTM = new List<Tilemap>();
        var listTR = new List<TilemapRenderer>();
        foreach (var tm in allTM)
        {
            var tr = tm.GetComponent<TilemapRenderer>();
            if (tr != null && tr.sortingLayerName == targetLayerName)
            {
                listTM.Add(tm);
                listTR.Add(tr);
            }
        }
        tilemaps = listTM.ToArray();
        tilemapRenderers = listTR.ToArray();

        playerSR.sortingLayerName = targetLayerName;
    }

    float GetBaseY(SpriteRenderer sr, out float baseX)
    {
        var a = sr.GetComponent<SortAnchor>();
        if (a != null && a.anchor != null)
        {
            baseX = a.anchor.position.x;
            return a.anchor.position.y + a.yOffset;
        }
        baseX = sr.bounds.center.x;
        return sr.bounds.min.y; // 앵커 없으면 fallback
    }

    float GetPlayerFeetY(out float feetX)
    {
        // 플레이어 발(피벗이 발이 아니면 보정)
        var b = playerSR.bounds;
        feetX = b.center.x;
        return b.min.y + feetYOffset;
    }

    void LateUpdate()
    {
        float playerFeetX;
        float feetY = GetPlayerFeetY(out playerFeetX);

        int maxBehind = int.MinValue; // 뒤(플레이어 뒤로 가야 하는 것)의 최대 order
        int minFront = int.MaxValue; // 앞(플레이어 앞에 있어야 하는 것)의 최소 order
        bool touched = false;

        // SpriteRenderer
        for (int i = 0; i < otherSRs.Length; i++)
        {
            var sr = otherSRs[i];

            // 발 기준 좌표
            float otherFeetX;
            float baseY = GetBaseY(sr, out otherFeetX);

            // 가로 영향 범위 체크(세로선 교차 대신 발 X 거리로 판정)
            float halfWidth = sr.bounds.size.x * 0.5f;
            if (Mathf.Abs(otherFeetX - playerFeetX) > (halfWidth + xRangeMargin))
                continue;

            int order = sr.sortingOrder;

            if (baseY > feetY) maxBehind = Mathf.Max(maxBehind, order); // 플레이어 뒤로 가야 함
            else minFront = Mathf.Min(minFront, order); // 플레이어 앞에 와야 함

            touched = true;
        }

        // Tilemap
        var pBounds = playerSR.bounds;
        float step = pBounds.size.y / (verticalSamples - 1);
        Vector3 feetPos = new Vector3(playerFeetX, feetY, transform.position.z);

        for (int j = 0; j < tilemaps.Length; j++)
        {
            var tm = tilemaps[j];
            var tr = tilemapRenderers[j];

            for (int s = 0; s < verticalSamples; s++)
            {
                float y = pBounds.min.y + step * s;
                Vector3 samplePos = new Vector3(playerFeetX, y, transform.position.z);

                Vector3Int cell = tm.WorldToCell(samplePos);
                if (!tm.HasTile(cell)) continue;

                float cellBaseY = tm.GetCellCenterWorld(cell).y - tm.cellSize.y * 0.5f;
                int order = tr.sortingOrder;

                bool isBehind = (cellBaseY > feetY);
                if (isBehind) maxBehind = Mathf.Max(maxBehind, order);
                else minFront = Mathf.Min(minFront, order);

                touched = true;
            }
        }

        // 주변이 전혀 없으면 기본값
        if (!touched)
        {
            playerSR.sortingOrder = 80;
            return;
        }

        // 뒤의 최대 +1 ~ 앞의 최소 -1
        int low = (maxBehind == int.MinValue) ? int.MinValue : maxBehind + 1;
        int high = (minFront == int.MaxValue) ? int.MaxValue : minFront - 1;

        int result;
        if (low == int.MinValue && high == int.MaxValue) result = 80;     // 완전 자유
        else if (low == int.MinValue) result = high;   // 앞만 있음 -> 그 바로 뒤
        else if (high == int.MaxValue) result = low;    // 뒤만 있음 -> 그 바로 앞
        else if (low > high) result = high;   // 충돌 났으면 플레이어를 더 뒤로
        else result = (low + high) / 2;

        playerSR.sortingOrder = result;
    }
}
