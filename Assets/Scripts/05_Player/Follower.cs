using UnityEngine;

public class Follower : MonoBehaviour
{
    [Header("추적 대상")]
    public Transform target;

    [Header("추적 설정")]
    public float followSpeed = 5f;
    public float followDistance = 1.5f; // 최소 간격

    private void Update()
    {
        // 추적 기능
        if (target == null) return;

        Vector3 dir = target.position - transform.position;
        float distance = dir.magnitude;

        if (distance > followDistance)
        {
            // 방향 유지한 채 부드럽게 이동
            transform.position = Vector3.MoveTowards(
                transform.position,
                target.position,
                followSpeed * Time.deltaTime
            );
        }
    }

    /// <summary>
    /// 외부에서 타겟을 동적으로 설정
    /// </summary>
    public void SetTarget(Transform newTarget)
    {
        target = newTarget;
    }
}