using Cinemachine;
using UnityEngine;

public class RoomPortalConfiner : MonoBehaviour
{
    [Tooltip("해당 방의 외곽 콜라이더")]
    [SerializeField] private PolygonCollider2D targetBounds;

    private Portal portal;
    private CinemachineVirtualCamera vcam;
    private CinemachineConfiner confiner;

    void Awake()
    {
        portal = GetComponent<Portal>();
        if (portal == null)
            Debug.LogError($"{name} 에 Portal 컴포넌트가 필요");

        vcam = FindObjectOfType<CinemachineVirtualCamera>();
        confiner = vcam.GetComponent<CinemachineConfiner>();

        if (targetBounds == null)
            Debug.LogError($"{name} 의 targetBounds 할당 필요. Bounds 오브젝트에 컴포넌트: Static Rigidbody2D, PolygonCollider2D 확인");
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (!other.CompareTag("Player")) return;

        // 순간이동
        var playerT = other.transform;
        Vector3 oldPos = playerT.position;
        playerT.position = portal.Destination.position;

        // Confiner 경계 교체
        confiner.m_BoundingShape2D = targetBounds;
        confiner.InvalidatePathCache();

        // 플레이어가 텔레포트 했다는 신호주고 카메라 Warp 보정해주기
        Vector3 displacement = playerT.position - oldPos;
        vcam.OnTargetObjectWarped(playerT, displacement);
    }
}