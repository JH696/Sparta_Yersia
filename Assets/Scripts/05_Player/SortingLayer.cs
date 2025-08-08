using UnityEngine;
using UnityEngine.Tilemaps;
using System.Collections.Generic;

public class SortingLayer : MonoBehaviour
{
    [Header("대상 Sorting Layer 이름")]
    public string targetLayerName = "Default";

    [Header("플레이어 발 위치 오프셋(+면 위로)")]
    public float feetYOffset = 0f;

    [Header("플레이어 바운드 내 샘플 개수(세로)")]
    [Range(2, 20)]
    public int verticalSamples = 20;

    private SpriteRenderer playerSpriteRenderer;
    private SpriteRenderer[] otherSpriteRenderers;
    private Tilemap[] tilemaps;
    private TilemapRenderer[] tilemapRenderers;

    void Awake()
    {
        playerSpriteRenderer = GetComponent<SpriteRenderer>();

        // 같은 SortingLayer 의 SpriteRenderer(플레이어 제외)
        var allSR = FindObjectsOfType<SpriteRenderer>();
        var listSR = new List<SpriteRenderer>(allSR.Length);
        foreach (var sr in allSR)
            if (sr != playerSpriteRenderer && sr.sortingLayerName == targetLayerName)
                listSR.Add(sr);
        otherSpriteRenderers = listSR.ToArray();

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
    }

    void LateUpdate()
    {
        var pBounds = playerSpriteRenderer.bounds;
        float sampleX = pBounds.center.x;

        // 발 위치(피벗이 발에 없으면 min.y + 오프셋)
        float feetY = pBounds.min.y + feetYOffset;

        // 뒤쪽의 최대 order / 앞쪽의 최소 order
        int maxBehind = int.MinValue;   // 뒤: baseY > feetY
        int minFront = int.MaxValue;   // 앞: baseY <= feetY
        bool touched = false;

        // SpriteRenderer
        for (int i = 0; i < otherSpriteRenderers.Length; i++)
        {
            var sr = otherSpriteRenderers[i];

            // 플레이어의 세로선과 가로로 겹치지 않으면 제외
            if (sr.bounds.max.x < sampleX || sr.bounds.min.x > sampleX)
                continue;

            float baseY = sr.bounds.min.y;  // 스프라이트 바닥선
            int order = sr.sortingOrder;

            if (baseY > feetY)
                maxBehind = Mathf.Max(maxBehind, order);
            else
                minFront = Mathf.Min(minFront, order);

            touched = true;
        }

        // Tilemap
        float step = pBounds.size.y / (verticalSamples - 1);

        // 발이 위치한 월드 좌표(셀은 타일맵마다 다르니 타일맵별로 계산해서 캐시)
        Vector3 feetPos = new Vector3(sampleX, feetY, transform.position.z);

        for (int j = 0; j < tilemaps.Length; j++)
        {
            var tm = tilemaps[j];
            var tr = tilemapRenderers[j];

            Vector3Int feetCell = tm.WorldToCell(feetPos); // 타일맵별로 1회 계산

            for (int s = 0; s < verticalSamples; s++)
            {
                float y = pBounds.min.y + step * s;
                Vector3 samplePos = new Vector3(sampleX, y, transform.position.z);

                Vector3Int cell = tm.WorldToCell(samplePos);
                if (!tm.HasTile(cell)) continue;

                bool isFeetCell = (cell == feetCell);

                float cellBaseY = tm.GetCellCenterWorld(cell).y - tm.cellSize.y * 0.5f;
                int order = tr.sortingOrder;

                // 발 셀은 앞(플레이어가 뒤)으로 강제
                bool isBehind = !isFeetCell && (cellBaseY > feetY);

                if (isBehind)
                    maxBehind = Mathf.Max(maxBehind, order);
                else
                    minFront = Mathf.Min(minFront, order);

                touched = true;
            }
        }

        // 주변이 전혀 없으면 기본값
        if (!touched)
        {
            playerSpriteRenderer.sortingOrder = 70;
            return;
        }

        // 뒤의 최대 +1 ~ 앞의 최소 -1 범위 안으로 배치
        int low = (maxBehind == int.MinValue) ? int.MinValue : maxBehind + 1;
        int high = (minFront == int.MaxValue) ? int.MaxValue : minFront - 1;

        int result;
        if (low == int.MinValue && high == int.MaxValue)
            result = 70;                 // 완전 자유
        else if (low == int.MinValue)
            result = high;               // 앞만 존재
        else if (high == int.MaxValue)
            result = low;                // 뒤만 존재
        else if (low > high)
            result = low;                // 충돌 시 뒤쪽 우선(혹은 high로 바꿔도 됨)
        else
            result = (low + high) / 2;   // 범위의 중간값

        playerSpriteRenderer.sortingOrder = result;
    }
}
