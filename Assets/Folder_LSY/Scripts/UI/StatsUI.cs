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
    [SerializeField] private TMP_Text YPTxt;
    [SerializeField] private GameObject PlayerInfo;

    [Header("펫 전용 UI")]
    [SerializeField] private GameObject PetInfo;
    [SerializeField] private TMP_Text PetNameTxt;
    [SerializeField] private TMP_Text EvoStageTxt;
    [SerializeField] private Image[] EvoIcons = new Image[3];

    [Header("기본값 (널일 때 표시용)")]
    [SerializeField] private string defaultPetName = "이름";
    [SerializeField] private string defaultEvoStage = "성장 단계";

    private BaseCharacter currentCharacter;

    // UI에 표시할 대상 캐릭터 설정
    public void SetTarget(BaseCharacter character)
    {
        currentCharacter = character; // null이어도 그대로 저장
        RefreshUI();
    }

    // 현재 캐릭터의 정보를 기반으로 UI 갱신
    public void RefreshUI()
    {
        bool isPlayer = currentCharacter is PlayerController;
        bool isPet = currentCharacter is PetController;

        // 공통 스탯 표시
        if (currentCharacter != null)
        {
            HpTxt.text = $"{Mathf.RoundToInt(currentCharacter.CurrentHp)} / {Mathf.RoundToInt(currentCharacter.MaxHp)}";
            MpTxt.text = $"{Mathf.RoundToInt(currentCharacter.CurrentMana)} / {Mathf.RoundToInt(currentCharacter.MaxMana)}";
            AttackTxt.text = $"{Mathf.RoundToInt(currentCharacter.Attack)}";
            DefenseTxt.text = $"{Mathf.RoundToInt(currentCharacter.Defense)}";
            LuckTxt.text = $"{Mathf.RoundToInt(currentCharacter.Luck)}";
            SpeedTxt.text = $"{Mathf.RoundToInt(currentCharacter.Speed)}";
        }
        else
        {
            HpTxt.text = "";
            MpTxt.text = "";
            AttackTxt.text = "";
            DefenseTxt.text = "";
            LuckTxt.text = "";
            SpeedTxt.text = "";
        }

        // 플레이어 정보 UI 활성화
        if (isPlayer)
        {
            PlayerController player = currentCharacter as PlayerController;

            if (YPTxt != null) YPTxt.text = $"YP : {player.YP}";
            if (ProfileImg != null && player.PlayerData != null)
                ProfileImg.sprite = player.PlayerData.GetDefaultProfileIcon();

            if (PlayerInfo != null) PlayerInfo.SetActive(true);
            if (PetInfo != null) PetInfo.SetActive(false);  // 플레이어일 땐 펫 UI 꺼짐
        }

        // 펫 정보 UI 활성화 (진화 단계 포함)
        else if (isPet)
        {
            PetController pet = currentCharacter as PetController;

            if (ProfileImg != null && pet.PetData != null)
                ProfileImg.sprite = pet.PetData.GetCurrentProfileIcon();
            else if (ProfileImg != null)
                ProfileImg.sprite = defaultPetProfile;

            if (PetInfo != null) PetInfo.SetActive(true);   // 펫이면 펫 UI 켜짐
            if (PlayerInfo != null) PlayerInfo.SetActive(false);

            if (PetNameTxt != null)
                PetNameTxt.text = pet.PetData?.PetName ?? defaultPetName;

            if (EvoStageTxt != null)
                EvoStageTxt.text = pet.PetData != null
                    ? $"성장 단계 : {pet.PetData.CurrentEvoStage + 1}"
                    : defaultEvoStage;

            if (EvoIcons != null)
            {
                for (int i = 0; i < EvoIcons.Length; i++)
                {
                    if (EvoIcons[i] == null) continue;
                    Sprite icon = (pet.PetData?.sprites != null && i < pet.PetData.sprites.Length)
                        ? pet.PetData.sprites[i].Icon
                        : null;

                    EvoIcons[i].sprite = icon;
                    EvoIcons[i].color = (icon != null) ? Color.white : new Color(1, 1, 1, 0);
                }
            }
        }

        // null 또는 기타 대상 처리
        else
        {
            if (PlayerInfo != null) PlayerInfo.SetActive(false);
            if (PetInfo != null) PetInfo.SetActive(true);  // 널이어도 펫 UI는 켜짐

            if (ProfileImg != null)
                ProfileImg.sprite = defaultPetProfile;

            if (PetNameTxt != null)
                PetNameTxt.text = defaultPetName;

            if (EvoStageTxt != null)
                EvoStageTxt.text = defaultEvoStage;

            if (EvoIcons != null)
            {
                for (int i = 0; i < EvoIcons.Length; i++)
                {
                    if (EvoIcons[i] == null) continue;
                    EvoIcons[i].sprite = null;
                    EvoIcons[i].color = new Color(1, 1, 1, 0);
                }
            }
        }
    }
}