using UnityEngine;

public class StatUIController : MonoBehaviour
{
    [Header("UI 표시 대상")]
    [SerializeField] private GameObject statsUIObject; // UI GameObject 전체
    [SerializeField] private StatsUI statsUI;
    [SerializeField] private BaseCharacter player;
    [SerializeField] private BaseCharacter pet1;
    [SerializeField] private BaseCharacter pet2;

    private bool ShowStatUI = false;

    private void Start()
    {
        if (statsUIObject == null) return;
        statsUIObject.SetActive(false);
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.C))
        {
            if (ShowStatUI) return;
            if (statsUIObject == null || statsUI == null || player == null) return;

            ShowStatUI = true;
            statsUIObject.SetActive(true);
            statsUI.SetTarget(player);
        }
    }

    // UI 내 돌아가기 버튼에 연결할 함수 (UI 끄기 담당)
    public void CloseStatUI()
    {
        if (statsUIObject == null) return;

        ShowStatUI = false;
        statsUIObject.SetActive(false);
    }

    public void ShowPlayerStats()
    {
        if (statsUIObject == null || statsUI == null || player == null) return;

        if (!ShowStatUI)
        {
            ShowStatUI = true;
            statsUIObject.SetActive(true);
        }

        statsUI.SetTarget(player);
    }

    public void ShowPet1Stats()
    {
        if (statsUIObject == null || statsUI == null || pet1 == null) return;

        if (!ShowStatUI)
        {
            ShowStatUI = true;
            statsUIObject.SetActive(true);
        }

        statsUI.SetTarget(pet1);
    }

    public void ShowPet2Stats()
    {
        if (statsUIObject == null || statsUI == null || pet2 == null) return;

        if (!ShowStatUI)
        {
            ShowStatUI = true;
            statsUIObject.SetActive(true);
        }

        statsUI.SetTarget(pet2);
    }
}