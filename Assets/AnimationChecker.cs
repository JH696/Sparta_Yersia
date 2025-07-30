//using UnityEngine;
//using UnityEditor;
//using System.IO;
//public class AnimationEventChecker : EditorWindow
//{
//    [MenuItem("Tools/검사/AnimationEvent 함수 누락 검사")]
//    public static void CheckAnimationEvents()
//    {
//        string[] guids = AssetDatabase.FindAssets("t:AnimationClip");
//        int totalEvents = 0;
//        int emptyEvents = 0;
//        foreach (string guid in guids)
//        {
//            string path = AssetDatabase.GUIDToAssetPath(guid);
//            AnimationClip clip = AssetDatabase.LoadAssetAtPath<AnimationClip>(path);
//            if (clip == null) continue;
//            AnimationEvent[] events = AnimationUtility.GetAnimationEvents(clip);
//            for (int i = 0; i < events.Length; i++)
//            {
//                totalEvents++;
//                if (string.IsNullOrEmpty(events[i].functionName))
//                {
//                    Debug.LogWarning($":느낌표: [누락된 함수] {clip.name} ({path}) - 이벤트 #{i + 1}에 Function 이름 없음", clip);
//                    emptyEvents++;
//                }
//            }
//        }
//        if (emptyEvents == 0)
//        {
//            Debug.Log($":흰색_확인_표시: 모든 애니메이션 이벤트에 함수가 지정되어 있습니다. 검사된 이벤트 수: {totalEvents}");
//        }
//        else
//        {
//            Debug.LogWarning($":경고: 비어 있는 AnimationEvent 함수 {emptyEvents}개 발견됨 (총 검사: {totalEvents})");
//        }
//    }
//}