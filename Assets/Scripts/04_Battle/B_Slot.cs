using System.Collections;
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

    [Header("애니메이션 설정")]
    [SerializeField] private Animator animator;
    [SerializeField] private float moveDistance = 3f;   // 이동 거리 (world 단위)
    [SerializeField] private float duration = 0.5f;     // 이동 시간

    private BattleVisuals visuals;

    public bool IsDead => character == null || character.IsDead;
    public CharacterStatus Character => character;

    public void SetSlot(CharacterStatus status)
    {
        if (status == null) return;

        this.gameObject.SetActive(true);
        character = status;
        spr.sprite = status.GetWSprite();

        visuals = Character.GetBattleVisuals();
        character.TakeDamaged += PlayHitAnim;

        ReplaceClip("Base_Move", visuals.Move);
        ReplaceClip("Base_Idle", visuals.Idle);
        ReplaceClip("Base_Attack", visuals.Attack);
        ReplaceClip("Base_Cast", visuals.Cast);
        ReplaceClip("Base_Hit", visuals.Hit);
        ReplaceClip("Base_Die", visuals.Die);

        StartCoroutine(AppearAnimation());

        character.OnCharacterDead += statGauge.ResetGauge;
    }

    public B_Slot IncreacedAP()
    {
        bool unlinked = statGauge.Slot == null;

        if (IsDead || unlinked) return null;

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
    private void ReplaceClip(string stateName, AnimationClip newClip)
    {
        if (newClip == null) return;
    
        var controller = animator.runtimeAnimatorController as AnimatorOverrideController;
        if (controller == null)
        {
            controller = new AnimatorOverrideController(animator.runtimeAnimatorController);
            animator.runtimeAnimatorController = controller;
        }
    
        controller[stateName] = newClip;
    }

    public void PlayAttackAnim()
    {
        animator.SetTrigger("Attack");

    }

    private void PlayHitAnim()
    {
        animator.SetBool("IsDead", IsDead);

        animator.SetTrigger("Hit");
    }

    private IEnumerator AppearAnimation()
    {
        animator.SetBool("IsMoving", true);

        Vector3 targetPos = transform.position;
        Vector3 startPos = targetPos + new Vector3(moveDistance, 0f, 0f);

        // 실제 위치를 시작 지점으로 세팅
        transform.position = startPos;

        float elapsed = 0f;
        while (elapsed < duration)
        {
            transform.position = Vector3.Lerp(startPos, targetPos, elapsed / duration);
            elapsed += Time.deltaTime;
            yield return null;
        }

        transform.position = targetPos;
        animator.SetBool("IsMoving", false);
        statGauge.SetGauges(this);
    }
}
