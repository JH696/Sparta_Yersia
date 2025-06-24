using System.Collections.Generic;
using UnityEngine;

public class InventoryManager : MonoBehaviour
{
    public static InventoryManager Instance { get; private set; }

    private readonly Dictionary<string, int> itemDic = new Dictionary<string, int>();

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject); // 인벤토리 매니저는 게임 전체에서 유지
        }
        else
        {
            Destroy(gameObject); // 중복 생성 방지
        }
    }

    // 아이템 추가
    public void AddItem(ItemData data, int count = 1)
    {
        if (data == null) return;

        // TODO: 실제 아이템 추가 로직 구현
        Debug.Log($"아이템 추가: {data.ItemName}, 개수: {count}");
    }

    // 아이템 제거
    public bool RemoveItem(ItemData data, int count = 1)
    {
        if (data == null) return false;

        // TODO: 실제 아이템 제거 로직 구현

        return true;
    }

    // 특정 아이템 개수 조회

    // 전체 아이템 목록 반환

    // 
}
