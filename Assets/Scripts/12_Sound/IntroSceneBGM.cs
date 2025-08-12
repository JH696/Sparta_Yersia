using UnityEngine;

public class IntroSceneBGM : MonoBehaviour
{
    [SerializeField] private AudioClip introSceneBGM;

    private void Start()
    {
        if (SoundManager.Instance != null)
        {
            SoundManager.Instance.PlayBGM(introSceneBGM, loop: true);
        }
    }
}