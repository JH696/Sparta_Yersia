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
    public List<MonsterData> monsters;        // 전투에 참여하는 몬스터 리스트
    // public bool isBossFight;               // 보스전 여부
    // public AudioClip battleMusic;          // 전투 BGM

    public BattleEncounter(List<MonsterData> monsterList)
    {
        monsters = monsterList;
        // isBossFight = false; // 기본값 설정
        // battleMusic = null;   // 기본값 설정
    }
}

public class MonsterSpawner : MonoBehaviour
{
    [Header("몬스터 스폰 위치")]
    [SerializeField] private E_StageType stageType;

    [Header("트리거 몬스터 프리팹")]
    [SerializeField] private GameObject triggerMonster;

    [Header("몬스터 최대 스폰 수")]
    [SerializeField] private int spawnCount = 3;

    private bool CanSpawn => triggers.Count < spawnCount;

    private Tilemap tilemap;
    private List<Vector3> spawnPositions = new List<Vector3>();
    private List<TriggerMonster> triggers = new List<TriggerMonster>();

    private void Start()
    {
        tilemap = GetComponent<Tilemap>();
        if (tilemap == null)
        {
            Debug.LogError("[MonsterSpawner] Tilemap 컴포넌트를 찾을 수 없습니다.");
            return;
        }

        CollectTilePositions();

        for (int i = 0; i < spawnCount; i++)
        {
            SpawnMonsters();
        }
    }
    private void CollectTilePositions()
    {
        BoundsInt bounds = tilemap.cellBounds;

        foreach (Vector3Int cellPos in bounds.allPositionsWithin)
        {
            if (tilemap.HasTile(cellPos))
            {
                Vector3 worldPos = tilemap.CellToWorld(cellPos) + tilemap.tileAnchor;
                spawnPositions.Add(worldPos);
            }
        }
    }


    // 몬스터 스폰 메서드
    private void SpawnMonsters()
    {
        if (spawnPositions.Count == 0 || triggerMonster == null) return;

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

        List<MonsterData> selectedMonsters = new List<MonsterData>();
        for (int i = 0; i < monsterCount; i++)
        {
            MonsterData selected = filtered[Random.Range(0, filtered.Count)];
            selectedMonsters.Add(selected);
        }

        // 전투 인카운터 생성
        BattleEncounter encounter = new BattleEncounter(selectedMonsters);

        // 위치 선택 및 트리거 생성
        Vector3 spawnPos = spawnPositions[Random.Range(0, spawnPositions.Count)];
        TriggerMonster trigger = Instantiate(triggerMonster, spawnPos, Quaternion.identity, transform)
            .GetComponent<TriggerMonster>();

        trigger.SetTriggerMonster(encounter);
        triggers.Add(trigger);
    }
}
