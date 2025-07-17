using UnityEngine;
using UnityEngine.UI;
using TMPro;

/// <summary>
/// 개별 펫 슬롯 UI 클래스
/// UI 보이는 부분만 담당
/// </summary>
//public class PetSlotUI : MonoBehaviour
//{
//    [SerializeField] private Image petImage;
//    [SerializeField] private TMP_Text infoTxt;
//    [SerializeField] private Button equipBtn;
//    [SerializeField] private TMP_Text equipBtnTxt;

//    private Pet pet;
//    private bool isEquipped = false;

//    public System.Action<Pet> OnEquipClicked;

//    /// <summary>
//    /// 슬롯에 펫 데이터 할당 및 UI 갱신
//    /// </summary>
//    public void SetData(Pet pet, bool isEquipped)
//    {
//        this.pet = pet;
//        this.isEquipped = isEquipped;

//        petImage.sprite = pet.PetData.GetCurrentProfileIcon();
//        infoTxt.text = $"Lv.{pet.Level} / {pet.PetData.PetName}";
//        equipBtnTxt.text = isEquipped ? "해제" : "장착";

//        equipBtn.onClick.RemoveAllListeners();
//        equipBtn.onClick.AddListener(() => OnEquipClicked?.Invoke(pet));
//    }

//    /// <summary>
//    /// 빈 슬롯 UI 상태로 초기화
//    /// </summary>
//    public void ClearData()
//    {
//        pet = null;
//        isEquipped = false;

//        petImage.sprite = null;
//        infoTxt.text = "";
//        equipBtnTxt.text = "";

//        equipBtn.onClick.RemoveAllListeners();
//        OnEquipClicked = null;
//    }

//    /// <summary>
//    /// 장착 상태 변경 UI 갱신
//    /// </summary>
//    public void RefreshEquipState(bool isEquipped)
//    {
//        this.isEquipped = isEquipped;
//        equipBtnTxt.text = isEquipped ? "해제" : "장착";
//    }
//}