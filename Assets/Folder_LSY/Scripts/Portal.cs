using UnityEngine;
using System.Collections;

public class Portal : MonoBehaviour, IInteractable
{
    [SerializeField] private Transform playerModel;

    // 이동 대상 위치
    [SerializeField] private Transform destination;

    [Header("전환 연출 설정")]
    [SerializeField] private EPortalEffectType portalEffectType;

    // 실제 실행될 전환 연출 컴포넌트
    private IPortalEffect portalEffect;

    private void Awake()
    {
        portalEffect = GetPortalEffect(portalEffectType);

        if (portalEffectType != EPortalEffectType.None && portalEffect == null)
        {
            Debug.LogWarning($"{gameObject.name} 포탈에 '{portalEffectType}' 효과 컴포넌트가 없습니다.");
        }
    }

    public void Interact()
    {
        StartCoroutine(Teleport());
    }

    // 전환 연출과 함께 플레이어 위치를 이동
    private IEnumerator Teleport()
    {
        // 연출 효과가 없으면 바로 종료
        if (portalEffect == null) yield break;

        // 이동 전 연출 실행
        yield return portalEffect.PlayBeforeTeleport();

        // 실제 위치 이동
        playerModel.position = destination.position;    

        // 이동 후 연출 실행
        yield return portalEffect.PlayAfterTeleport();
    }

    /// <summary>
    /// 설정된 이넘 타입에 따라 해당 연출 컴포넌트를 가져옴
    /// 추후 연출 타입이 늘어나면 switch 구문 확장
    /// </summary>
    private IPortalEffect GetPortalEffect(EPortalEffectType type)
    {
        if (type == EPortalEffectType.Fade)
        {
            var effect = GetComponent<FadePortalEffect>();
            if (effect == null) return null;
            return effect;
        }

        return null;
    }
}