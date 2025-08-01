using System.Collections;
using System.Text.RegularExpressions;
using TMPro;
using UnityEngine;

public class BattleEffecter : MonoBehaviour
{
    [Header("연결된 슬롯")]
    [SerializeField] private B_Slot slot;

    [Header("애니메이터")]
    [SerializeField] private Animator animator;

    [Header("저장된 데미지")]
    [SerializeField] private float invokedDamage;

    [Header("데미지 텍스트")]
    [SerializeField] private TextMeshPro damageText;

    private B_BattleButtons bButtons;
    private string lastParam;
    private Coroutine hideRoutine;

    public B_Slot Slot
    {
        get { return slot; }

        private set { slot = value; }
    }

    public void SetSkillEffecter(CharacterStats attacker, SkillStatus skill, B_BattleButtons bButtons)
    {
        DamageCalculator cal = new DamageCalculator();

        CharacterStatus target = slot.Character;

        string input = skill.Data.ID;

        // 문자 부분 추출 (앞쪽 문자)
        string prefix = Regex.Match(input, @"^[^\d]+").Value;  // "S_f"

        // 숫자 부분 추출 (뒤쪽 숫자)
        string numberStr = Regex.Match(input, @"\d+$").Value;  // "01"

        // 숫자로 변환하고 싶다면
        int number = int.Parse(numberStr);

        invokedDamage = cal.DamageCalculate(attacker, target.stat, skill);
        lastParam = prefix;

        this.bButtons = bButtons;

        SetDamageText(skill.Data.Type);
        animator.SetInteger(prefix, number);
    }

    public void SetBaseEffect(CharacterStats attacker, B_BattleButtons bButtons)
    {
        DamageCalculator cal = new DamageCalculator();

        CharacterStatus target = slot.Character;

        this.bButtons = bButtons;

        invokedDamage = cal.DamageCalculate(attacker, target.stat, null);

        SetDamageText(E_ElementalType.Physical);

        animator.SetTrigger("Normal_Attack");
    }

    public IEnumerator GainDamage()
    {
        slot.Character.TakeDamage(invokedDamage);
        ShowDamageText();

        if (!string.IsNullOrEmpty(lastParam))
        {
            animator.SetInteger(lastParam, 9999);
        }

        yield return new WaitForSeconds(0.5f);

        bButtons.OnTurnEnd();
        bButtons = null;
    }

    private void SetDamageText(E_ElementalType type)
    {
        damageText.renderer.sortingOrder = 210;

        Color top;
        Color bottom;

        switch (type)
        {
            case E_ElementalType.Fire:
                top = new Color(1.0f, 0.65f, 0.32f);
                bottom = new Color(1.0f, 0.17f, 0.0f);
                break;
            case E_ElementalType.Ice:
                top = new Color(0.71f, 0.95f, 1.0f);
                bottom = new Color(0.24f, 0.62f, 1.0f);
                break;
            case E_ElementalType.Nature:
                top = new Color(0.72f, 1.0f, 0.52f);
                bottom = new Color(0.13f, 0.55f, 0.13f);
                break;
            default:
                top = new Color(0.8f, 0.8f, 0.8f);
                bottom = new Color(0.33f, 0.33f, 0.33f);
                break;
        }

        SetVerticalGradient(top, bottom);

        damageText.text = $"{invokedDamage}";
    }

    // 상단/하단 색상 설정
    private void SetVerticalGradient(Color topColor, Color bottomColor)
    {
        damageText.enableVertexGradient = true;

        damageText.colorGradient = new VertexGradient
        (
            topColor,  // topLeft
            topColor,  // topRight
            bottomColor,  // bottomLeft
            bottomColor   // bottomRight
        );
    }

    private void ShowDamageText()
    {
        damageText.gameObject.SetActive(true);

        // 기존 코루틴 중지 후 재시작
        if (hideRoutine != null) StopCoroutine(hideRoutine);

        hideRoutine = StartCoroutine(HideDamageTextAfterDelay());
    }

    private IEnumerator HideDamageTextAfterDelay()
    {
        yield return new WaitForSeconds(0.6f);

        damageText.text = string.Empty;
        SetVerticalGradient(Color.white, Color.white);
        damageText.gameObject.SetActive(false);

        invokedDamage = 0;
    }
}
