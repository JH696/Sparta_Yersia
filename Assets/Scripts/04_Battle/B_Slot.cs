using UnityEngine;

public enum E_B_SlotType
{
    Ally,
    Enemy
}

public class B_Slot : MonoBehaviour
{
    [Header("등록된 캐릭터")]
    [SerializeField] private CharacterStatus character;

    [Header("진영 분류")]
    [SerializeField] private E_B_SlotType slotType = E_B_SlotType.Ally;

    [Header("스탯 게이지")]
    [SerializeField] private B_StatGauge statGauge;

    [Header("행동력")]
    [SerializeField] private float actionPoint = 0f;

    [Header("스프라이트")]
    [SerializeField] private SpriteRenderer spr;

    [Header("포인터 / 아우라")]
    [SerializeField] private GameObject pointer;
    [SerializeField] private Animator aura;

    public bool IsDead => character == null || character.IsDead;
    public CharacterStatus Character => character;
    public GameObject Pointer => pointer;
    public Animator Aura => aura;

    public void SetSlot(CharacterStatus status)
    {
        if (status == null) return;

        this.gameObject.SetActive(true);
        character = status;
        statGauge.SetGauges(this);
        spr.sprite = status.GetWSprite();

        character.OnCharacterDead += statGauge.ResetGauge;
    }

    public B_Slot IncreacedAP()
    {
        if (IsDead) return null;

        statGauge.gameObject.SetActive(true);

        actionPoint += Mathf.Clamp(Character.stat.Speed * Time.deltaTime, 0f, 100 - actionPoint);
        statGauge.RefreshAPGauge(actionPoint);

        if (actionPoint >= 100)
        {
            actionPoint = 0f;
            statGauge.RefreshAPGauge(actionPoint);
            Character.skills.ReduceCooldown(1);

            if (slotType == E_B_SlotType.Ally)
            {
                statGauge.gameObject.SetActive(false);
            }

            return this;
        }
        return null;
    }

    public E_B_SlotType GetSlotType()
    {
        return slotType;
    }

    public void ResetSlot()
    {
        if (character == null) return;

        actionPoint = 0f;
        character.OnCharacterDead -= statGauge.ResetGauge;
    }

    private void OnDestroy()
    {
        if (character == null) return;

        ResetSlot();
    }
}
