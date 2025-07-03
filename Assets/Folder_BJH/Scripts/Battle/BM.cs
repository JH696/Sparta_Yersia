using UnityEngine;
using UnityEngine.TextCore.Text;

public class Test_BattleManager : MonoBehaviour
{
    public static Test_BattleManager Instance;

    [Header("배틀 UI")]
    public BattleUI battleUI;

    public event System.Action OnAction;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else if (Instance != this)
        {
            Destroy(gameObject);
        }
    }

    public void StartAction(CharacterSlot slot)
    {
        Debug.Log("액션 시작");
        battleUI.ShowActionButtons(slot);
        OnAction?.Invoke();
    }
}
