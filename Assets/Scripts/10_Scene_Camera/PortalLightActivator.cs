// PortalLightActivator.cs
using System.Collections;
using UnityEngine;

[RequireComponent(typeof(Portal))]
public class PortalLightActivator : MonoBehaviour
{
    [Header("레거시: 도착 반경 감지로 라이트 켜기 (권장: 꺼둠)")]
    [SerializeField] private bool useLegacyArrivalProbe = false;

    [Header("포탈을 탔을 때 켤 추가 오브젝트")]
    [SerializeField] private GameObject extraObjectToActivate;

    [Header("도착 감지 반경/타임아웃(초)")]
    [SerializeField] private float arriveRadius = 0.7f;
    [SerializeField] private float timeout = 3f;

    private Portal portal;
    private bool aboutToUseThisPortal;
    private Coroutine waitRoutine;

    void Awake()
    {
        portal = GetComponent<Portal>();
    }

    void OnDisable()
    {
        if (waitRoutine != null) StopCoroutine(waitRoutine);
        aboutToUseThisPortal = false;
    }

    void OnTriggerStay2D(Collider2D col)
    {
        if (!useLegacyArrivalProbe) return;
        if (!col.CompareTag("Player")) return;

        if (Input.GetKeyDown(KeyCode.F))
        {
            aboutToUseThisPortal = true;

            if (waitRoutine != null) StopCoroutine(waitRoutine);
            waitRoutine = StartCoroutine(WaitForArrival(col.transform));
        }
    }

    private IEnumerator WaitForArrival(Transform player)
    {
        var dest = portal.Destination;
        if (dest == null) yield break;

        float t = 0f;
        while (aboutToUseThisPortal && t < timeout)
        {
            if (Vector2.Distance(player.position, dest.position) <= arriveRadius)
            {
                LightManager.Instance?.Activate(extraObjectToActivate);
                aboutToUseThisPortal = false;
                waitRoutine = null;
                yield break;
            }

            t += Time.deltaTime;
            yield return null;
        }

        aboutToUseThisPortal = false;
        waitRoutine = null;
    }
}
