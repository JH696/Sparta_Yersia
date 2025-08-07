using Cinemachine;
using System.Collections;
using UnityEngine;

public class Portal : MonoBehaviour, IInteractable
{
    public Transform Destination;

    [Header("펫 스폰 포인트")]
    [SerializeField] private Transform[] petSpawnPoints;

    [Header("해당 포탈로 통하는 방의 카메라 Bounds")]
    [SerializeField] private PolygonCollider2D roomBounds;

    private bool isTeleporting = false;

    public void Interact(GameObject interactor)
    {
        if (interactor == null || isTeleporting) return;
        StartCoroutine(Teleport(interactor.transform));
    }

    private IEnumerator Teleport(Transform target)
    {
        isTeleporting = true;

        if (Destination == null)
        {
            Debug.LogError($"[{name}] Destination이 할당되지 않았습니다");
            isTeleporting = false;
            yield break;
        }

        var vcam = FindObjectOfType<CinemachineVirtualCamera>();
        if (vcam == null)
        {
            Debug.LogError("씬에 CinemachineVirtualCamera가 없습니다.");
            isTeleporting = false;
            yield break;
        }

        var confiner = vcam.GetComponent<CinemachineConfiner2D>();
        if (confiner == null)
        {
            Debug.LogError("VCam_Follow에 CinemachineConfiner2D 컴포넌트가 없습니다.");
            isTeleporting = false;
            yield break;
        }

        if (roomBounds == null)
        {
            Debug.LogError($"[{name}] roomBounds(PolygonCollider2D)가 할당되지 않았습니다");
            isTeleporting = false;
            yield break;
        }

        // ✅ [1] 페이드 인 (어두워짐)
        yield return FadeScreen.Instance.FadeIn();

        // ✅ [2] 위치 이동 및 설정
        Vector2 vec = Destination.position;
        vec.y -= 0.5f;
        target.position = vec;

        Vector3 oldPos = target.position;
        confiner.m_BoundingShape2D = roomBounds;
        confiner.InvalidateCache();

        Vector3 displacement = target.position - oldPos;
        vcam.OnTargetObjectWarped(target, displacement);

        // ✅ [3] 펫들 이동
        Player player = target.GetComponent<Player>();
        if (player != null && player.Party != null)
        {
            var partyList = player.Party.GetOrderedParty();
            for (int i = 0; i < partyList.Count; i++)
            {
                PetStatus status = partyList[i];
                if (status == null || status.PetInstance == null) continue;

                if (i < petSpawnPoints.Length && petSpawnPoints[i] != null)
                {
                    status.PetInstance.transform.position = petSpawnPoints[i].position;
                }
                else
                {
                    Vector3 fallbackOffset = new Vector3(1f + i * 0.5f, 0f, 0f);
                    status.PetInstance.transform.position = target.position + fallbackOffset;
                }
            }
        }

        // ✅ [4] 페이드 아웃 (밝아짐)
        yield return FadeScreen.Instance.FadeOut();

        isTeleporting = false;
    }
}