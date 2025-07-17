using UnityEngine;

public class TestSceneLoad : MonoBehaviour
{
    public Player player; // Player 컴포넌트를 할당해야 합니다.
    private void Start()
    {
        player = GameManager.Instance.Player.GetComponent<Player>();

        GlobalSaveManager.Load(player);
    }
}