using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public enum ECharacterType
{
    Ally,
    Enemy
}

public class B_CharacterSlot : MonoBehaviour
{
    [Header("슬롯 타입")]
    [SerializeField] private ECharacterType type;

    [Header("등록된 캐릭터")]
    [SerializeField] private BaseCharacter character;

    [Header("캐릭터 행동력")]
    [SerializeField] private float actionPoint;

    [Header("연결된 게이지 UI")]
    [SerializeField] private B_ActionGauge gauge;

    [Header("몬스터 액션")]
    [SerializeField] private MonsterAction mAction;

    [Header("캐릭터 행동 중 여부")]
    [SerializeField] private bool hasTurn;

    public ECharacterType Type => type;
    public BaseCharacter Character => character;

    public void SetCharSlot(BaseCharacter character)
    {
        this.character = character;
        this.gameObject.SetActive(true);
    }

    public void ResetSlot()
    {
        actionPoint = 0;
        character = null;
        hasTurn = false;
        gauge.gameObject.SetActive(false);
        this.gameObject.SetActive(false);
    }

    public void ChangeStatus()
    {
        if (character.IsDead)
        {
            if (type == ECharacterType.Enemy)
            {
                ResetSlot();
            }

            //character.IsDIe();
        }
    }

    public void IncreaseAPoint()
    {
        if (character == null) return;

        actionPoint += character.Speed * Time.deltaTime;

        if (actionPoint >= 100)
        {
            actionPoint = 0;
            gauge.RefreshGauge(actionPoint);

            if (type == ECharacterType.Ally)
            {
                TurnStart();
            }
            else if (type == ECharacterType.Enemy)
            {
                Debug.Log("MonsterAction");
            }
        }

        gauge.RefreshGauge(actionPoint);
    }

    public void LinkActionGauge(B_ActionGauge gauge)
    {
        if (character == null) return;

        this.gauge = gauge;
    }

    public List<SkillStatus> GetLearnedSkill()
    {
        if (character is Player player)
        {
            return player.Skill.AllStatusesa;
        }
        else if (character is NPC npc)
        {
            return npc.Skill.AllStatusesa;
        }
        else if (character is Pet pet)
        {
            return pet.Skill.AllStatusesa;
        }
        else if (Character is Monster monster)
        {
            return monster.Skill.AllStatusesa;
        }

        Debug.Log("알 수 없는 유형입니다.");
        return null;
    }

    private void TurnStart()
    {
        hasTurn = true;
        gauge.gameObject.SetActive(false);
    }

    public void TurnEnd()
    {
        gauge.gameObject.SetActive(true);
        hasTurn = false;
    }

    public bool HasTurn()
    {
        return hasTurn;
    }
}
