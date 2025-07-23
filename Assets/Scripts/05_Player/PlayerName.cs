using UnityEngine;

public class PlayerName : MonoBehaviour
{
    [SerializeField] private Player player;
    [SerializeField] private NameInputUI nameInputUI;
    [SerializeField] private StatsUI statsUI;
    [SerializeField] private PlayerUI playerUI;

    private void Start()
    {
        nameInputUI.Init(player, statsUI, playerUI);
        nameInputUI.gameObject.SetActive(true); // 게임 시작 시 자동 표시
    }
}