//using System.Collections.Generic;
//using UnityEngine;
//using UnityEngine.Profiling;

//public class B_EnemyUI : MonoBehaviour
//{
//    [Header("게이지")]
//    [SerializeField] private List<GameObject> gauges;

//    [Header("캐릭터")]
//    [SerializeField] private B_Characters chars;


//    public void SetEnemyHP()
//    {
//        List<B_CharacterSlot> slots = chars.Slots.FindAll(B_CharacterSlot => B_CharacterSlot.Type == ECharacterType.Enemy);
//        List<B_CharacterSlot> enemy = slots.FindAll(B_CharacterSlot => B_CharacterSlot.Character != null);

//        foreach (var a in ally)
//        {

//            B_ProfilePrefab partyPrefab = obj.GetComponent<B_ProfilePrefab>();

//            profiles.Add(partyPrefab);

//            partyPrefab.SetProfile(a.Character);
//        }
//    }


//    public void SetGauge(B_CharacterSlot slot)
//    {
//        slot.LinkActionGauge(this);

//        RectTransform canvasRect = canvas.GetComponent<RectTransform>();
//        Vector2 screenPos = Camera.main.WorldToScreenPoint(slot.gameObject.transform.position);

//        // 이 UI의 위치를 대상 오브젝트(슬롯) 트랜스폼 조금 위로 이동
//        Vector2 localPoint;
//        RectTransformUtility.ScreenPointToLocalPointInRectangle
//            (canvasRect, screenPos, Camera.main, out localPoint);

//        localPoint.y += 1.5f;

//        this.GetComponent<RectTransform>().localPosition = localPoint;
//    }

//    public void RefreshGauge(float amount)
//    {
//        img.fillAmount = amount / 100f;
//    }

//}
