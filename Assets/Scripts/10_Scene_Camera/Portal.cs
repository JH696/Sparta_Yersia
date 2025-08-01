using UnityEngine;
using System.Collections;

public class Portal : MonoBehaviour, IInteractable
{
    public Transform Destination;

    [Header("펫 스폰 포인트")]
    [SerializeField] private Transform[] petSpawnPoints;

    [Header("전환 연출 설정")]
    [SerializeField] private EPortalEffectType portalEffectType;

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

        yield return portalEffect.PlayBeforeTeleport();

        // 플레이어 위치 이동
        Vector2 vec = Destination.position;
        vec.y -= 0.5f;
        target.position = vec;

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