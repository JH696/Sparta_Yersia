using UnityEngine;

public class StartSceneBGM : MonoBehaviour
{
    [SerializeField] private AudioClip startSceneBGM;

    private void Start()
    {
        if (SoundManager.Instance != null)
        {
            SoundManager.Instance.PlayBGM(startSceneBGM, loop: true);
        }
    }
}