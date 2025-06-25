using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class DialogueUI : MonoBehaviour
{
    [Header("NPC 대화 UI")]
    public Image npcRenderer;
    public GameObject DialogueBox;
    public TextMeshProUGUI Text;

    [SerializeField] private float typingSpeed = 0.05f;

    private Coroutine typingCoroutine;

    public void ShowDialogueBox()
    {
        if (DialogueBox == null) return;

        DialogueBox.SetActive(true);
    }

    public void SetSprite(Sprite sprite)
    {
        if (npcRenderer == null) return;

        npcRenderer.sprite = sprite;
    }

    public void ResetDialogueUI()
    {
         DialogueBox.SetActive(false);
         npcRenderer.sprite = null;
         Text.text = string.Empty;
    }

    public void SetTypingText(string line)
    {
        if (IsTyping())
        {
            StopCoroutine(typingCoroutine);
        }

        typingCoroutine = StartCoroutine(TypeLine(line));
    }

    private IEnumerator TypeLine(string line)
    {
        Text.text = "";
        foreach (char c in line)
        {
            Text.text += c;
            yield return new WaitForSeconds(typingSpeed);
        }
        typingCoroutine = null;
    }

    public bool IsTyping()
    {
        return typingCoroutine != null;
    }

    //public void SkipTyping(string fullLine)
    //{
    //    if (IsTyping())
    //    {
    //        StopCoroutine(typingCoroutine);
    //        Text.text = fullLine;
    //        typingCoroutine = null;
    //    }
    //}
}
