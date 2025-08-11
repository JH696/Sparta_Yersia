using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ShopItemSlot : MonoBehaviour
{
    [Header("아이콘/이름")]
    [SerializeField] private Image icon;
    [SerializeField] private TextMeshProUGUI nameText;

    private BaseItem data;
    public System.Action<ShopItemSlot> OnClicked;

    public BaseItem Data => data;

    public void Set(BaseItem item)
    {
        data = item;
        if (icon != null) icon.sprite = item.Icon;
        if (nameText != null) nameText.text = item.Name;
        gameObject.SetActive(true);
    }

    public void Clear()
    {
        data = null;
        if (icon != null) icon.sprite = null;
        if (nameText != null) nameText.text = string.Empty;
        gameObject.SetActive(false);
    }

    public void OnClick()
    {
        if (data == null) return;
        OnClicked?.Invoke(this);
    }
}
