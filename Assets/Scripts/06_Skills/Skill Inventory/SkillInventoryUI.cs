using System.Collections.Generic;
using UnityEngine;

public class SkillInventoryUI : MonoBehaviour
{
    [Header("현재 카테고리")]
    [SerializeField] private E_ElementalType type = E_ElementalType.None;

    [Header("선택한 슬롯")]
    [SerializeField] private I_SkillSlot seletedSlot = null;

    [Header("장착 스킬 아이콘")]
    [SerializeField] private EquipSkillSlot[] eSlots = new EquipSkillSlot[5];
    [SerializeField] private Sprite defaultImg;

    [Header("플레이어 스킬 인벤토리 (자동 참조)")]
    [SerializeField] private SkillInventory skillInventory;

    [Header("슬롯 프리팹 / 생성 위치")]
    [SerializeField] private GameObject slotPrefab;
    [SerializeField] private Transform slotParents;

    private List<I_SkillSlot> slots = new List<I_SkillSlot>();


    private void Start()
    {
        skillInventory = GameManager.player.skills;
        skillInventory.OnChanged += DisplaySlots;

        DisplaySlots();
    }

    public void ShowSkillUI()
    {
        this.gameObject.SetActive(true);

        RefreshEquipSlots();
    }

    public void ResetSkillUI()
    {
        type = E_ElementalType.None;
        seletedSlot = null;

        foreach (I_SkillSlot slot in slots)
        {
            slot.ResetColor();
        }

        this.gameObject.SetActive(false);
    }

    private void DisplaySlots()
    {
        if (slots.Count > 0)
        {
            foreach (I_SkillSlot slot in slots)
            {
                Destroy(slot.gameObject);
            }

            slots.Clear();
        }

        List<SkillStatus> skills = new List<SkillStatus>();

        switch (type)
        {
            case E_ElementalType.None:
                skills = skillInventory.AllSkills;
                break;
            case E_ElementalType.Fire:
                skills = skillInventory.AllSkills.FindAll(skill => skill.Data.Type == E_ElementalType.Fire);
                break;
            case E_ElementalType.Ice:
                skills = skillInventory.AllSkills.FindAll(skill => skill.Data.Type == E_ElementalType.Ice);
                break;
            case E_ElementalType.Nature:
                skills = skillInventory.AllSkills.FindAll(skill => skill.Data.Type == E_ElementalType.Nature);
                break;
            case E_ElementalType.Physical:
                skills = skillInventory.AllSkills.FindAll(skill => skill.Data.Type == E_ElementalType.Physical);
                break;
        }

        for (int i = 0; i < skills.Count; i++)
        {
            I_SkillSlot slot = Instantiate(slotPrefab, slotParents).GetComponent<I_SkillSlot>();
            slot.OnSlotClicked += SelectSkillSlot;
            slot.SetSlot(skills[i]);
            slots.Add(slot);
        }

        return;
    }

    public void ChangeType(int number)
    {
        this.type = (E_ElementalType)number;
        DisplaySlots();
    }

    private void SelectSkillSlot(I_SkillSlot slot)
    {
        if (seletedSlot != null)
        {
            seletedSlot.ResetColor();
        }

        seletedSlot = slot;
    }

    public void OnEquipButton()
    {
        if (seletedSlot == null) return;

        skillInventory.EquipSkill(seletedSlot.CurStatus);
        seletedSlot = null;
        RefreshEquipSlots();
    }

    public void OnUnequipButton()
    {
        if (seletedSlot == null) return;

        skillInventory.UnequipSkill(seletedSlot.CurStatus);
        seletedSlot = null;
        RefreshEquipSlots();
    }

    private void RefreshEquipSlots()
    {
        List<SkillStatus> equipSkills = skillInventory.EquipSkills;

        for (int i = 0; i < eSlots.Length; i++)
        {
            eSlots[i].ResetSlot();
        }

        for (int i = 0; i < equipSkills.Count; i++)
        {
            if (i > eSlots.Length - 1)
            {
                Debug.Log("최대 5개의 스킬을 장착할 수 있습니다.");
                return;
            }

            eSlots[i].SetSlot(equipSkills[i]);
        }
    }
}
