using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EquipmentUI : MonoBehaviour
{
    [Header("장비 관리")]
    [SerializeField] private Player player; 

    [Header("장비 슬롯 아이콘")]
    [SerializeField] private List<Image> icon; // 장비 아이콘

    private void Start()
    {
        player.Status.equipment.EquipmentChanged += RefreshIcon;

        RefreshIcon();
    }

    private void OnDestroy()
    {
        player.Status.equipment.EquipmentChanged -= RefreshIcon;
    }

    // 장비 아이콘 동기화 메서드
    private void RefreshIcon()
    {
        Debug.Log("아이콘 업데이트");

        for (int i = 0; i < icon.Count; i++)
        {
            Sprite icon = player.Status.equipment.GetItemIcon((E_EquipType)i);

            if (icon != null)
            {
                this.icon[i].enabled = true;
                this.icon[i].sprite = icon;
            }
            else
            {
                this.icon[i].enabled = false;
                this.icon[i].sprite = null;
            }
        }
    }
}
