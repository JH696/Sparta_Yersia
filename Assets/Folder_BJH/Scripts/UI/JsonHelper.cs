using System.Collections.Generic;
using UnityEngine;

// 
[System.Serializable]
public class DialogueLine
{
    public string Speaker;
    public string Text;
}

// 분리된 문자를 저장할 데이터 클래스
[System.Serializable]
public class DialogueData
{
    public string DialogueID;
    public List<DialogueLine> Lines;
}

// json속 문자를 분리해 보관할 클래스
[System.Serializable]
public class Wrapper<T>
{
    public T[] array;
}

public class JsonHelper : MonoBehaviour
{
    // 지정된 경로에서 JSON 파일을 로드하고 DialogueData 배열로 변환
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

    // JSON 문자열을 T 타입의 배열로 변환
    private T[] WrapingJson<T>(string json)
    {
        string newJson = "{\"array\":" + json + "}";
        Wrapper<T> wrappedJson = JsonUtility.FromJson<Wrapper<T>>(newJson);
        return wrappedJson.array;
    }
}