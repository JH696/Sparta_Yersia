using UnityEngine;
using UnityEngine.UI;

public class B_ProfilePrefab : MonoBehaviour
{
    [Header("연결된 캐릭터")]
    [SerializeField] private BaseCharacter character;

    [Header("컴포넌트 할당")]
    [SerializeField] private Image icon;
    [SerializeField] private Image hpGauge;
    [SerializeField] private Image manaGauge;

    public void SetProfile(BaseCharacter character)
    {
        this.character = character;
        RefreshGauge();
    }

    public void RefreshGauge()
    {
        icon.sprite = character.Icon;
        hpGauge.fillAmount = character.CurrentHp / character.MaxHp;
        manaGauge.fillAmount = character.CurrentMana / character.MaxMana;
    }
}
