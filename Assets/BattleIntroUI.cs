using UnityEngine;

public class BattleIntroUI : MonoBehaviour
{
    public CanvasGroup IntroCanvas;

    public float FadeDuration = 1.0f;

    // Update is called once per frame
    void Update()
    {
        if (IntroCanvas.alpha > 0)
        {
            IntroCanvas.alpha -= FadeDuration * Time.deltaTime;
            Debug.Log("캔버스 알파 감소");
        }
        else if (IntroCanvas.gameObject.activeSelf)
        {
            IntroCanvas.gameObject.SetActive(false);
            Debug.Log("캔버스 비활성화");
        }
    }
}
