using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UIElements;

public class IntroManager : MonoBehaviour
{

    FadeIn fadein;
    CameraFollow cameraFollow;
    TextEffect textEffect;
    [SerializeField] private CanvasGroup fadeBlack;  //까만화면
    [SerializeField] private float fadeDelay;
    [SerializeField] private string[] narrationTexts;
    [SerializeField] private float textDelay = 0.1f;
    [SerializeField] private RectTransform lastBGImage;
    [SerializeField] private RectTransform narrationBox;
    [SerializeField] private RectTransform dialogue;
    [SerializeField] private RectTransform grayImage;
    [SerializeField] private TextMeshProUGUI dialogueText;
    [SerializeField] private TextMeshProUGUI dialogueName;
    [SerializeField] private string[] dialogueTexts;


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
            lastBGImage = data.backGround;
            //페이드 인
            StartCoroutine(fadein.Fade(fadeBlack, 1f, 0f));
            yield return new WaitForSeconds(fadein.fadeDuration + fadeDelay);
            //스크롤, 텍스트 출력
            StartCoroutine(cameraFollow.Scroll());
            StartCoroutine(textEffect.PrintText(data.narrationText, textDelay));
            yield return new WaitForSeconds(data.duration + fadeDelay);
            //페이드아웃
            StartCoroutine(fadein.Fade(fadeBlack, 0f, 1f));
            yield return new WaitForSeconds(fadein.fadeDuration + fadeDelay);

            textEffect.curText.text = string.Empty;
            data.backGround.gameObject.SetActive(false);
        }
        yield return StartCoroutine(DialogueAnd());
    }

    void InputText()
    {
        cameraFollow.images[0].narrationText = "때는 바야흐로 마법의 시대, 인간과 마법사들이 공존하며 이례없던 황금기를 누리고 있었다.";
        cameraFollow.images[1].narrationText = "하지만 언제나 그렇듯 불화는 갑자기 찾아왔는데, 일부 마법을 악용한 마법사들이 마왕 소환을 목적으로 전쟁을 일으킨 것이다.";
        cameraFollow.images[2].narrationText = "불리한 전황을 이어가고 있던 와중 한 대마법사의 활약으로 어둠의 마법사를 물리치고 전쟁에 승리하게 된다.";
        cameraFollow.images[3].narrationText = "그로인해 대마법사는 큰 상처를 입게 되었지만 육신은 쓰러질지언정 수호령이 되어 나라를 지키겠다며 미래를 기약하는데...";
        dialogueTexts[0] = "자네.. 이름이 무엇인가..";
        dialogueTexts[1] = "네..? 저.. 저요? 절 어떻게..";
        dialogueTexts[2] = "그래.. 자네에게서... 미래가 보여...";
        dialogueTexts[3] = "그래서 이름이 뭐라고?";
    }

    public IEnumerator DialogueAnd()
    {
        lastBGImage.gameObject.SetActive(true);
        narrationBox.gameObject.SetActive(false);
        yield return new WaitForSeconds(1.5f);
        //페이드 인
        StartCoroutine(fadein.Fade(fadeBlack, 1f, 0f));
        yield return new WaitForSeconds(fadein.fadeDuration + fadeDelay);
        dialogue.gameObject.SetActive(true);
        textEffect.curText = dialogueText;
        for (int i = 0; i < dialogueTexts.Length; i++)
        {
            dialogueName.text = "???";
            grayImage.gameObject.SetActive(false);
            //플레이어 말할때
            if(i == 1)
            {
                dialogueName.text = string.Empty;
                grayImage.gameObject.SetActive(true);
            }
            StartCoroutine(textEffect.PrintText(dialogueTexts[i], textDelay));
            yield return new WaitForSeconds (dialogueTexts[i].Length * textDelay + 1f);
        }
        SceneLoader.LoadScene("WorldScene");
    }
}
