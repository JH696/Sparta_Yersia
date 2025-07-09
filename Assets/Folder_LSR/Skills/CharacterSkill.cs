using System.Collections.Generic;
using UnityEngine;

public class CharacterSkill : MonoBehaviour
{
    [Header("스킬 데이터 목록 SO")]
    [SerializeField] private List<ScriptableObject> skillDataList;
    private List<SkillStatus> skillStatus = new List<SkillStatus>();

    public IReadOnlyList<SkillStatus> AllStatuses => skillStatus;

    private void Awake()
    {
        foreach (var obj in skillDataList)
        {
            if (obj is ISkillBase data)
            {
                //skillStatus.Add(new SkillStatus(data));
            }
        }
    }

    private void Update()
    {
        // 쿨타임 감소 처리
        float deltaTime = Time.deltaTime;
        foreach (var status in skillStatus)
        {
            //status.
        }
    }
}

///// <summary> 배틀매니져에서 턴마다 호출 </summary>
//public void UseSkill(string id)
//{
//    var status = skillStatus.Find(skillState => skillState.Data.Id == id);
//    if (status == null || !status.CanUse) return;

//    // 마나 소모 로직
//    int cost = (status.Data is SkillData skillDatas) ? skillDatas.ManaCost : 0;
//    if (cost > 0)
//    {
//        if (playerStats == null || playerStats.CurrentMana < cost)
//        {
//            Debug.Log("스킬 사용 실패: 마나 부족");
//            return;
//        }
//        playerStats.SetCurrentMana(playerStats.CurrentMana - cost);
//    }

//    status.Use();

//    // 실제 스탯 연동된 데미지 계산
//    float atk = (playerStats != null) ? playerStats.Attack : 0f;
//    float damage = status.Data.Damage + (status.Data.Coefficient * atk);
//    Debug.Log($"[{id}] 를 사용했습니다. 데미지: {damage}");
//}

///// <summary> 배틀매니져에서 턴마다 호출 </summary>
//public void CompleteCoolTime(string id)
//{
//    var status = skillStatus.Find(skillState => skillState.Data.Id == id);
//    if (status == null)
//    {
//        Debug.LogWarning($"[PlayerSkillController] 스킬 {id}상태를 찾을 수 없습니다.");
//        return;
//    }
//    status.ResetCoolTime();
//}
