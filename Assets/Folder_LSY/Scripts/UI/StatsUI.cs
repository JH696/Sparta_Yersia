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

    private BaseCharacter currentCharacter;

    public void SetTarget(BaseCharacter character)
    {
        currentCharacter = character; // null이어도 그대로 저장
        RefreshUI();
    }

    public void RefreshUI()
    {
        bool isPlayer = currentCharacter is Player;
        bool isPet = currentCharacter is Pet;

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

        if (isPlayer)
        {
            Player player = currentCharacter as Player;

            if (YPTxt != null) YPTxt.text = $"YP : {player.YP}";
            if (ProfileImg != null && player.PlayerData != null)
                ProfileImg.sprite = player.PlayerData.Icon;

            if (GenderTxt != null)
            {
                switch (player.Gender)
                {
                    case EGender.Male:
                        GenderTxt.text = "성별 : 남성";
                        break;
                    case EGender.Female:
                        GenderTxt.text = "성별 : 여성";
                        break;
                    default:
                        GenderTxt.text = "성별 : 알 수 없음";
                        break;
                }
            }

            if (TierTxt != null && player.PlayerData is PlayerData playerData)
            {
                switch (playerData.tier)
                {
                    case ETier.Basic:
                        TierTxt.text = "등급 : 초급 마법사";
                        break;
                    case ETier.Advanced:
                        TierTxt.text = "등급 : 중급 마법사";
                        break;
                    case ETier.Expert:
                        TierTxt.text = "등급 : 상급 마법사";
                        break;
                    default:
                        TierTxt.text = "등급 : 알 수 없음";
                        break;
                }
            }

            if (PlayerInfo != null) PlayerInfo.SetActive(true);
            if (PetInfo != null) PetInfo.SetActive(false);
        }
        else if (isPet)
        {
            Pet pet = currentCharacter as Pet;

            if (ProfileImg != null && pet.PetData != null)
                ProfileImg.sprite = pet.PetData.GetCurrentProfileIcon();
            else if (ProfileImg != null)
                ProfileImg.sprite = defaultPetProfile;

            if (PetInfo != null) PetInfo.SetActive(true);
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

                    if (pet.PetData?.sprites != null && i < pet.PetData.sprites.Length)
                    {
                        bool isReached = pet.PetData.CurrentEvoStage >= i;
                        EvoIcons[i].sprite = isReached ? pet.PetData.sprites[i].Icon : unknownIcon;
                        EvoIcons[i].color = Color.white;
                    }
                    else
                    {
                        EvoIcons[i].sprite = unknownIcon;
                        EvoIcons[i].color = Color.white;
                    }
                }
            }
        }
        else
        {
            if (PlayerInfo != null) PlayerInfo.SetActive(false);
            if (PetInfo != null) PetInfo.SetActive(true);

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

        ILevelable levelable = currentCharacter as ILevelable;

        if (levelable != null)
        {
            if (LevelTxt != null)
                LevelTxt.text = $"Lv. {levelable.Level}";
        }
        else
        {
            if (LevelTxt != null)
                LevelTxt.text = "Lv";
        }
    }
}