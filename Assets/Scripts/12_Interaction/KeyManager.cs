using UnityEngine;

public static class KeyManager
{
    private const string KeyPref = "[F] 열기";

    // 키 얻었을 때 호출 (퀘스트 완료 지점)
    public static void ObtainKey()
    {
        PlayerPrefs.SetInt(KeyPref, 1);
        PlayerPrefs.Save();
    }

    // 키 소비할 때 호출
    public static void ConsumeKey()
    {
        PlayerPrefs.SetInt(KeyPref, 0);
        PlayerPrefs.Save();
    }

    // 키 보유 여부 확인
    public static bool HasKey()
    {
        return PlayerPrefs.GetInt(KeyPref, 0) == 1;
    }
}
