using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class B_MonsterStatUI : MonoBehaviour
{
    [Header("체력 / 마나 텍스트")]
    [SerializeField] private TextMeshProUGUI hpText;

    [Header("체력 / 마나 게이지")]
    [SerializeField] private Image hpGauge;

    public void SetGauge(B_MonsterSlot monster)
    {
        Canvas canvas = GetComponentInParent<Canvas>();
        RectTransform canvasRect = canvas.GetComponent<RectTransform>();
        Vector2 screenPos = Camera.main.WorldToScreenPoint(monster.gameObject.transform.position);

        // 이 UI의 위치를 대상 오브젝트 트랜스폼 조금 위로 이동
        Vector2 localPoint;
        RectTransformUtility.ScreenPointToLocalPointInRectangle
            (canvasRect, screenPos, Camera.main, out localPoint);

        localPoint.y += 1.25f;

        this.GetComponent<RectTransform>().localPosition = localPoint;
        this.gameObject.SetActive(true);

        RefreshGauge(monster.Monster);
    }

    public void ResetGauge()
    {
        this.gameObject.SetActive(false);
        hpGauge.fillAmount = 0;
        hpText.text = "";
    }

    public void RefreshGauge(BaseCharacter character)
    {
        hpGauge.fillAmount = character.CurrentHp / character.MaxHp;
        hpText.text = $"{character.CurrentHp} / {character.MaxHp}";
    }
}