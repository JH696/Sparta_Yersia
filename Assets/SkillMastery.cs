using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SkillMastery : MonoBehaviour
{
    [Header("버튼 리스트")]
    [SerializeField] private List<Button> buttons = new List<Button>();

    [Header("스킬 마스터리 UI")]
    [SerializeField] private SkillMasteryUI skillMasteryUI;

    private void Start()
    {
        foreach (var button in buttons)
        {
            var btn = button;
            btn.onClick.RemoveAllListeners();
            btn.onClick.AddListener(() => OnClickButton(btn.gameObject));
            Debug.Log($"{button.name}");
        }
    }

    private void OnClickButton(GameObject gameObject)
    {
        Debug.Log("버튼 클릭됨");
        skillMasteryUI.SetInfoUI(gameObject.GetComponent<SkillSlot>());
    }
}


