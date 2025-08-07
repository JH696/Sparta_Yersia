using UnityEngine.SceneManagement;

public static class SceneLoader
{
    // 씬 로드
    public static void LoadScene(string scene)
    {
        if (!CheckScene(scene)) return;

        SceneManager.LoadScene(scene);
    }

    // 멀티 씬 로드
    public static void MultipleLoadScene(string scene)
    {
        if (!CheckScene(scene)) return;

        SceneManager.LoadScene(scene, LoadSceneMode.Additive);
    }

    // 멀티 씬 언로드
    public static void UnloadScene(string scene)
    {
        if (!CheckScene(scene)) return;

        var targetScene = SceneManager.GetSceneByName(scene);
        if (targetScene.isLoaded)
        {
            SceneManager.UnloadSceneAsync(targetScene);
        }
    }

    // Build Settings에 해당 씬이 존재하는지 확인
    private static bool CheckScene(string sceneName)
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