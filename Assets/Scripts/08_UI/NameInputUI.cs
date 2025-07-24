using TMPro;
using UnityEngine;

public class NameInputUI : MonoBehaviour
{
    [SerializeField] private TMP_InputField nameField;
    private Player player;
    private StatsUI statsUI;
    private PlayerUI playerUI;

    public void Init(Player player, StatsUI statsUI, PlayerUI playerUI)
    {
        this.player = player;
        this.statsUI = statsUI;
        this.playerUI = playerUI;
    }

    public void OnConfirmButton()
    {
        string enteredName = nameField.text;

        if (!string.IsNullOrEmpty(enteredName))
        {
            //player?.SetPlayerName(enteredName);
            statsUI?.RefreshUI();
            playerUI?.RefreshUI();
        }

        gameObject.SetActive(false);
    }
}