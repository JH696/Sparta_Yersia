using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.GraphicsBuffer;

[System.Serializable]
public class DialogueData
{
    public string DialogueID;
    public List<string> Lines;
}

[System.Serializable]
public class Wrapper<T>
{
    public T[] array;
}

public class JsonHelper : MonoBehaviour
{
    public T[] WrapingJson<T>(string json)
    {
        string newJson = "{\"array\":" + json + "}";
        Wrapper<T> wrappedJson = JsonUtility.FromJson<Wrapper<T>>(newJson);
        return wrappedJson.array;
    }

    public DialogueData[] LoadJsonFromPath(string path)
    {
        TextAsset jsonText = Resources.Load<TextAsset>(path);

        if (jsonText == null)
        {
            Debug.LogError("JSON 파일을 찾을 수 없습니다: " + path);
            return null;
        }

        return WrapingJson<DialogueData>(jsonText.text);
    }

}