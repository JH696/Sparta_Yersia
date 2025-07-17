//using UnityEngine;
//using UnityEngine.UI;

//public class B_ActionGauge : MonoBehaviour
//{
//    [Header("게이지 이미지")]
//    [SerializeField] private Image img;

//    public void SetGauge(B_CharacterSlot character, B_MonsterSlot monster)
//    {
//        this.gameObject.SetActive(true);

//        Canvas canvas = GetComponentInParent<Canvas>();
//        RectTransform canvasRect = canvas.GetComponent<RectTransform>();

//        Vector2 screenPos;

//        if (character != null)
//        {
//            screenPos = Camera.main.WorldToScreenPoint(character.gameObject.transform.position);
//        }
//        else if (monster != null) 
//        {
//            screenPos = Camera.main.WorldToScreenPoint(monster.gameObject.transform.position);
//        }
//        else
//        {
//            Debug.LogWarning("SetGauge: character와 monster가 모두 null입니다.");
//            return;
//        }

//        // 이 UI의 위치를 대상 오브젝트 트랜스폼 조금 위로 이동
//        Vector2 localPoint;
//        RectTransformUtility.ScreenPointToLocalPointInRectangle
//            (canvasRect, screenPos, Camera.main, out localPoint);

//        localPoint.y += 1.5f;
        
//        this.GetComponent<RectTransform>().localPosition = localPoint;
//    }

//    public void ResetGauge()
//    {
//        this.gameObject.SetActive(false);
//        RefreshGauge(0);
//    }

//    public void RefreshGauge(float amount)
//    {
//        img.fillAmount = amount / 100f;
//    }
//}
