using System.Collections.Generic;
using UnityEngine;

public class BattleUI : MonoBehaviour
{
    [SerializeField] private ActionButtons actionButtons;

    public Canvas canvas;

    public void ShowActionButtons(CharacterSlot slot)
    {
        actionButtons.SetActioner(slot);
        actionButtons.SetPosition();
        actionButtons.gameObject.SetActive(true);
    }
}
