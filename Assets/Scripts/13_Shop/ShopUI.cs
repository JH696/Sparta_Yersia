using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ShopUI : MonoBehaviour
{
    [Header("플레이어 참조")]
    [SerializeField] private Player player;

    [Header("상점 아이템 원본 목록(퀘스트 제외)")]
    [SerializeField] private List<BaseItem> allShopItems = new List<BaseItem>();

    [Header("카테고리")]
    [SerializeField] private E_CategoryType category = E_CategoryType.All;
    [SerializeField] private Button allBtn;
    [SerializeField] private Button equipBtn;
    [SerializeField] private Button consumeBtn;

    [Header("리스트")]
    [SerializeField] private Transform slotParent;
    [SerializeField] private GameObject slotPrefab;
    private readonly List<ShopItemSlot> slots = new List<ShopItemSlot>();

    [Header("상세 패널")]
    [SerializeField] private GameObject infoPanel;
    [SerializeField] private TextMeshProUGUI nameText;
    [SerializeField] private TextMeshProUGUI typeText;
    [SerializeField] private TextMeshProUGUI statText;
    [SerializeField] private TextMeshProUGUI priceText;
    [SerializeField] private Button buyButton;

    [Header("안내")]
    [SerializeField] private TextMeshProUGUI notifyText;

    private BaseItem selected;

    private void Awake()
    {
        // 카테고리 버튼 연결
        if (allBtn) allBtn.onClick.AddListener(() => ChangeCategory(E_CategoryType.All));
        if (equipBtn) equipBtn.onClick.AddListener(() => ChangeCategory(E_CategoryType.Equip));
        if (consumeBtn) consumeBtn.onClick.AddListener(() => ChangeCategory(E_CategoryType.Consume));

        if (buyButton) buyButton.onClick.AddListener(OnClickBuy);
    }

    private void OnEnable()
    {
        RefreshList();
        ClearDetail();
        Notify("");
    }

    public void Show() => gameObject.SetActive(true);
    public void Hide() => gameObject.SetActive(false);

    public void SetItems(IEnumerable<BaseItem> items)
    {
        allShopItems = items?
            .Where(i => i != null && i.GetCategory() != E_CategoryType.Quest)
            .ToList() ?? new List<BaseItem>();
        RefreshList();
    }

    private void ChangeCategory(E_CategoryType cat)
    {
        category = cat;
        RefreshList();
        ClearDetail();
    }
    
    private void RefreshList()
    {
        // 슬롯 정리
        foreach (var s in slots) Destroy(s.gameObject);
        slots.Clear();

        // 카테고리 (퀘스트 제외)
        IEnumerable<BaseItem> src = allShopItems.Where(i => i != null && i.GetCategory() != E_CategoryType.Quest);

        switch (category)
        {
            case E_CategoryType.Equip: src = src.Where(i => i.GetCategory() == E_CategoryType.Equip); break;
            case E_CategoryType.Consume: src = src.Where(i => i.GetCategory() == E_CategoryType.Consume); break;
            default: /* All */ break;
        }

        foreach (var item in src)
        {
            var go = Instantiate(slotPrefab, slotParent);
            var slot = go.GetComponent<ShopItemSlot>();
            slot.Set(item);
            slot.OnClicked += OnClickSlot;
            slots.Add(slot);
        }
    }

    private void OnClickSlot(ShopItemSlot slot)
    {
        selected = slot.Data;
        ShowDetail(selected);
    }

    private void ShowDetail(BaseItem item)
    {
        if (item == null) { ClearDetail(); return; }

        infoPanel.SetActive(true);

        // 공통
        nameText.text = item.Name;
        typeText.text = "";
        statText.text = "";
        priceText.text = $"{GetPrice(item)} YP";

        // 장비: 분류/성능스탯
        if (item is EquipItemData e)
        {
            typeText.text = $"{e.Type}";
            foreach (var v in e.Values)
                statText.text += $"{v.Stat}: {v.Value}\n";
        }
        // 소모품: 성능스탯
        else if (item is ConsumeItemData c)
        {
            foreach (var v in c.Values)
                statText.text += $"{v.Stat}: {v.Value}\n";
        }
    }

    private void ClearDetail()
    {
        selected = null;
        infoPanel.SetActive(false);
        nameText.text = typeText.text = statText.text = priceText.text = "";
    }

    private int GetPrice(BaseItem item)
    {
        if (item is EquipItemData e) return e.Price;
        if (item is ConsumeItemData c) return c.Price;
        return 0;
    }

    private void OnClickBuy()
    {
        if (selected == null || player == null || player.Status == null)
        {
            Notify("구매할 수 없습니다.");
            return;
        }

        var status = player.Status;
        var inv = status.inventory;
        var wallet = status.Wallet;

        // 인벤토리에 1개 더 들어갈지(스택 여유 또는 빈 슬롯) 확인
        if (!inv.CanAddOne(selected))
        {
            Notify("인벤토리가 가득 찼습니다.");
            return;
        }

        // 가격 확인, YP 차감 시도
        int price = GetPrice(selected);
        if (price <= 0)
        {
            // 무료 아이템이면 바로 추가
            if (!inv.TryAddOne(selected))
            {
                Notify("인벤토리에 공간이 없습니다.");
                return;
            }
            Notify("구입 완료!");
            return;
        }

        if (!wallet.SpendYP(price))
        {
            Notify("YP가 부족합니다.");
            return;
        }

        // 실제 1개 추가
        if (!inv.TryAddOne(selected))
        {
            // 만약 실패했다면 돈 다시 돌려줌
            wallet.AddYP(price);
            Notify("인벤토리에 공간이 없습니다.");
            return;
        }

        Notify("구입 완료!");
    }

    private void Notify(string msg)
    {
        if (notifyText == null) return;
        notifyText.text = msg;
    }
}
