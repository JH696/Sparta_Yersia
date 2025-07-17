#if UNITY_EDITOR
using UnityEditor;
using UnityEngine;

public class TestItem : EditorWindow
{
    //public InventoryUI InventoryUI;
    //private PlayerInventory _inventory;
    //private ItemData[] _items;
    //private int _selectedIndex;

    //[MenuItem("Tools/Inventory Test Window")]
    //public static void ShowWindow()
    //{
    //    GetWindow<TestItem>("Inventory Test");
    //}

    //private void OnEnable()
    //{
    //    // Resources 폴더에서 SO 로드
    //    _items = Resources.LoadAll<ItemData>("ItemDatas");
    //}

    //private void OnGUI()
    //{
    //    GUILayout.Label("Inventory 테스트 도구", EditorStyles.boldLabel);

    //    if (!Application.isPlaying)
    //    {
    //        EditorGUILayout.HelpBox("Play 모드에서만 동작합니다.", MessageType.Info);
    //        return;
    //    }

    //    // PlayerInventory 인스턴스 찾기
    //    if (_inventory == null)
    //    {
    //        _inventory = FindObjectOfType<PlayerInventory>();
    //        if (_inventory == null)
    //        {
    //            EditorGUILayout.HelpBox("씬에 PlayerInventory가 없습니다.", MessageType.Error);
    //            return;
    //        }
    //    }

    //    if (_items == null || _items.Length == 0)
    //    {
    //        EditorGUILayout.HelpBox("Resources/ItemDatas에 ItemData SO가 없습니다.", MessageType.Warning);
    //        if (GUILayout.Button("SO 다시 로드"))
    //            //_items = Resources.LoadAll<ItemData>("ItemDatas");
    //        return;
    //    }

    //    EditorGUILayout.Space();
    //    if (GUILayout.Button("Add All Items"))
    //    {
    //        foreach (var item in _items)
    //        {
    //            _inventory.AddItem(item, 1);
    //            InventoryUI.RefreshUI();
    //            InventoryUI.Show();
    //        }
    //        Debug.Log($"[TestItem] {_items.Length}개 아이템을 인벤토리에 추가했습니다.");
    //    }
    //}
}
#endif