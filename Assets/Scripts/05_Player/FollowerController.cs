using UnityEngine;

public class Follower : MonoBehaviour
{
    [Header("추적 대상")]
    public Transform target;

    [Header("추적 설정")]
    public float followSpeed = 5f;
    public float followDistance = 1.5f; // 최소 간격

    private void Start()
    {
        // 초기 타겟은 부모 (추후 재설정 가능)
        if (target == null)
        {
            target = GetComponentInParent<Transform>();
        }
    }

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

//public class FollowerController : MonoBehaviour
//{
//    [Header("따라가기 대상")]
//    [SerializeField] private Transform followTarget;

//    [Header("따라가기 설정")]
//    [SerializeField, Tooltip("대상과 유지할 최소 거리")]
//    private float followDistance = 2f;

//    [SerializeField, Tooltip("따라가는 속도")]
//    private float followSpeed = 3f;

//    private void Awake()
//    {
//        // 씬 전환이 일어날 때마다 팔로우 활성화 여부를 갱신하기 위해 씬 변경 이벤트 구독
//        UnityEngine.SceneManagement.SceneManager.activeSceneChanged += OnActiveSceneChanged;

//        // 초기 활성화 상태 설정
//        UpdateFollowAllowed();
//    }

//    private void OnDestroy()
//    {
//        // 구독 해제
//        UnityEngine.SceneManagement.SceneManager.activeSceneChanged -= OnActiveSceneChanged;
//    }

//    // 씬이 변경될 때 호출되는 콜백
//    private void OnActiveSceneChanged(UnityEngine.SceneManagement.Scene oldScene, UnityEngine.SceneManagement.Scene newScene)
//    {
//        UpdateFollowAllowed();
//    }

//    // 씬 정보에 따라 따라오기 활성화 여부 갱신
//    // 배틀씬일 경우 비활성화, 메인씬일 경우 활성화
//    private void UpdateFollowAllowed()
//    {
//        if (SceneLoader.CurrentScene == EScene.Scene_LSY_Battle)
//        {
//            // 배틀씬에서는 따라오기 비활성화
//            IsFollowingAllowed = false;
//        }
//        else
//        {
//            // 그 외 씬에서는 활성화
//            IsFollowingAllowed = true;
//        }
//    }

//    private bool IsFollowingAllowed = true;

//    private void Update()
//    {
//        if (!IsFollowingAllowed || followTarget == null) return;

//        // 타겟과의 방향 벡터 (내 위치에서 타겟 위치를 뺀 방향)
//        Vector3 direction = (transform.position - followTarget.position).normalized;

//        // 타겟 위치에서 일정 거리 떨어진 목표 위치 계산
//        Vector3 desiredPos = followTarget.position + direction * followDistance;

//        // 목표 위치로 부드럽게 이동 (속도 제한)
//        transform.position = Vector3.MoveTowards(transform.position, desiredPos, followSpeed * Time.deltaTime);
//    }

//    // 따라가기 대상 설정
//    public void SetFollowTarget(Transform target)
//    {
//        followTarget = target;
//    }
//}