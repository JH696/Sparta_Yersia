using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class StatsUI : MonoBehaviour
{
    [Header("공통 스탯 텍스트")] // StatText
    [SerializeField] private TMP_Text HpTxt;
    [SerializeField] private TMP_Text MpTxt;
    [SerializeField] private TMP_Text AttackTxt;
    [SerializeField] private TMP_Text DefenseTxt;
    [SerializeField] private TMP_Text LuckTxt;
    [SerializeField] private TMP_Text SpeedTxt;

    [Header("공통 프로필 이미지")]
    [SerializeField] private Image ProfileImg;

    [Header("플레이어 전용 UI")]
    [SerializeField] private TMP_Text YPTxt;
    [SerializeField] private GameObject PlayerInfo;

    [Header("펫 전용 UI")]
    [SerializeField] private GameObject PetInfo;
    [SerializeField] private TMP_Text PetNameTxt;
    [SerializeField] private TMP_Text EvoStageTxt;
    [SerializeField] private Image[] EvoIcons = new Image[3];

    private BaseCharacter currentCharacter;

    // UI에 표시할 대상 캐릭터 설정
    public void SetTarget(BaseCharacter character)
    {
        if (character == null) return;
        currentCharacter = character;
        RefreshUI();
    }

    // 현재 캐릭터의 정보를 기반으로 UI 갱신
    public void RefreshUI()
    {
        if (currentCharacter == null) return;

        // 공통 스탯 표시
        HpTxt.text = $"{Mathf.RoundToInt(currentCharacter.CurrentHp)} / {Mathf.RoundToInt(currentCharacter.MaxHp)}";
        MpTxt.text = $"{Mathf.RoundToInt(currentCharacter.CurrentMana)} / {Mathf.RoundToInt(currentCharacter.MaxMana)}";
        AttackTxt.text = $"{Mathf.RoundToInt(currentCharacter.Attack)}";
        DefenseTxt.text = $"{Mathf.RoundToInt(currentCharacter.Defense)}";
        LuckTxt.text = $"{Mathf.RoundToInt(currentCharacter.Luck)}";
        SpeedTxt.text = $"{Mathf.RoundToInt(currentCharacter.Speed)}";

        // 플레이어 정보 UI 활성화
        if (currentCharacter is PlayerController player)
        {
            if (YPTxt != null) YPTxt.text = $"YP : {player.YP}";
            if (ProfileImg != null && player.PlayerData != null)
                ProfileImg.sprite = player.PlayerData.GetDefaultProfileIcon();

            if (PlayerInfo != null) PlayerInfo.SetActive(true);
            if (PetInfo != null) PetInfo.SetActive(false);
        }

        // 펫 정보 UI 활성화 (진화 단계 포함)
        else if (currentCharacter is PetController pet)
        {
            if (ProfileImg != null && pet.PetData != null)
                ProfileImg.sprite = pet.PetData.GetCurrentProfileIcon();

            if (PetInfo != null) PetInfo.SetActive(true);
            if (PlayerInfo != null) PlayerInfo.SetActive(false);

            if (PetNameTxt != null) PetNameTxt.text = pet.PetData?.PetName ?? "???";
            if (EvoStageTxt != null) EvoStageTxt.text = $"성장 단계 : {pet.PetData?.CurrentEvoStage + 1}";

            if (EvoIcons != null)
            {
                for (int i = 0; i < EvoIcons.Length; i++)
                {
                    if (EvoIcons[i] == null) continue;
                    Sprite icon = pet.PetData?.sprites != null && i < pet.PetData.sprites.Length
                        ? pet.PetData.sprites[i].Icon
                        : null;

                    EvoIcons[i].sprite = icon;
                    EvoIcons[i].color = (icon != null) ? Color.white : new Color(1, 1, 1, 0); // 아이콘이 없으면 투명 처리
                }
            }
        }

        // 대상이 없거나 처리 불가한 경우 (기본 숨김 처리)
        else
        {
            if (PlayerInfo != null) PlayerInfo.SetActive(false);
            if (PetInfo != null) PetInfo.SetActive(false);
        }
    }
}