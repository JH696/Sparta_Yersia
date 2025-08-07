using UnityEngine;
using TMPro;
using System.Collections;

public class DamageTextSpawner : MonoBehaviour
{
    [SerializeField] private GameObject damageTextPrefab;

    public void SpawnDamageText(Vector3 hitPosition, int damage)
    {
        GameObject textObj = Instantiate(damageTextPrefab, transform);
        TextMeshProUGUI tmp = textObj.GetComponentInChildren<TextMeshProUGUI>();

        tmp.text = damage.ToString();

        // 랜덤 방향으로 살짝 튕겨나가게 offset 설정
        Vector2 randomOffset = Random.insideUnitCircle.normalized * Random.Range(0.5f, 1.2f);
        Vector3 spawnPos = hitPosition + new Vector3(randomOffset.x, randomOffset.y, 0);

        textObj.transform.position = Camera.main.WorldToScreenPoint(spawnPos);

        // 부드러운 애니메이션 실행
        StartCoroutine(AnimateText(textObj));
    }

    private IEnumerator AnimateText(GameObject textObj)
    {
        RectTransform rect = textObj.GetComponent<RectTransform>();

        Vector3 startPos = rect.position;
        Vector3 endPos = startPos + Vector3.up * 60f;

        float duration = 0.8f;
        float elapsed = 0f;

        CanvasGroup canvasGroup = textObj.GetComponent<CanvasGroup>();
        if (canvasGroup == null)
        {
            canvasGroup = textObj.AddComponent<CanvasGroup>();
        }

        while (elapsed < duration)
        {
            float t = elapsed / duration;
            rect.position = Vector3.Lerp(startPos, endPos, t);
            canvasGroup.alpha = Mathf.Lerp(1f, 0f, t);

            elapsed += Time.deltaTime;
            yield return null;
        }

        Destroy(textObj);
    }
}