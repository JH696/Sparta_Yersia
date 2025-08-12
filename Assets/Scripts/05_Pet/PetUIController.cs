using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// 플레이어 보유 펫 리스트를 UI에 표시하고,
/// 기본 슬롯 4개는 항상 활성화 상태 유지,
/// 4마리 초과 시 동적 슬롯을 생성하여 추가 표시.
/// 슬롯 클릭 시 장착/해제 처리하고 UI 갱신.
/// </summary>
public class PetUIController : MonoBehaviour
{
    [Header("플레이어")]
    [SerializeField] private Player player;

    [Header("펫 UI")]
    [SerializeField] private GameObject petBG;

    [Header("펫 슬롯 프리팹 (동적 생성용)")]
    [SerializeField] private GameObject petSlotPrefab;

    [Header("슬롯 부모 트랜스폼")]
    [SerializeField] private Transform slotParent;

    [Header("기본 슬롯 4개 (인스펙터에서 할당)")]
    [SerializeField] private List<PetSlotUI> baseSlots = new List<PetSlotUI>(4);

    [Header("뒤로가기 버튼")]
    [SerializeField] private Button backBtn;

    // 기본 슬롯 이후에 생성된 슬롯들
    private List<PetSlotUI> additionalSlots = new List<PetSlotUI>();

    private void Awake()
    {
        if (backBtn != null)
            backBtn.onClick.AddListener(HidePetUI);
    }

    private void Start()
    {
        RefreshUI();
    }

    /// <summary>
    /// 현재 보유 중인 펫 리스트에 따라 UI 슬롯들을 갱신합니다.
    /// 기본 슬롯 4개는 항상 활성화하며, 초과하는 펫 수만큼 동적 슬롯을 생성/활성화합니다.
    /// 각 슬롯에 장착 여부를 반영하고 클릭 시 장착/해제 처리를 콜백으로 연결합니다.
    /// </summary>
    public void RefreshUI()
    {
        if (player == null || player.Party == null || player.Party.curPets == null)
            return;

        var curPets = player.Party.curPets;
        var equippedPets = player.Party.partyPets;

        int petCount = curPets.Count;

        // 기본 슬롯 4개 채우기
        for (int i = 0; i < baseSlots.Count; i++)
        {
            if (i < petCount)
            {
                var petStatus = curPets[i];
                bool isEquipped = equippedPets.Contains(petStatus);

                // PetStatus에서 Pet 인스턴스를 꺼내 전달
                baseSlots[i].gameObject.SetActive(true);
                baseSlots[i].SetData(petStatus, isEquipped);
                baseSlots[i].OnEquipClicked = OnEquipButtonClicked;
            }
            else
            {
                baseSlots[i].gameObject.SetActive(true);
                baseSlots[i].ClearData();
                baseSlots[i].OnEquipClicked = null;
            }
        }

        // 추가 슬롯 필요 여부 판단 및 생성/활성화
        int additionalPetStartIndex = baseSlots.Count;
        int additionalPetCount = Mathf.Max(0, petCount - baseSlots.Count);

        for (int i = 0; i < additionalPetCount; i++)
        {
            PetSlotUI slot;

            if (i < additionalSlots.Count)
            {
                slot = additionalSlots[i];
                slot.gameObject.SetActive(true);
            }
            else
            {
                var slotObj = Instantiate(petSlotPrefab, slotParent);
                slot = slotObj.GetComponent<PetSlotUI>();
                additionalSlots.Add(slot);
            }

            var petStatus = curPets[additionalPetStartIndex + i];
            bool isEquipped = equippedPets.Contains(petStatus);

            slot.SetData(petStatus, isEquipped);
            slot.OnEquipClicked = OnEquipButtonClicked;
        }

        // 남는 추가 슬롯 비활성화
        for (int i = additionalPetCount; i < additionalSlots.Count; i++)
        {
            additionalSlots[i].gameObject.SetActive(false);
        }
    }

    /// <summary>
    /// 슬롯 장착/해제 버튼 클릭 콜백
    /// </summary>
    /// <param name="pet">대상 펫 (UI에서는 Pet 기준으로 전달)</param>
    private void OnEquipButtonClicked(PetStatus pet)
    {
        var party = player.Party;
        if (party == null) return;

        if (party.partyPets.Contains(pet))
        {
            party.UnequipPet(pet);
        }
        else
        {
            party.EquipPet(pet);
        }

        RefreshUI();
    }

    /// <summary>
    /// 펫 UI 활성화
    /// </summary>
    public void ShowPetUI()
    {
        petBG.gameObject.SetActive(true);
        RefreshUI();
    }

    /// <summary>
    /// 펫 UI 비활성화
    /// </summary>
    public void HidePetUI()
    {
        petBG.SetActive(false);
    }
}