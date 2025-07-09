using System.Collections.Generic;
using UnityEngine;

public class BattleTrigger : MonoBehaviour
{
    [SerializeField] private EScene battleScene;

    [Header("배틀에 등장할 몬스터 데이터 리스트")]
    [SerializeField] private List<MonsterData> monsterDataList = new List<MonsterData>();

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (!other.CompareTag("Player")) return;

        var playerParty = other.GetComponent<PlayerParty>();
        if (playerParty == null)
        {
            Debug.LogWarning("PlayerParty 컴포넌트를 찾을 수 없습니다.");
            return;
        }

        // BattleManager가 있다면 여기에 데이터 전달
        //BattleManager.monsterDataList = new List<MonsterData>(monsterDataList);
        //BattleManager.playerPartyList = playerParty.GetFullPartyMembers();

        // 씬 전환
        SceneLoader.LoadScene(battleScene);
    }
}