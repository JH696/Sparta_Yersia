using System.Collections;
using UnityEngine;

public class StartManager : MonoBehaviour
{
    FadeIn fadein;
    CameraFollow cameraFollow;
    [SerializeField] private CanvasGroup fade;  //로고+버튼들
    [SerializeField] private CanvasGroup fadeBlack;  //까만화면
    [SerializeField] private float fadeDelay = 0.5f;  //
    [SerializeField] private GameObject helpPanel;
    // Start is called before the first frame update
    private void Awake()
    {
        cameraFollow = GetComponent<CameraFollow>();
        fadein = GetComponent<FadeIn>();
    }
    void Start()
    {
        StartCoroutine(ScrollAndFadeInUI());
    }

    IEnumerator ScrollAndFadeInUI()
    {
        StartCoroutine(cameraFollow.Scroll());
        yield return new WaitForSeconds(cameraFollow.image.duration + fadeDelay);
        StartCoroutine(fadein.Fade(fade, 0f, 1f));
    }

    public void OnClickHelp()
    {
        helpPanel.SetActive(!helpPanel.activeSelf);
    }

    public void OnClickNewGame()
    {
        StartCoroutine(FadeOutAndLoadScene());
    }

    public void OnClickLoadGame()
    {
        //불러오기
    }

    public void OnClickQuitGame()
    {
        StartCoroutine(FadeOutAndQuit());
    }

    IEnumerator FadeOutAndLoadScene()
    {
        StartCoroutine(fadein.Fade(fadeBlack, 0f, 1f));
        yield return new WaitForSeconds(fadein.fadeDuration);
        SceneLoader.LoadScene("IntroScene");
    }

    IEnumerator FadeOutAndQuit()
    {
        StartCoroutine(fadein.Fade(fadeBlack, 0f, 1f));
        yield return new WaitForSeconds(fadein.fadeDuration + fadeDelay);
//유니티 에디터일 경우 플레이모드 종료
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
//아니면 어플리케이션 종료
#else
        Application.Quit();
#endif
    }

}
