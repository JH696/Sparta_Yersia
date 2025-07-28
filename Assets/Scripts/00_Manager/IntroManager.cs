using System.Collections;
using UnityEngine;

public class IntroManager : MonoBehaviour
{

    FadeIn fadein;
    CameraFollow cameraFollow;
    [SerializeField] private CanvasGroup fade;  //로고+버튼들
    [SerializeField] private CanvasGroup fadeBlack;  //까만화면
    [SerializeField] private float fadeDelay;


    // Start is called before the first frame update
    private void Awake()
    {
        cameraFollow = GetComponent<CameraFollow>();
        fadein = GetComponent<FadeIn>();
    }
    // Start is called before the first frame update
    void Start()
    {
        foreach (var data in cameraFollow.images)
        {
            data.backGround.anchoredPosition = data.startPosition;
            data.backGround.gameObject.SetActive(false);
        }
        StartCoroutine(FadeInAndScrollAndFadeOut());
    }

    // Update is called once per frame
    void Update()
    {

    }

    public IEnumerator FadeInAndScrollAndFadeOut()
    {
        foreach (var data in cameraFollow.images)
        {
            cameraFollow.image = data;
            data.backGround.gameObject.SetActive(true);
            StartCoroutine(fadein.Fade(fadeBlack, 1f, 0f));
            yield return new WaitForSeconds(fadein.fadeDuration + fadeDelay);
            StartCoroutine(cameraFollow.Scroll());
            //텍스트 출력
            yield return new WaitForSeconds(cameraFollow.image.duration + fadeDelay);
            StartCoroutine(fadein.Fade(fadeBlack, 0f, 1f));
            yield return new WaitForSeconds(fadein.fadeDuration + fadeDelay);
            data.backGround.gameObject.SetActive(false);

        }
    }
}
