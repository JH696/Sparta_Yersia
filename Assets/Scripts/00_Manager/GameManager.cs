using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;

    public GameObject Player;

    private void Awake()
    {
        // 싱글톤 설정
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;
        DontDestroyOnLoad(gameObject);

        if (Player != null)
        {
            DontDestroyOnLoad(Player);
            Debug.Log(Player.name);
        }
        else
        {
            Debug.LogWarning("Player not found in scene.");
        }
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            GlobalSaveManager.Save(Player.GetComponent<Player>());
        }
    }
}