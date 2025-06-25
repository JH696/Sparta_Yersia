using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PetController : BaseCharacter
{
    private void Start()
    {
        Debug.Log($"펫 스탯 확인: HP {CurrentHp}/{MaxHp}, MP {CurrentMp}/{MaxMp}, Attack {Attack}, Defense {Defense}, Luck {Luck}, Speed {Speed}");
    }

    private void Update()
    {
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
}
