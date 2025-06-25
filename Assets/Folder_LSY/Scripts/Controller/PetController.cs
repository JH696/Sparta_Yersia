using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PetController : BaseCharacter
{
    [SerializeField] private Transform followTarget;
    [SerializeField] private float followDistance = 1f;
    [SerializeField] private float followSpeed = 3f;

    private void Start()
    {
        Debug.Log($"펫 스탯 확인: HP {CurrentHp}/{MaxHp}, MP {CurrentMp}/{MaxMp}, Attack {Attack}, Defense {Defense}, Luck {Luck}, Speed {Speed}");
    }

    private void Update()
    {
        if (followTarget == null) return;

        float distance = Vector3.Distance(transform.position, followTarget.position);
        if (distance > followDistance)
        {
            Vector3 dir = (followTarget.position - transform.position).normalized;
            transform.position += dir * followSpeed * Time.deltaTime;
        }

        // 테스트용: 키 입력 시 데미지 입거나 회복
        if (Input.GetKeyDown(KeyCode.K))  // K 누르면 힐 10
        {
            Heal(10f);
            Debug.Log($"펫 힐 받음: 현재 체력 {CurrentHp}/{MaxHp}");
        }
        if (Input.GetKeyDown(KeyCode.L))  // L 누르면 데미지 20
        {
            TakeDamage(20f);
            Debug.Log($"펫 데미지 입음: 현재 체력 {CurrentHp}/{MaxHp}");
        }
    }

    public void SetFollowTarget(Transform target)
    {
        followTarget = target;
    }
}
