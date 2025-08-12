using TMPro;
using UnityEngine;

public class GuideTextUI : MonoBehaviour
{
    [Header("참조 텍스트")]
    [SerializeField] private TextMeshProUGUI nameText;
    [SerializeField] private TextMeshProUGUI contentText;

    public void UpdateGuideText(string name, string content)
    {
        nameText.text = name;
        contentText.text = content;
    }

    public void ResetGuideText()
    {
        nameText.text = "대기 중...";
        contentText.text = string.Empty;
    }
}
