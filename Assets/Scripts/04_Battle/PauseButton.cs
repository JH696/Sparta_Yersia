using UnityEngine;
using UnityEngine.UI;
using static UnityEngine.InputManagerEntry;

public class PauseButton : MonoBehaviour
{
    [Header("버튼")]
    [SerializeField] private Button button;

    [Header("버튼 이미지")]
    [SerializeField] private Image buttonImage;

    [Header("상태별 아이콘")]
    [SerializeField] private Sprite pauseIcon;
    [SerializeField] private Sprite playIcon;

    [Header("블라인드 오브젝트")]
    [SerializeField] private GameObject blind;

    public void Start()
    {
        button.onClick.AddListener(OnPauseButton);
    }

    public void OnPauseButton()
    {
        blind.SetActive(true);
        button.onClick.RemoveAllListeners();    
        button.onClick.AddListener(OnPlayButton);
        buttonImage.sprite = playIcon;
        Time.timeScale = 0f;
    }

    public void OnPlayButton()
    {
        blind.SetActive(false);
        button.onClick.RemoveAllListeners();
        button.onClick.AddListener(OnPauseButton);
        buttonImage.sprite = pauseIcon;
        Time .timeScale = 1f;   
    }
}
