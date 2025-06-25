using UnityEngine;

public class FollowerController : MonoBehaviour
{
    [Header("따라가기 대상")]
    [SerializeField] private Transform followTarget;

    [Header("따라가기 설정")]
    [SerializeField, Tooltip("대상과 유지할 최소 거리")]
    private float followDistance = 1f;

    [SerializeField, Tooltip("따라가는 속도")]
    private float followSpeed = 3f;

    private void Update()
    {
        if (followTarget == null) return;

        float distance = Vector3.Distance(transform.position, followTarget.position);
        
        // 거리가 followDistance 이상일 때만 따라감
        if (distance > followDistance)
        {
            Vector3 dir = (followTarget.position - transform.position).normalized;
            transform.position += dir * followSpeed * Time.deltaTime;
        }
    }

    public void SetFollowTarget(Transform target)
    {
        followTarget = target;
    }
}
