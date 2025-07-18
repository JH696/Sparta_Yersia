using System;

[System.Serializable]
public class ItemStatus
{
    public BaseItem Data { get; private set; }
    public int Stack { get; private set; }
    public bool IsFull => Stack == Data.MaxStack;

    public event Action StatusChanged; // 아이템 상태 변경 이벤트
    public event Action OnEmpty; // 아이템이 비었을 때 호출
    public ItemStatus(BaseItem data)
    {
        Data = data;
        Stack = 1;
    }

    public void UseItem()
    {
        Stack--;

        if (Stack == 0)
        {
            OnEmpty?.Invoke();
        }

        StatusChanged?.Invoke();
    }
    public void StackItem()
    {
        if (IsFull) return;

        Stack++;
    }

    /// <summary>
    /// BaseItem 상속 클래스(EquipItemData, ConsumeItemData...) 반환
    /// 사용법: GetChild<EquipItemData>(), GetChild<ConsumeItemData>() 등
    /// </summary>
    public T GetDataAs<T>() where T : BaseItem
    {
        if (Data is T result)
        {
            return result;
        }
        return null;
    }
}

