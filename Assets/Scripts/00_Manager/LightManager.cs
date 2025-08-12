using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LightManager : MonoBehaviour
{
    public static LightManager Instance { get; private set; }

    [Header("패배 시 켤 양호실 조명")]
    [SerializeField] private GameObject infirmaryLight;

    [Header("층별 승리 시 켤 조명 (예: Upper=B1, Middle=B2, Lower=… )")]
    [SerializeField] private GameObject upperVictoryLight;
    [SerializeField] private GameObject middleVictoryLight;
    [SerializeField] private GameObject lowerVictoryLight;

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

    // ===== 외부 API =====

    // 배틀 시작 직전: 현재 상태 스냅샷 + 전체 소등
    public void OnBattleStartSnapshot()
    {
        SnapshotForBattle();
        DeactivateAll();
    }

    // 배틀 종료 후: 승/패 + 층에 따른 처리
    public void OnBattleEnd(bool isWin, E_StageType stage)
    {
        if (isWin)
        {
            var stageLight = GetStageVictoryLight(stage);
            if (stageLight != null)
            {
                Activate(stageLight);
            }
            else
            {
                // 스테이지 조명이 지정되지 않았다면 스냅샷 기준으로 복원
                RestoreAfterVictory();
            }
        }
        else
        {
            // 패배 시: 양호실 조명 켜고 기록(씬/글로벌)
            if (infirmaryLight != null)
                Activate(infirmaryLight);
            else
                RestoreAfterVictory(); // 폴백
        }
    }

    // 모든 라이트 OFF (PlayerLightController 경유)
    public void DeactivateAll()
    {
        EnsurePLC();
        plc?.SetByPortal(false, null);
    }

    // 특정 라이트 ON (기억 포함)
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
            // 전역은 유지
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

        // 전역도 갱신
        lastScene_Global = sceneName;
        lastObjectName_Global = objName;
    }

    public void SnapshotForBattle()
    {
        battleSnap_Scene = lastScene_Global;
        battleSnap_Object = lastObjectName_Global;
    }

    // 배틀 승리시: 스냅샷 기준 복구
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

    // ===== 내부 유틸 =====

    private GameObject GetStageVictoryLight(E_StageType stage)
    {
        switch (stage)
        {
            case E_StageType.Upper: return upperVictoryLight;
            case E_StageType.Middle: return middleVictoryLight;
            case E_StageType.Lower: return lowerVictoryLight;
            default: return null;
        }
    }

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
