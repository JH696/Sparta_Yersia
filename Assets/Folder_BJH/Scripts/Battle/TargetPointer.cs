using System.Collections.Generic;
using UnityEngine;

public class TargetPointer : MonoBehaviour
{
    [SerializeField] private List<Transform> points;
    [SerializeField] private List<GameObject> pointers;

    [SerializeField] private GameObject pointer;
    [SerializeField] private Canvas canvas;

    public void SetPointer()
    {
        foreach (Transform t in points)
        {
            RectTransform canvasRect = canvas.GetComponent<RectTransform>();

            // 월드 좌표 → 스크린 좌표 → 로컬 UI 좌표로 변환
            Vector2 screenPos = Camera.main.WorldToScreenPoint(t.position);
            Vector2 localPoint;
            RectTransformUtility.ScreenPointToLocalPointInRectangle(canvasRect, screenPos, Camera.main, out localPoint);

            localPoint.y += 30f;

            // 포인터 생성
            GameObject pointer = Instantiate(this.pointer, this.transform);
            pointer.GetComponent<RectTransform>().anchoredPosition = localPoint;

            pointers.Add(pointer);
        }
    }

    public void AddPoint(Transform target)
    {
        if (points.Contains(target)) return;

        points.Add(target);
        SetPointer();
    }
    public void RemovePoint(Transform target)
    {
        if (!points.Contains(target)) return;

        points.Remove(target);
    }

    private void RefreshPointer()
    {

    }
}