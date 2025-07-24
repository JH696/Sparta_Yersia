using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StartManager : MonoBehaviour
{
    FadeIn fadein;
    CameraFollow cameraFollow;
    [SerializeField] private CanvasGroup fade;  //로고+버튼들
    [SerializeField] private CanvasGroup fadeBlack;  //까만화면
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

    // Update is called once per frame
    void Update()
    {
        
    }

    IEnumerator ScrollAndFadeInUI()
    {
        StartCoroutine(cameraFollow.Scroll());
        yield return new WaitForSeconds(cameraFollow.duration);
        StartCoroutine(fadein.Fade(fade, 0f, 1f));
    }
    public void OnClickNewGame()
    {
        StartCoroutine(FadeOutAndLoadScene());
    }
    public void OnClickLoadGame()
    {

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
        yield return new WaitForSeconds(fadein.fadeDuration);
//유니티 에디터일 경우 플레이모드 종료
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
//아니면 어플리케이션 종료
#else
        Application.Quit();
#endif
    }
}
