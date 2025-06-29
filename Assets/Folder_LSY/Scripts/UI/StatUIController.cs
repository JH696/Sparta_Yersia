using UnityEngine;

public class StatUIController : MonoBehaviour
{
    [Header("UI 표시 대상")]
    [SerializeField] private StatsUI statsUI;
    [SerializeField] private BaseCharacter player;
    [SerializeField] private BaseCharacter pet1;
    [SerializeField] private BaseCharacter pet2;

    private void Start()
    {
        // 시작 시 플레이어 스탯 표시
        statsUI.SetTarget(player);
    }

    public void ShowPlayerStats()
    {
        statsUI.SetTarget(player);
    }

    public void ShowPet1Stats()
    {
        statsUI.SetTarget(pet1);
    }

    public void ShowPet2Stats()
    {
        statsUI.SetTarget(pet2);
    }
}
