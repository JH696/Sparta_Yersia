using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class StatsUI : MonoBehaviour
{
    [Header("스탯 텍스트")] // StatText
    [SerializeField] private TMP_Text HpTxt;
    [SerializeField] private TMP_Text MpTxt;
    [SerializeField] private TMP_Text AttackTxt;
    [SerializeField] private TMP_Text DefenseTxt;
    [SerializeField] private TMP_Text LuckTxt;
    [SerializeField] private TMP_Text SpeedTxt;

    [Header("프로필 이미지")]
    [SerializeField] private Image ProfileImg;
    [SerializeField] private Sprite defaultPetProfile; // 기본 펫 프로필 이미지

    [Header("플레이어 전용 UI")]
    [SerializeField] private TMP_Text PlayerNameTxt;
    [SerializeField] private TMP_Text YPTxt;
    [SerializeField] private TMP_Text GenderTxt;
    [SerializeField] private TMP_Text TierTxt;
    [SerializeField] private GameObject PlayerInfo;

    [Header("펫 전용 UI")]
    [SerializeField] private GameObject PetInfo;
    [SerializeField] private TMP_Text PetNameTxt;
    [SerializeField] private TMP_Text EvoStageTxt;
    [SerializeField] private Image[] EvoIcons = new Image[3];

    [Header("진화 단계 미달성 시 대체 이미지")]
    [SerializeField] private Sprite unknownIcon;  // '?' 이미지

    [Header("레벨 UI")]
    [SerializeField] private TMP_Text LevelTxt;

    [Header("기본값 (널일 때 표시용)")]
    [SerializeField] private string defaultPetName = "이름";
    [SerializeField] private string defaultEvoStage = "성장 단계";

    private CharacterStatus currentCharacter;

    public void SetTarget(CharacterStatus character)
    {
        currentCharacter = character; // null이어도 그대로 저장
        RefreshUI();
    }

    public void RefreshUI()
    {
        if (currentCharacter == null)
        {
            ClearUI();
            return;
        }

        // 공통 스탯 표시
        HpTxt.text = $"{Mathf.RoundToInt(currentCharacter.stat.CurrentHp)} / {Mathf.RoundToInt(currentCharacter.stat.MaxHp)}";
        MpTxt.text = $"{Mathf.RoundToInt(currentCharacter.stat.CurrentMana)} / {Mathf.RoundToInt(currentCharacter.stat.MaxMana)}";
        AttackTxt.text = $"{Mathf.RoundToInt(currentCharacter.stat.Attack)}";
        DefenseTxt.text = $"{Mathf.RoundToInt(currentCharacter.stat.Defense)}";
        LuckTxt.text = $"{Mathf.RoundToInt(currentCharacter.stat.Luck)}";
        SpeedTxt.text = $"{Mathf.RoundToInt(currentCharacter.stat.Speed)}";

        // Player인지 확인
        var playerStatus = currentCharacter as PlayerStatus;
        var petStatus = currentCharacter as PetStatus;

        if (playerStatus != null)
        {
            DrawPlayerUI(playerStatus);
        }
        else if (petStatus != null)
        {
            DrawPetUI(petStatus);
        }
        else
        {
            ClearUI();
        }

        if (playerStatus != null)
        {
            if (LevelTxt != null)
                LevelTxt.text = $"Lv {playerStatus.stat.Level}";
        }
        else if (petStatus != null)
        {
            if (LevelTxt != null)
                LevelTxt.text = $"Lv {petStatus.stat.Level}";
        }
        else
        {
            if (LevelTxt != null)
                LevelTxt.text = "Lv";
        }
    }

    private void DrawPlayerUI(PlayerStatus playerStatus)
    {
        // 닉네임
        PlayerNameTxt.text = playerStatus.PlayerName;

        // 선택사항: 성별 표시
        if (GenderTxt != null)
            GenderTxt.text = $"성별: {playerStatus.PlayerData.gender}";

        // 프로필 아이콘 (갈색 vs 다크)
        bool isExpert = playerStatus.Rank == E_Rank.Expert;
        ProfileImg.sprite = isExpert
            ? playerStatus.PlayerData.darkProfileIcon
            : playerStatus.PlayerData.brownProfileIcon;

        // 등급 텍스트
        switch (playerStatus.Rank)
        {
            case E_Rank.Basic:
                TierTxt.text = "등급 : 초급 마법사"; break;
            case E_Rank.Advanced:
                TierTxt.text = "등급 : 중급 마법사"; break;
            case E_Rank.Expert:
                TierTxt.text = "등급 : 상급 마법사"; break;
            default:
                TierTxt.text = "등급 : 알 수 없음"; break;
        }

        // 플레이어 정보 패널 활성화 / 펫 정보 비활성
        PlayerInfo.SetActive(true);
        PetInfo.SetActive(false);
    }

    private void DrawPetUI(PetStatus petStatus)
    {
        if (PetNameTxt.text != null)
            PetNameTxt.text = petStatus.PetData?.PetName ?? defaultPetName;

        // Pet UI 업데이트
        if (ProfileImg != null && petStatus.PetData != null)
            ProfileImg.sprite = petStatus.GetCurrentProfileIcon();
        else if (ProfileImg != null)
            ProfileImg.sprite = defaultPetProfile;

        if (PetNameTxt != null) // 레벨도 같이 표시되게 해야함
            PetNameTxt.text = petStatus.PetData?.PetName ?? defaultPetName;

        if (EvoStageTxt != null)
            EvoStageTxt.text = petStatus.PetData != null
                ? $"성장 단계 : {petStatus.EvoLevel + 1}"
                : defaultEvoStage;

        if (EvoIcons != null)
        {
            for (int i = 0; i < EvoIcons.Length; i++)
            {
                if (EvoIcons[i] == null) continue;

                if (petStatus.PetData?.sprites != null && i < petStatus.PetData.sprites.Length)
                {
                    bool isReached = petStatus.EvoLevel >= i;
                    EvoIcons[i].sprite = isReached ? petStatus.PetData.sprites[i].ProfileIcon : unknownIcon;
                    EvoIcons[i].color = Color.white;
                }
                else
                {
                    EvoIcons[i].sprite = unknownIcon;
                    EvoIcons[i].color = Color.white;
                }
            }
        }

        if (PlayerInfo != null) PlayerInfo.SetActive(false);
        if (PetInfo != null) PetInfo.SetActive(true);
    }

    private void ClearUI()
    {
        HpTxt.text = "";
        MpTxt.text = "";
        AttackTxt.text = "";
        DefenseTxt.text = "";
        LuckTxt.text = "";
        SpeedTxt.text = "";

        if (PlayerInfo != null) PlayerInfo.SetActive(false);
        if (PetInfo != null) PetInfo.SetActive(false);

        if (ProfileImg != null) ProfileImg.sprite = defaultPetProfile;

        if (PetNameTxt != null) PetNameTxt.text = defaultPetName;
        if (EvoStageTxt != null) EvoStageTxt.text = defaultEvoStage;

        if (EvoIcons != null)
        {
            for (int i = 0; i < EvoIcons.Length; i++)
            {
                if (EvoIcons[i] == null) continue;
                EvoIcons[i].sprite = null;
                EvoIcons[i].color = new Color(1, 1, 1, 0);
            }
        }

        if (LevelTxt != null) LevelTxt.text = "Lv";
    }
}