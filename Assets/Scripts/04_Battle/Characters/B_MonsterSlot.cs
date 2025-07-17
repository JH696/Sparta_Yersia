using UnityEngine;

public class B_MonsterSlot : MonoBehaviour
{
    //[Header("등록된 몬스터")]
    //[SerializeField] private CharacterStatus monster;

    //[Header("몬스터 상태 UI")]
    //[SerializeField] private B_MonsterStatUI mStatUI;

    //[Header("행동력, 게이지")]
    //[SerializeField] private float actionPoint;
    //[SerializeField] private B_ActionGauge aGauge;

    //[Header("몬스터 액션")]
    //[SerializeField] private MonsterAction mAction;
    
    //[Header("포인터")]
    //[SerializeField] private GameObject pointer;

    //public CharacterStatus Monster => monster;

    //public void Start()
    //{
    //    Monster.StatusChanged += StatusChange;
    //}

    //public void OnDisable()
    //{
    //    Monster.StatusChanged -= StatusChange;   
    //}

    //public void SetMonSlot(GameObject monster)
    //{
    //    this.monster = monster.GetComponent<CharacterStatus>();
    //    aGauge.SetGauge(null, this);
    //    mStatUI.SetGauge(this);
    //    this.gameObject.SetActive(true);
    //}

    //public void ResetSlot()
    //{
    //    actionPoint = 0;
    //    //monster = null;
    //    aGauge.ResetGauge();
    //    mStatUI.ResetGauge();
    //    this.gameObject.SetActive(false);
    //}

    //public void StatusChange()
    //{
    //    mStatUI.RefreshGauge(monster);

    //    if (monster.IsDead)
    //    {
    //        B_Manager.Instance.UpECount();
    //        ResetSlot();
    //    }
    //}

    //public void IncreaseAPoint()
    //{
    //    if (monster == null || monster.IsDead) return;

    //    actionPoint += monster.Speed * Time.deltaTime;

    //    if (actionPoint >= 100)
    //    {
    //        actionPoint = 0;
    //        mAction.MonsterAttack(monster);
    //    }

    //    aGauge.RefreshGauge(actionPoint);
    //}

    //public void SetPointer()
    //{
    //    pointer.SetActive(true);
    //}

    //public void ResetPointer()
    //{
    //    pointer.SetActive(false);
    //}

    //public CharacterSkill GetLearnedSkill()
    //{
    //    if (monster is Player player)
    //    {
    //        return player.Skill;
    //    }
    //    else if (monster is NPC npc)
    //    {
    //        return npc.Skill;
    //    }
    //    else if (monster is Pet pet)
    //    {
    //        return pet.Skill;
    //    }
    //    else if (this.monster is Monster monster)
    //    {
    //        return monster.Skill;
    //    }

    //    Debug.Log("알 수 없는 유형입니다.");
    //    return null;
    //}
}
