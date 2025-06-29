using UnityEngine;
using TMPro;

public class StatsUI : MonoBehaviour
{
    [Header("StatTxt")] // StatTxt라고 써진 Text 넣어주면 됩니다
    [SerializeField] private TMP_Text HpTxt;
    [SerializeField] private TMP_Text MpTxt;
    [SerializeField] private TMP_Text AttackTxt;
    [SerializeField] private TMP_Text DefenseTxt;
    [SerializeField] private TMP_Text LuckTxt;
    [SerializeField] private TMP_Text SpeedTxt;

    private BaseCharacter currentCharacter;

    public void SetTarget(BaseCharacter character)
    {
        if (character == null) return;
        currentCharacter = character;
        RefreshUI();
    }

    public void RefreshUI()
    {
        if (currentCharacter == null) return;

        HpTxt.text = $"{Mathf.RoundToInt(currentCharacter.CurrentHp)} / {Mathf.RoundToInt(currentCharacter.MaxHp)}";
        MpTxt.text = $"{Mathf.RoundToInt(currentCharacter.CurrentMana)} / {Mathf.RoundToInt(currentCharacter.MaxMana)}";
        AttackTxt.text = $"{Mathf.RoundToInt(currentCharacter.Attack)}";
        DefenseTxt.text = $"{Mathf.RoundToInt(currentCharacter.Defense)}";
        LuckTxt.text = $"{Mathf.RoundToInt(currentCharacter.Luck)}";
        SpeedTxt.text = $"{Mathf.RoundToInt(currentCharacter.Speed)}";
    }
}