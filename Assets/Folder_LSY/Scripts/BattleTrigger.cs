using System.Collections.Generic;
using UnityEngine;

public class BattleTrigger : MonoBehaviour
{
    [Header("배틀에 등장할 몬스터 데이터 리스트")]
    [SerializeField] private List<GameObject> monsterList = new List<GameObject>();

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (!other.CompareTag("Player")) return;

        Debug.Log("충돌");

        if (monsterList.Count == 0)
        {
            Debug.LogWarning("PlayerParty 컴포넌트를 찾을 수 없거나, 몬스터 리스트가 비었습니다.");
            return;
        }

        B_Manager.Instance.EnterBattle(monsterList);
    }
}