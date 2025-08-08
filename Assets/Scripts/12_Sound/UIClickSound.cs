using UnityEngine;
using UnityEngine.EventSystems;

/// <summary>
/// UI 요소 클릭 시 클릭 사운드를 재생합니다.
/// </summary>
public class UIClickSound : MonoBehaviour, IPointerClickHandler
{
    public void OnPointerClick(PointerEventData eventData)
    {
        SoundManager.Instance?.PlayClick();
    }
}