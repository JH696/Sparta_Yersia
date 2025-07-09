using System;
using System.Collections.Generic;
using UnityEngine;

// 플레이어 스킬 전반 ( 해금, 사용, 쿨타임, 다음 스킬 해금 등)
public class PlayerSkillController : MonoBehaviour
{
    [Header("스킬 데이터 목록 SO")]
    [SerializeField] private List<ScriptableObject> skillDataList;

    [Header("플레이어 스킬 포인트")]
    [SerializeField] private int skillPoints = 100000; // 임시로 포인트 부여

    private CharacterStats playerStats;
    private List<SkillStatus> skillStatuses = new List<SkillStatus>();

    //스킬 상태 변경 알림 이벤트(UI)
    public event Action<SkillStatus> OnSkillStateChanged;
    public event Action<SkillStatus> OnSkillLevelUp;

    private void Awake()
    {
        // GameManager에 연결된 플레이어 스탯 가져오기
        if (GameManager.Instance != null && GameManager.Instance.Player != null)
        {
            playerStats = GameManager.Instance.Player.GetComponent<CharacterStats>();
            if (playerStats == null)
            {
                Debug.LogError("[PlayerSkillController] 플레이어 스탯을 찾을 수 없습니다.");
            }
        }

        // SO 중 ISkillInfo 인터페이스를 구현한 데이터만 필터링
        foreach (var obj in skillDataList)
        {
            if (obj is ISkillInfo info)
            {
                var status = new SkillStatus(info);
                status.OnStateChanged += skillState => OnSkillStateChanged?.Invoke(skillState);
                status.OnLevelUp += skillState => OnSkillLevelUp?.Invoke(skillState);
                skillStatuses.Add(status);
            }
        }
    }

    private void Start()
    {
        // 초급 스킬 자동 해금 - 임시 로직
        foreach (var status in skillStatuses)
        {
            if (status.Data.SkillTier == ETier.Basic)
            {
                status.Unlock();
            }
        }
    }

    public void UnlockSkill(string id) =>
        skillStatuses.Find(skillState => skillState.Data.Id == id)?.Unlock();


    public void LevelUpSkill(string id)
    {
        var status = skillStatuses.Find(skillState => skillState.Data.Id == id);
        if (status == null)
        {
            Debug.LogWarning($"[PlayerSkillController] 스킬 {id} 상태를 찾을 수 없습니다.");
            return;
        }
        status.LevelUp(ref skillPoints);
    }

    // 실제 플레이어 공격력 반환
    private float GetPlayerAttackPower()
    {
        if (playerStats == null)
        {
            Debug.LogWarning("[PlayerSkillController] 플레이어 스탯을 찾을 수 없습니다. 기본 공격력 0 반환");
            return 0f;
        }
        return playerStats.Attack;
    }
}
