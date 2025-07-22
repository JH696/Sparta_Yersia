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
        //fadein.Fade(0f, 1f);
        SceneLoader.LoadScene(EScene.IntroScene);
    }
    public void OnClickLoadGame()
    {

    }

    public void OnClickQuitGame()
    {
        
    }
}
