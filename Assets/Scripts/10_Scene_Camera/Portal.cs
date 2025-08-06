using Cinemachine;
using System.Collections;
using UnityEngine;

public class Portal : MonoBehaviour, IInteractable
{
    public Transform Destination;

    [Header("펫 스폰 포인트")]
    [SerializeField] private Transform[] petSpawnPoints;

    [Header("전환 연출 설정")]
    [SerializeField] private EPortalEffectType portalEffectType;

    [Header("해당 포탈로 통하는 방의 카메라 Bounds")]
    [SerializeField] private PolygonCollider2D roomBounds;

    private IPortalEffect portalEffect;

    private void Awake()
    {
        portalEffect = GetPortalEffect(portalEffectType);

        if (portalEffectType != EPortalEffectType.None && portalEffect == null)
        {
            Debug.LogWarning($"{gameObject.name} 포탈에 '{portalEffectType}' 효과 컴포넌트가 없습니다.");
        }
    }

    /// <summary>
    /// 상호작용자(GameObject)를 전달받아 텔레포트 수행
    /// </summary>
    public void Interact(GameObject interactor)
    {
        if (interactor == null) return;
        StartCoroutine(Teleport(interactor.transform));
    }

    /// <summary>
    /// 플레이어와 파티 펫을 함께 목적지로 이동
    /// </summary>
    private IEnumerator Teleport(Transform target)
    {
        if (portalEffect == null) yield break;
        if (Destination == null)
        {
            Debug.LogError($"[{name}] Destination이 할당되지 않았습니다");
            yield break;
        }
        var vcam = FindObjectOfType<CinemachineVirtualCamera>();
        if (vcam == null)
        {
            Debug.LogError("씬에 CinemachineVirtualCamera가 없습니다. VCam_Follow 를 배치하세요.");
            yield break;
        }
        var confiner = vcam.GetComponent<CinemachineConfiner2D>();
        if (confiner == null)
        {
            Debug.LogError("VCam_Follow에 CinemachineConfiner2D 컴포넌트가 붙어 있어야 합니다.");
            yield break;
        }
        if (roomBounds == null)
        {
            Debug.LogError($"[{name}] roomBounds(PolygonCollider2D)가 할당되지 않았습니다");
            yield break;
        }

        yield return portalEffect.PlayBeforeTeleport();

        // 플레이어 위치 이동
        Vector2 vec = Destination.position;
        vec.y -= 0.5f;
        target.position = vec;

        // Cinemachine Confiner 경계 교체
        Vector3 oldPos = target.position;
        confiner.m_BoundingShape2D = roomBounds;
        confiner.InvalidateCache();

        // 카메라 워프 보정
        Vector3 displacement = target.position - oldPos;
        vcam.OnTargetObjectWarped(target, displacement);

        // 펫들도 함께 이동
        Player player = target.GetComponent<Player>();
        if (player != null && player.Party != null)
        {
            var partyList = player.Party.GetOrderedParty();
            for (int i = 0; i < partyList.Count; i++)
            {
                PetStatus status = partyList[i];
                if (status == null || status.PetInstance == null) continue;

                // 수동 지정된 스폰 포인트가 있으면 그 위치로 이동
                if (i < petSpawnPoints.Length && petSpawnPoints[i] != null)
                {
                    status.PetInstance.transform.position = petSpawnPoints[i].position;
                }
                else
                {
                    // 부족할 경우 fallback 위치 적용
                    Vector3 fallbackOffset = new Vector3(1f + i * 0.5f, 0f, 0f);
                    status.PetInstance.transform.position = target.position + fallbackOffset;
                }
            }
        }

        yield return portalEffect.PlayAfterTeleport();
    }

    /// <summary>
    /// 포탈 효과 타입에 따른 컴포넌트 반환
    /// </summary>
    private IPortalEffect GetPortalEffect(EPortalEffectType type)
    {
        if (type == EPortalEffectType.Fade)
        {
            return GetComponent<FadePortalEffect>();
        }

        return null;
    }
}