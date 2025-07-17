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
    //[Header("플레이어")]
    //[SerializeField] private Player player;

    //[Header("펫 슬롯 프리팹 (동적 생성용)")]
    //[SerializeField] private GameObject petSlotPrefab;

    //[Header("슬롯 부모 트랜스폼")]
    //[SerializeField] private Transform slotParent;

    //[Header("기본 슬롯 4개 (인스펙터에서 할당)")]
    //[SerializeField] private List<PetSlotUI> baseSlots = new List<PetSlotUI>(4);

    //[Header("뒤로가기 버튼")]
    //[SerializeField] private Button backBtn;

    //// 기본 슬롯 이후에 생성된 슬롯들
    //private List<PetSlotUI> additionalSlots = new List<PetSlotUI>();

    //private void Awake()
    //{
    //    if (backBtn != null)
    //        backBtn.onClick.AddListener(HidePetUI);
    //}

    //private void Start()
    //{
    //    RefreshUI();
    //}

    ///// <summary>
    ///// UI가 다시 켜질 때 최신 상태로 갱신
    ///// </summary>
    //private void OnEnable()
    //{
    //    RefreshUI();
    //}

    ///// <summary>
    ///// 보유 펫 리스트 기반 UI 갱신.
    ///// 기본 슬롯 4개 먼저 채우고,
    ///// 펫이 많으면 추가 슬롯 생성 및 채움.
    ///// 남는 추가 슬롯은 비활성화.
    ///// 기본 슬롯은 항상 활성화, 펫 없으면 빈 상태 유지.
    ///// </summary>
    //public void RefreshUI()
    //{
    //    if (player == null || player.OwnedPets == null) return;

    //    int petCount = player.OwnedPets.Count;

    //    // 기본 슬롯 4개 채우기
    //    for (int i = 0; i < baseSlots.Count; i++)
    //    {
    //        if (i < petCount)
    //        {
    //            var pet = player.OwnedPets[i];
    //            bool isEquipped = player.EquippedPets.Contains(pet);
    //            baseSlots[i].gameObject.SetActive(true);
    //            baseSlots[i].SetData(pet, isEquipped);
    //            baseSlots[i].OnEquipClicked = OnEquipButtonClicked;
    //        }
    //        else
    //        {
    //            baseSlots[i].gameObject.SetActive(true);
    //            baseSlots[i].ClearData();
    //            baseSlots[i].OnEquipClicked = null;
    //        }
    //    }

    //    // 추가 슬롯 필요 여부 판단 및 생성/활성화
    //    int additionalPetStartIndex = baseSlots.Count;
    //    int additionalPetCount = Mathf.Max(0, petCount - baseSlots.Count);

    //    for (int i = 0; i < additionalPetCount; i++)
    //    {
    //        PetSlotUI slot;

    //        if (i < additionalSlots.Count)
    //        {
    //            slot = additionalSlots[i];
    //            slot.gameObject.SetActive(true);
    //        }
    //        else
    //        {
    //            var slotObj = Instantiate(petSlotPrefab, slotParent);
    //            slot = slotObj.GetComponent<PetSlotUI>();
    //            additionalSlots.Add(slot);
    //        }

    //        var pet = player.OwnedPets[additionalPetStartIndex + i];
    //        bool isEquipped = player.EquippedPets.Contains(pet);
    //        slot.SetData(pet, isEquipped);
    //        slot.OnEquipClicked = OnEquipButtonClicked;
    //    }

    //    // 남는 추가 슬롯 비활성화
    //    for (int i = additionalPetCount; i < additionalSlots.Count; i++)
    //    {
    //        additionalSlots[i].gameObject.SetActive(false);
    //    }
    //}

    ///// <summary>
    ///// 슬롯 장착/해제 버튼 클릭 콜백
    ///// </summary>
    ///// <param name="pet">대상 펫</param>
    //private void OnEquipButtonClicked(Pet pet)
    //{
    //    if (player.EquippedPets.Contains(pet))
    //    {
    //        player.UnequipPet(pet);
    //    }
    //    else
    //    {
    //        player.EquipPet(pet);
    //    }

    //    RefreshUI();
    //}

    //// 펫 UI 비활성화
    //public void HidePetUI()
    //{
    //    gameObject.SetActive(false);
    //}
}