using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Tilemaps;

[System.Serializable]
public enum E_StageType
{
    Upper,      // 상층
    Middle,     // 중층
    Lower,      // 하층
    Deep        // 심층
}

// 배틀 정보
[System.Serializable]
public struct BattleEncounter
{
    public MonsterData[] Monsters;
    public E_StageType Stage;

    public BattleEncounter(MonsterData[] monsters, E_StageType stage)
    {
        Monsters = monsters;
        Stage = stage;
    }
}

public class MonsterSpawner : MonoBehaviour
{
    [Header("현재 던전 층")]
    [SerializeField] private E_StageType stageType;

    [Header("트리거 몬스터 프리팹")]
    [SerializeField] private GameObject triggerMonster;

    [Header("몬스터 스폰")]
    [SerializeField, Tooltip("최대 스폰 가능한 몬스터 수")] private int maxSpawnCount = 3;
    [SerializeField, Tooltip("현재 스폰된 몬스터 수")] private int nowSpawnCount = 0;

    [SerializeField, Tooltip("몬스터 스폰 주기")] private float spawnInterval = 5f;

    private Tilemap tilemap;
    private List<Vector3> spawnPositions = new List<Vector3>();
    private List<TriggerMonster> triggers = new List<TriggerMonster>();

    // 내부용 타이머
    private float timer = 0;

    // 몬스터 스폰 가능 여부
    private bool IsMaxSpawn => maxSpawnCount <= nowSpawnCount;

    private void Start()
    {
        tilemap = GetComponent<Tilemap>();
        if (tilemap == null)
        {
            Debug.LogError("[MonsterSpawner] Tilemap 컴포넌트를 찾을 수 없습니다.");
            return;
        }

        CollectTilePositions();

        for (int i = 0; i < maxSpawnCount; i++)
        {
            SpawnMonsters();
        }
    }

    private void Update()
    {
        timer += Time.deltaTime;

        if (timer >= spawnInterval)
        {
            SpawnMonsters();
            timer = 0f;
        }
    }

    private void CollectTilePositions()
    {
        spawnPositions.Clear();

        BoundsInt bounds = tilemap.cellBounds;

        foreach (Vector3Int cellPos in bounds.allPositionsWithin)
        {
            if (tilemap.HasTile(cellPos))
            {
                Vector3 worldPos = tilemap.GetCellCenterWorld(cellPos); // 정확한 중심 좌표
                spawnPositions.Add(worldPos);
            }
        }
    }

    // 몬스터 스폰 메서드
    private void SpawnMonsters()
    {
        if (spawnPositions.Count == 0 || triggerMonster == null)
        {
            Debug.Log("[MonsterSpawner] 컴포넌트를 확인해주세요.");
        }

        if (IsMaxSpawn) return;

        // MonsterData 로드 및 필터링
        MonsterData[] allDatas = Resources.LoadAll<MonsterData>("MonsterDatas");
        List<MonsterData> filtered = allDatas
            .Where(data => data.StageType == stageType)
            .ToList();

        if (filtered.Count == 0)
        {
            Debug.LogWarning($"[MonsterSpawner] {stageType}에 해당하는 몬스터 없음");
            return;
        }

        // 몬스터 개수: 1~4마리 랜덤
        int monsterCount = Random.Range(1, 5); // min 포함, max 미포함

        MonsterData[] selectedMonsters = new MonsterData[monsterCount]; // 배열 크기를 monsterCount로 설정

        for (int i = 0; i < monsterCount; i++)
        {
            MonsterData selected = filtered[Random.Range(0, filtered.Count)];
            selectedMonsters[i] = selected;
        }

        // 위치 선택 및 트리거 생성
        Vector3 spawnPos = spawnPositions[Random.Range(0, spawnPositions.Count)];
        TriggerMonster trigger =
            Instantiate(triggerMonster, spawnPos, Quaternion.identity, transform).GetComponent<TriggerMonster>();

        trigger.OnTrigged += () => nowSpawnCount--;

        nowSpawnCount++;

        trigger.SetTriggerMonster(selectedMonsters, stageType);
        triggers.Add(trigger);
    }
}
