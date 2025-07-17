using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BattleTrigger : MonoBehaviour
{
    [Header("배틀에 등장할 몬스터 데이터 리스트")]
    [SerializeField] private List<GameObject> monsterList = new List<GameObject>();

    [Header("충돌 무시 여부")]
    [SerializeField] private bool isIgnoring;

    [Header("충돌 무시 시간")]
    [SerializeField] private float ignoreTime = 2f;

    private void OnEnable()
    {
        StartCoroutine(IgnoreCollision());
    }

    public IEnumerator IgnoreCollision()
    {
        isIgnoring = true;

        SpriteRenderer sr = GetComponent<SpriteRenderer>();
        Color color = sr.color;

        color.a = 0.1f;
        sr.color = color;

        yield return new WaitForSeconds(ignoreTime);

        color.a = 1f;
        sr.color = color;

        isIgnoring = false;
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (!other.CompareTag("Player") || isIgnoring) return;

        Debug.Log("충돌");

        if (monsterList.Count == 0)
        {
            Debug.LogWarning("PlayerParty 컴포넌트를 찾을 수 없거나, 몬스터 리스트가 비었습니다.");
            return;
        }

        //B_Manager.Instance.SetTrigger(this.gameObject);
        //B_Manager.Instance.SavaModelPosition(other.transform);
        //B_Manager.Instance.EnterBattle(monsterList);
    }
}