using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LightManager : MonoBehaviour
{
    public static LightManager Instance { get; private set; }

    // 씬별 마지막으로 켰던 라이트 이름
    private readonly Dictionary<string, string> lastExtraByScene = new Dictionary<string, string>();

    // 전역 마지막 활성 라이트 (씬+오브젝트 이름)
    private string lastScene_Global;
    private string lastObjectName_Global;

    // 배틀 진입시 복원용
    private string battleSnap_Scene;
    private string battleSnap_Object;

    private PlayerLightController plc;

    private void Awake()
    {
        if (Instance != null && Instance != this) { Destroy(gameObject); return; }
        Instance = this;
        DontDestroyOnLoad(gameObject);
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    private void OnDestroy()
    {
        if (Instance == this) Instance = null;
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    private void EnsurePLC()
    {
        if (plc != null) return;
        plc = PlayerLightController.Instance ?? FindObjectOfType<PlayerLightController>(true);
    }

    private void OnSceneLoaded(Scene s, LoadSceneMode mode)
    {
        // 씬 전환 시, 그 씬에서 마지막에 켰었던 라이트가 있으면 복구
        RestoreForScene(s.name);
    }

    //외부 API

    // 모든 라이트 OFF (PlayerLightController 경유)
    public void DeactivateAll()
    {
        EnsurePLC();
        plc?.SetByPortal(false, null);
    }

    public void Activate(GameObject extraToActivate)
    {
        EnsurePLC();

        plc?.SetByPortal(true, extraToActivate);

        string sceneName = SceneManager.GetActiveScene().name;

        if (extraToActivate != null)
        {
            string name = extraToActivate.name;
            lastExtraByScene[sceneName] = name;

            lastScene_Global = sceneName;
            lastObjectName_Global = name;
        }
        else
        {
            lastExtraByScene.Remove(sceneName);
            // 전역은 유지 (null로 지우지 않음)
        }
    }

    // 포탈/임의 호출: 현재 씬의 마지막 라이트 복구
    public void RestoreForScene(string sceneName)
    {
        EnsurePLC();

        if (!lastExtraByScene.TryGetValue(sceneName, out var objName))
        {
            // 없으면 싹 끈다
            plc?.SetByPortal(false, null);
            return;
        }

        var go = FindSceneObjectByName(objName);
        plc?.SetByPortal(true, go);

        // 전역도 갱신(방문한 씬의 마지막 라이트가 최신이 된다)
        lastScene_Global = sceneName;
        lastObjectName_Global = objName;
    }

    public void SnapshotForBattle()
    {
        battleSnap_Scene = lastScene_Global;
        battleSnap_Object = lastObjectName_Global;
    }

    // 배틀 승리시
    public void RestoreAfterVictory()
    {
        EnsurePLC();

        if (string.IsNullOrEmpty(battleSnap_Scene) || string.IsNullOrEmpty(battleSnap_Object))
        {
            RestoreForScene(SceneManager.GetActiveScene().name);
            return;
        }

        var go = FindSceneObjectByName(battleSnap_Object);
        if (go == null)
        {
            RestoreForScene(SceneManager.GetActiveScene().name);
            return;
        }

        plc?.SetByPortal(true, go);

        // 전역/씬 기록 갱신
        lastScene_Global = battleSnap_Scene;
        lastObjectName_Global = battleSnap_Object;
        lastExtraByScene[lastScene_Global] = lastObjectName_Global;
    }

    // 배틀 패배시
    public void RestoreAfterDefeat(GameObject infirmaryLight)
    {
        if (infirmaryLight == null)
        {
            RestoreAfterVictory();
            return;
        }

        Activate(infirmaryLight); // Activate가 전역/씬 기록까지 갱신함
    }

    // 내부 유틸
    private GameObject FindSceneObjectByName(string name)
    {
        if (string.IsNullOrEmpty(name)) return null;

        var all = Resources.FindObjectsOfTypeAll<GameObject>();
        foreach (var go in all)
        {
            if (go.hideFlags != HideFlags.None) continue;
            if (go.name == name && go.scene.IsValid())
                return go;
        }
        return null;
    }
}
