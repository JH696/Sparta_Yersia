using UnityEngine;
using UnityEngine.Tilemaps;
using System.Collections.Generic;

public class SortingLayer : MonoBehaviour
{
    [Header("대상 Sorting Layer 이름")]
    public string targetLayerName = "Default";

    [Header("플레이어 바운드 내 샘플 개수")]
    [Range(2, 20)]
    public int verticalSamples = 8;

    private SpriteRenderer playerSpriteRenderer;
    private SpriteRenderer[] otherSprites;
    private Tilemap[] tilemaps;
    private TilemapRenderer[] tilemapRenderers;

    void Awake()
    {
        playerSpriteRenderer = GetComponent<SpriteRenderer>();

        // Default 레이어의 SpriteRenderer 찾기
        var allSR = FindObjectsOfType<SpriteRenderer>();
        var listSR = new List<SpriteRenderer>(allSR.Length);
        foreach (var sr in allSR)
            if (sr != playerSpriteRenderer && sr.sortingLayerName == targetLayerName)
                listSR.Add(sr);
        otherSprites = listSR.ToArray();

        // Default 레이어의 모든 타일맵 찾기
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
        var bounds = playerSpriteRenderer.bounds;
        float bottomY = bounds.min.y;
        float height = bounds.size.y;
        float step = height / (verticalSamples - 1);

        int maxOrder = int.MinValue;
        float maxOrderY = 0f;
        bool foundAny = false;

        // 플레이어 verticalSamples 점
        for (int i = 0; i < verticalSamples; i++)
        {
            Vector3 samplePos = new Vector3(
                bounds.center.x,
                bottomY + step * i,
                bounds.center.z
            );

            // SpriteRenderer 체크, bounds내 포함 여부
            for (int j = 0; j < otherSprites.Length; j++)
            {
                var sr = otherSprites[j];
                if (!sr.bounds.Contains(samplePos)) continue;

                int o = sr.sortingOrder;
                float y = sr.bounds.center.y;
                if (!foundAny || o > maxOrder)
                {
                    maxOrder = o;
                    maxOrderY = y;
                    foundAny = true;
                }
            }

            // Tilemap
            for (int j = 0; j < tilemaps.Length; j++)
            {
                var tm = tilemaps[j];
                // 해당 위치가 타일맵에 실제 타일이 있으면
                Vector3Int cell = tm.WorldToCell(samplePos);
                if (!tm.HasTile(cell)) continue;

                var tr = tilemapRenderers[j];
                int o = tr.sortingOrder;
                // 셀의 월드 중심 Y좌표
                float y = tm.GetCellCenterWorld(cell).y;
                if (!foundAny || o > maxOrder)
                {
                    maxOrder = o;
                    maxOrderY = y;
                    foundAny = true;
                }
            }
        }

        // 주변 대상이 없을 때
        if (!foundAny)
        {
            playerSpriteRenderer.sortingOrder = 70;
            return;
        }

        // 플레이어 Y축 위치와 비교해 정렬 순서 결정
        playerSpriteRenderer.sortingOrder = (maxOrderY > transform.position.y)
                              ? maxOrder + 1
                              : maxOrder - 1;
    }
}