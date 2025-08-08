using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Rendering.Universal;

public class PlayerLightController : MonoBehaviour
{
    public static PlayerLightController Instance { get; private set; }

    [Header("플레이어 Light2D")]
    [SerializeField] private Light2D playerLight;

    private GameObject currentExtraObj;

    void Awake()
    {
        if (Instance != null && Instance != this) { Destroy(this); return; }
        Instance = this;

        if (playerLight == null)
            playerLight = GetComponentInChildren<Light2D>(true);

        ForceOff();
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    void OnDestroy()
    {
        if (Instance == this) Instance = null;
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    private void OnSceneLoaded(Scene s, LoadSceneMode m)
    {
        // 씬이 바뀌면 무조건 꺼줌
        ForceOff();
    }

    public void ForceOff()
    {
        if (playerLight != null) playerLight.enabled = false;
        if (currentExtraObj != null) currentExtraObj.SetActive(false);
        currentExtraObj = null;
    }

    /// <summary>
    /// 포탈을 타서 도착했을 때만 호출.
    /// </summary>
    public void SetByPortal(bool on, GameObject extraToActivate = null)
    {
        if (!on)
        {
            ForceOff();
            return;
        }

        if (playerLight != null) playerLight.enabled = true;

        // 이전에 켠 오브젝트 끄기
        if (currentExtraObj != null && currentExtraObj != extraToActivate)
            currentExtraObj.SetActive(false);

        // 새 오브젝트 켜기(있다면)
        if (extraToActivate != null)
        {
            extraToActivate.SetActive(true);
            currentExtraObj = extraToActivate;
        }
        else
        {
            currentExtraObj = null;
        }
    }
}
