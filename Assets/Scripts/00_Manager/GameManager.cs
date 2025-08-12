using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;

    public static PlayerStatus player;

    private void Awake()
    {
        // 싱글톤 설정
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            //GlobalSaveManager.Save(Player.GetComponent<Player>());
        }
    }
}