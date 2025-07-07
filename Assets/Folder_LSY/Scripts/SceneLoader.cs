using UnityEngine;
using UnityEngine.SceneManagement;

public static class SceneLoader
{
    // Enum을 기반으로 씬을 로드합니다.
    public static void LoadScene(EScene scene)
    {
        string sceneName = scene.ToString();

        if (!IsSceneInBuildSettings(sceneName)) return;

        SceneManager.LoadScene(sceneName);
    }

    // Build Settings에 해당 씬이 존재하는지 확인
    private static bool IsSceneInBuildSettings(string sceneName)
    {
        int sceneCount = SceneManager.sceneCountInBuildSettings;

        for (int i = 0; i < sceneCount; i++)
        {
            string path = SceneUtility.GetScenePathByBuildIndex(i);
            string fileName = System.IO.Path.GetFileNameWithoutExtension(path);

            if (fileName == sceneName) return true;
        }

        return false;
    }
}