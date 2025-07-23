using UnityEngine;
using System.Collections;

public class Portal : MonoBehaviour, IInteractable
{
    [SerializeField] private Transform destination;

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

    private IEnumerator Teleport(Transform target)
    {
        if (portalEffect == null) yield break;

        yield return portalEffect.PlayBeforeTeleport();

        target.position = destination.position;

        yield return portalEffect.PlayAfterTeleport();
    }

    private IPortalEffect GetPortalEffect(EPortalEffectType type)
    {
        if (type == EPortalEffectType.Fade)
        {
            return GetComponent<FadePortalEffect>();
        }

        return null;
    }
}