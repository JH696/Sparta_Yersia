using System.Collections;
using System.Text;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class TextEffect : MonoBehaviour
{
    public TextMeshProUGUI narrationText;

    public IEnumerator PrintText(string text, float delay)
    {
        narrationText.text = string.Empty;
        StringBuilder sb = new StringBuilder();
        for (int i = 0; i < text.Length; i++)
        {
            sb.Append(text[i]);
            narrationText.text = sb.ToString();

            yield return new WaitForSeconds(delay);
        }
    }
}
