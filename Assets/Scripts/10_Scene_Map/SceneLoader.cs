using UnityEngine.SceneManagement;

public static class SceneLoader
{
    // 현재 로드된 씬을 나타내는 enum 값
    public static EScene CurrentScene { get; private set; } = EScene.Scene_LSY; // 기본 초기값 설정

    // Enum을 기반으로 씬을 로드
    public static void LoadScene(string scene)
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