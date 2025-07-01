using UnityEngine;

public class StatUIController : MonoBehaviour
{
    [Header("UI 표시 대상")]
    [SerializeField] private GameObject statsUIObject; // UI GameObject 전체
    [SerializeField] private StatsUI statsUI;
    [SerializeField] private BaseCharacter player;
    [SerializeField] private BaseCharacter pet1;
    [SerializeField] private BaseCharacter pet2;

    [Header("추가 UI 오브젝트")]
    [SerializeField] private GameObject PlayerInfo;    // 플레이어 전용 정보 UI
    [SerializeField] private GameObject PetInfo;       // 펫 전용 정보 UI

    private bool ShowStatUI = false;

    private void Start()
    {
        if (statsUIObject == null) return;

        statsUIObject.SetActive(false);
        if (PlayerInfo != null) PlayerInfo.SetActive(false);
        if (PetInfo != null) PetInfo.SetActive(false);
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

            if (PlayerInfo != null) PlayerInfo.SetActive(true);
            if (PetInfo != null) PetInfo.SetActive(false);
        }
    }

    // UI 내 돌아가기 버튼에 연결할 함수 (UI 끄기 담당)
    public void CloseStatUI()
    {
        if (statsUIObject == null) return;

        ShowStatUI = false;
        statsUIObject.SetActive(false);

        if (PlayerInfo != null) PlayerInfo.SetActive(false);
        if (PetInfo != null) PetInfo.SetActive(false);
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

        if (PlayerInfo != null) PlayerInfo.SetActive(true);
        if (PetInfo != null) PetInfo.SetActive(false);
    }

    public void ShowPet1Stats()
    {
        if (statsUIObject == null || statsUI == null) return;

        if (!ShowStatUI)
        {
            ShowStatUI = true;
            statsUIObject.SetActive(true);
        }

        // null이어도 전달
        statsUI.SetTarget(pet1);
        if (PlayerInfo != null) PlayerInfo.SetActive(false);
        if (PetInfo != null) PetInfo.SetActive(true);
    }

    public void ShowPet2Stats()
    {
        if (statsUIObject == null || statsUI == null) return;

        if (!ShowStatUI)
        {
            ShowStatUI = true;
            statsUIObject.SetActive(true);
        }

        // null이어도 전달
        statsUI.SetTarget(pet2);
        if (PlayerInfo != null) PlayerInfo.SetActive(false);
        if (PetInfo != null) PetInfo.SetActive(true);
    }
}