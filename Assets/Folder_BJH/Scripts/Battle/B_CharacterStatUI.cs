using UnityEngine;
using UnityEngine.UI;

public class B_CharacterStatUI : MonoBehaviour
{
    [Header("캐릭터 아이콘")]
    [SerializeField] private Image icon;

    [Header("체력 / 마나 게이지")]
    [SerializeField] private Image hpGauge;
    [SerializeField] private Image mpGauge;

    public void SetProfile(BaseCharacter character)
    {
        this.gameObject.SetActive(true);   
        icon.sprite = character.Icon;
        RefreshGauge(character);
    }

    public void ResetGauge()
    {
        this.gameObject.SetActive(false);
        hpGauge.fillAmount = 0;
        mpGauge.fillAmount = 0;
    }

    public void RefreshGauge(BaseCharacter character)
    {
        hpGauge.fillAmount = character.CurrentHp / character.MaxHp;
        mpGauge.fillAmount = character.CurrentMana / character.MaxMana;
    }
}
