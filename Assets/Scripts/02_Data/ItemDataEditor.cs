#if UNITY_EDITOR
using UnityEditor;

[CustomEditor(typeof(BaseItem))]
public class ItemDataEditor : Editor
{
    private SerializedProperty categoryProp;
    private SerializedProperty equipTypeProp;

    // 인스펙터가 활성화될 때 호출
    private void OnEnable()
    {
        categoryProp = serializedObject.FindProperty("category");
        equipTypeProp = serializedObject.FindProperty("equipType");
    }

    // ItemData의 인스펙터 커스텀
    public override void OnInspectorGUI()
    {
        serializedObject.Update();

        EditorGUILayout.PropertyField(categoryProp);

        if ((E_CategoryType)categoryProp.enumValueIndex == E_CategoryType.Equip)
        {
            EditorGUILayout.PropertyField(equipTypeProp);
        }

        DrawPropertiesExcluding(serializedObject, "m_Script", "category", "equipType");
        serializedObject.ApplyModifiedProperties();
    }
}
#endif