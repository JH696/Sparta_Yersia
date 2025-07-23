using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IntroManager : MonoBehaviour
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
    // Start is called before the first frame update
    void Start()
    {
        cameraFollow.Scroll();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
