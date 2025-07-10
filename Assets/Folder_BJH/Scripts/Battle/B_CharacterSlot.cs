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

    public void SetCharSlot(BaseCharacter character)
    {
        this.character = character; 
    }

    public void IncreaseAPoint(float amount)
    {
        actionPoint += amount;

        if (actionPoint >= 100)
        {
            actionPoint = 100;
        }

        gauge.RefreshGauge(actionPoint);
    }

    public void LinkActionGauge(B_ActionGauge gauge)
    {
        this.gauge = gauge;
    }

    public bool IsAPointMax()
    {
        return actionPoint >= 100;
    }

    public BaseCharacter GetCharacter()
    {
        return character;
    }
}
