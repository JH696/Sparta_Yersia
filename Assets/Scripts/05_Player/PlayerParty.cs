using System;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 플레이어의 펫 보유 및 파티 관리
/// </summary>
[Serializable]
public class PlayerParty
{
    [Header("파티 제한")]
    [SerializeField] private int maxPartyPets = 2; // 최대 파티 펫 수

    // 보유한 펫 목록
    public List<PetStatus> curPets;

    // 실제 파티에 장착된 펫 목록
    public List<PetStatus> partyPets;

    private Transform playerTransform; // 플레이어 위치 참조

    public PlayerParty()
    {
        curPets = new List<PetStatus>();
        partyPets = new List<PetStatus>();
    }

    /// <summary>
    /// 초기화 (플레이어 위치 정보 할당)
    /// </summary>
    public void Initialize(Transform playerTransform)
    {
        this.playerTransform = playerTransform;
    }

    /// <summary>
    /// 펫을 보유 목록에 추가
    /// </summary>
    public void AddPet(PetStatus status)
    {
        if (status == null)
        {
            Debug.LogWarning("추가할 펫이 null입니다.");
            return;
        }

        if (!curPets.Contains(status))
        {
            curPets.Add(status);
            Debug.Log($"펫 {status.PetData.PetName} 보유 리스트에 추가됨");
        }
        else
        {
            Debug.Log($"이미 보유 중인 펫입니다: {status.PetData.PetName}");
        }
    }

    /// <summary>
    /// 펫을 파티에 장착
    /// </summary>
    public void EquipPet(PetStatus status)
    {
        if (status == null)
        {
            Debug.LogWarning("장착할 펫이 null입니다.");
            return;
        }

        if (!curPets.Contains(status))
        {
            Debug.LogWarning($"보유하지 않은 펫은 장착할 수 없습니다: {status.PetData.PetName}");
            return;
        }

        if (partyPets.Contains(status))
        {
            Debug.Log($"이미 장착된 펫입니다: {status.PetData.PetName}");
            return;
        }

        if (partyPets.Count >= maxPartyPets)
        {
            Debug.LogWarning($"파티에는 최대 {maxPartyPets}마리의 펫만 장착할 수 있습니다.");
            return;
        }

        if (status.PetInstance == null)
        {
            if (status.PetData == null || status.PetData.PetPrefab == null)
            {
                Debug.LogError($"펫 프리팹이 설정되지 않았습니다: {status.PetData?.PetName ?? "Unknown"}");
                return;
            }
            if (playerTransform == null)
            {
                Debug.LogError("playerTransform이 할당되어 있지 않습니다.");
                return;
            }

            // 펫 프리팹 인스턴스화
            GameObject petObj = UnityEngine.Object.Instantiate(status.PetData.PetPrefab);

            // 플레이어 주변에 생성
            float followDistance = 5f;
            float angle = UnityEngine.Random.Range(0f, 360f);
            Vector3 offset = new Vector3(Mathf.Cos(angle), 0, Mathf.Sin(angle)) * followDistance;
            petObj.transform.position = playerTransform.position + offset;

            petObj.name = $"Pet_{status.PetData.PetName}";

            // Pet 컴포넌트 설정
            Pet pet = petObj.GetComponent<Pet>();
            if (pet != null)
            {
                pet.SetStatus(status);
            }
            else
            {
                Debug.LogWarning("펫 프리팹에 Pet 컴포넌트가 없습니다.");
            }

            status.PetInstance = pet;
        }

        partyPets.Add(status);
        Debug.Log($"펫 {status.PetData.PetName} 장착됨 (파티에 추가됨)");

        RefreshPartyState();
    }

    /// <summary>
    /// 펫을 파티에서 해제
    /// </summary>
    public void UnequipPet(PetStatus status)
    {
        if (status == null)
        {
            Debug.LogWarning("해제할 펫이 null입니다.");
            return;
        }

        if (!partyPets.Contains(status))
        {
            Debug.LogWarning($"장착되어 있지 않은 펫입니다: {status.PetData.PetName}");
            return;
        }

        partyPets.Remove(status);
        Debug.Log($"펫 {status.PetData.PetName} 장착 해제됨 (파티에서 제거됨)");

        if (status.PetInstance != null)
        {
            UnityEngine.Object.Destroy(status.PetInstance.gameObject);
            status.PetInstance = null;
        }

        RefreshPartyState();
    }

    /// <summary>
    /// 현재 파티 펫 목록 반환
    /// </summary>
    public List<PetStatus> GetOrderedParty()
    {
        return new List<PetStatus>(partyPets);
    }

    /// <summary>
    /// 팔로우 체인 및 UI 갱신
    /// </summary>
    public void RefreshPartyState()
    {
        RefreshFollowChain();
        RefreshUI();
    }

    /// <summary>
    /// 펫 간의 팔로우 연결 갱신
    /// </summary>
    private void RefreshFollowChain()
    {
        if (partyPets.Count == 0) return;

        // 첫 번째 펫은 플레이어를 따라감
        if (partyPets[0].PetInstance != null && playerTransform != null)
        {
            var follower = partyPets[0].PetInstance.GetComponent<Follower>();
            if (follower != null)
                follower.target = playerTransform;
        }

        // 나머지는 앞 펫을 따라감
        for (int i = 1; i < partyPets.Count; i++)
        {
            var current = partyPets[i];
            var previous = partyPets[i - 1];

            if (current.PetInstance == null || previous.PetInstance == null) continue;

            var follower = current.PetInstance.GetComponent<Follower>();
            if (follower != null)
            {
                follower.target = previous.PetInstance.transform;
            }
        }
    }

    /// <summary>
    /// 펫 UI 갱신
    /// </summary>
    private void RefreshUI()
    {
        // TODO: UI 갱신 처리
        Debug.Log("파티 UI 갱신 처리");
    }
}