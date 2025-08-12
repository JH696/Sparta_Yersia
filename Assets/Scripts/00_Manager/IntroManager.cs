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
    [Header("페이드되는 캔버스")]
    [SerializeField] private CanvasGroup fadeBlack;  //까만화면
    private RectTransform lastBGImage;
    [Header("페이드 된 후 여유 시간")]
    [SerializeField] private float fadeDelay = 0.5f;
    [Header("텍스트 한 글자 나오는 속도")]
    [SerializeField] private float textDelay = 0.1f;
    [Header("대화 UI들")]
    [SerializeField] private RectTransform narrationBox;
    [SerializeField] private RectTransform dialogue;
    [SerializeField] private TextMeshProUGUI dialogueText;
    [SerializeField] private TextMeshProUGUI dialogueName;
    private string[] dialogueTexts = new string[5];
    [Header("대화 화자 구분용 회색화면")]
    [SerializeField] private RectTransform grayImage;
    [Header("스킵 버튼")]
    [SerializeField] private GameObject skipBtn;
    bool isSkipButtonPressed = false;


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
        lastBGImage = cameraFollow.images[cameraFollow.images.Length - 1].backGround;
        StartCoroutine(FadeInAndScrollAndFadeOut());
    }

    public IEnumerator FadeInAndScrollAndFadeOut()
    {
        foreach (var data in cameraFollow.images)
        {
            cameraFollow.image = data;
            data.duration = data.narrationText.Length * textDelay;
            data.backGround.gameObject.SetActive(true);
            //페이드 인
            StartCoroutine(fadein.Fade(fadeBlack, 1f, 0f));
            yield return new WaitForSeconds(fadein.fadeDuration + fadeDelay);
            //스크롤, 텍스트 출력
            Coroutine scrollRoutine = StartCoroutine(cameraFollow.Scroll());
            Coroutine textRoutine = StartCoroutine(textEffect.PrintText(data.narrationText, textDelay));

            yield return new WaitUntil(() =>
            {
                if (!isSkipButtonPressed && !textEffect.isTyping && Input.GetMouseButtonDown(0))
                {
                    Debug.Log("ScrollSkipClick");
                    cameraFollow.isMoving = false;
                }
                if (!isSkipButtonPressed && Input.GetMouseButtonDown(0))
                {
                    Debug.Log("TextSkipClick");
                    textEffect.curText.text = data.narrationText;
                    textEffect.isTyping = false;

                    StopCoroutine(textRoutine);
                }

                return !cameraFollow.isMoving && !textEffect.isTyping;
            });
            //페이드아웃
            StartCoroutine(fadein.Fade(fadeBlack, 0f, 1f));
            yield return new WaitForSeconds(fadein.fadeDuration + fadeDelay);

            textEffect.curText.text = string.Empty;
            StopCoroutine(scrollRoutine);
            data.backGround.gameObject.SetActive(false);
        }
        yield return StartCoroutine(DialogueAndLoadScene());
    }

    void InputText()
    {
        cameraFollow.images[0].narrationText = "때는 바야흐로 마법의 시대, 인간과 마법사들이 공존하며 이례없던 황금기를 누리고 있었다.";
        cameraFollow.images[1].narrationText = "하지만 언제나 그렇듯 불화는 갑자기 찾아왔는데, 일부 마법을 악용한 마법사들이 마왕 소환을 목적으로 전쟁을 일으킨 것이다.";
        cameraFollow.images[2].narrationText = "불리한 전황을 이어가고 있던 와중 한 대마법사의 활약으로 어둠의 마법사를 물리치고 전쟁에 승리하게 된다.";
        cameraFollow.images[3].narrationText = "그로인해 대마법사는 큰 상처를 입게 되었지만 육신은 쓰러질지언정 수호령이 되어 나라를 지키겠다며 미래를 기약하는데...";
        dialogueTexts[0] = "자네.. 이름이 무엇인가..";
        dialogueTexts[1] = "엥? 저.. 저요?";
        dialogueTexts[2] = "그래.. 자네의 영혼의 파장이 심상치 않구만...";
        dialogueTexts[3] = "그래서 이름이 뭐라고?";
        dialogueTexts[4] = "아 저는..!";
    }
    public void OnClickSkipButton()
    {
        isSkipButtonPressed = true;
        StopAllCoroutines();
        StartCoroutine(SkipAndStartDialogue());
        //skipBtn.SetActive(false);
    }

    IEnumerator SkipAndStartDialogue()
    {
        //페이드중일때 자연스럽게 페이드 아웃
        StartCoroutine(fadein.Fade(fadeBlack, fadeBlack.alpha, 1f));
        yield return new WaitForSeconds(fadein.fadeDuration + fadeDelay);
        cameraFollow.image.backGround.gameObject.SetActive(false);
        StartCoroutine(DialogueAndLoadScene());
    }

    public IEnumerator DialogueAndLoadScene()
    {
        skipBtn.SetActive(false);
        lastBGImage.gameObject.SetActive(true);
        lastBGImage.anchoredPosition = (new Vector2(0, -300));
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
            if (i == 1 || i == 4)
            {
                dialogueName.text = string.Empty;
                grayImage.gameObject.SetActive(true);
            }
            Coroutine textRoutine = StartCoroutine(textEffect.PrintText(dialogueTexts[i], textDelay));
            if (textEffect.isTyping && Input.GetMouseButtonDown(0))
            {
                Debug.Log("TextSkipClick");
                textEffect.curText.text = dialogueTexts[i];
                textEffect.isTyping = false;

                StopCoroutine(textRoutine);
                yield return null;
            }
            yield return new WaitUntil(() => !textEffect.isTyping && Input.GetMouseButtonDown(0));
            yield return null;
        }

        StartCoroutine(fadein.Fade(fadeBlack, 0f, 1f));
        yield return new WaitForSeconds(fadein.fadeDuration + fadeDelay);
        SceneLoader.LoadScene("WorldScene");
    }
}
