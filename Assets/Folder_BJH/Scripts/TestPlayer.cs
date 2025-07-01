using UnityEngine;

public class TestPlayer : MonoBehaviour
{
    public static TestPlayer Instance { get; private set; }

    public PlayerQuest playerQuest;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }
}

