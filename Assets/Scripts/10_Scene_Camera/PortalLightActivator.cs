using System.Collections;
using UnityEngine;

[RequireComponent(typeof(Portal))]
public class PortalLightActivator : MonoBehaviour
{
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
        // 포탈이 비활성화되면 안전하게 플래그/코루틴 정리
        if (waitRoutine != null) StopCoroutine(waitRoutine);
        aboutToUseThisPortal = false;
    }

    // 플레이어가 포탈 콜라이더에 서서 F키로 상호작용할 때 플래그만 잡음
    void OnTriggerStay2D(Collider2D col)
    {
        if (!col.CompareTag("Player")) return;

        if (Input.GetKeyDown(KeyCode.F))
        {
            aboutToUseThisPortal = true;

            // 도착 대기 코루틴 시작
            if (waitRoutine != null) StopCoroutine(waitRoutine);
            waitRoutine = StartCoroutine(WaitForArrival(col.transform));
        }
    }

    private IEnumerator WaitForArrival(Transform player)
    {
        // 포탈에 Destination이 할당되어 있다고 가정(Portal.cs가 검증함)
        var dest = portal.Destination;
        if (dest == null)
        {
            // 목적지가 없으면 실패 → 라이트 끔
            PlayerLightController.Instance?.SetByPortal(false);
            yield break;
        }

        float t = 0f;
        while (aboutToUseThisPortal && t < timeout)
        {
            // 플레이어가 목적지 근처에 도착하면 성공 판정
            if (Vector2.Distance(player.position, dest.position) <= arriveRadius)
            {
                PlayerLightController.Instance?.SetByPortal(true, extraObjectToActivate);
                aboutToUseThisPortal = false;
                waitRoutine = null;
                yield break;
            }

            t += Time.deltaTime;
            yield return null;
        }

        // 타임아웃 혹은 플래그 해제 -> 실패/취소로 간주, 라이트 끔
        PlayerLightController.Instance?.SetByPortal(false);
        aboutToUseThisPortal = false;
        waitRoutine = null;
    }
}
