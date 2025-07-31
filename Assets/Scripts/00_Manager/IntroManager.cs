using System.Collections;
using UnityEngine;

public class IntroManager : MonoBehaviour
{

    FadeIn fadein;
    CameraFollow cameraFollow;
    TextEffect textEffect;
    [SerializeField] private CanvasGroup fadeBlack;  //까만화면
    [SerializeField] private float fadeDelay;
    [SerializeField] private string[] narrationTexts;
    [SerializeField] private float textDelay = 0.1f;
    RectTransform lastBGImage;


    // Start is called before the first frame update
    private void Awake()
    {
        cameraFollow = GetComponent<CameraFollow>();
        fadein = GetComponent<FadeIn>();
        textEffect = GetComponent<TextEffect>();
    }
    // Start is called before the first frame update
    void Start()
    {
        InputText();
        foreach (var data in cameraFollow.images)
        {
            data.backGround.anchoredPosition = data.startPosition;
            data.backGround.gameObject.SetActive(false);
        }
        StartCoroutine(FadeInAndScrollAndFadeOut());
    }

    public IEnumerator FadeInAndScrollAndFadeOut()
    {
        foreach (var data in cameraFollow.images)
        {
            cameraFollow.image = data;
            data.duration = data.narrationText.Length * textDelay;
            data.backGround.gameObject.SetActive(true);
            StartCoroutine(fadein.Fade(fadeBlack, 1f, 0f));
            yield return new WaitForSeconds(fadein.fadeDuration + fadeDelay);
            StartCoroutine(cameraFollow.Scroll());
            StartCoroutine(textEffect.PrintText(data.narrationText, textDelay));
            yield return new WaitForSeconds(data.duration + fadeDelay);
            StartCoroutine(fadein.Fade(fadeBlack, 0f, 1f));
            yield return new WaitForSeconds(fadein.fadeDuration + fadeDelay);
            textEffect.narrationText.text = string.Empty;
            data.backGround.gameObject.SetActive(false);
        }
    }

    void InputText()
    {
        cameraFollow.images[0].narrationText = "때는 바야흐로 마법의 시대, 인간과 마법사들이 공존하며 이례없던 황금기를 누리고 있었다.";
        cameraFollow.images[1].narrationText = "하지만 언제나 그렇듯 불화는 갑자기 찾아왔는데, 일부 마법을 악용한 마법사들이 마왕 소환을 목적으로 전쟁을 일으킨 것이다.";
        cameraFollow.images[2].narrationText = "불리한 전황을 이어가고 있던 와중 한 대마법사의 활약으로 어둠의 마법사를 물리치고 전쟁에 승리하게 된다.";
        cameraFollow.images[3].narrationText = "그로인해 대마법사는 큰 상처를 입게 되었지만 육신은 쓰러질지언정 수호령이 되어 나라를 지키겠다며 미래를 기약하는데...";
    }


}
