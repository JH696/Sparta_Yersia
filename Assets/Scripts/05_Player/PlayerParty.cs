using System;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class PlayerParty
{
    [Header("파티 제한")]
    [SerializeField] private int maxPartyPets = 2;

    [Header("펫 생성 관련")]
    [SerializeField] private Transform petParent;

    public List<PetStatus> curPets;
    public List<PetStatus> partyPets;

    public PlayerParty()
    {
        curPets = new List<PetStatus>();
        partyPets = new List<PetStatus>();
    }

    public void Initialize(Transform petParent)
    {
        this.petParent = petParent;
    }

    /// <summary>
    /// 보유 중인 펫 추가 (중복 추가 방지)
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
    /// 보유 펫 중에서 장착 (파티에 추가 및 인스턴스 생성)
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
            if (petParent == null)
            {
                Debug.LogError("petParent가 할당되어 있지 않습니다.");
                return;
            }

            GameObject petObj = UnityEngine.Object.Instantiate(status.PetData.PetPrefab, petParent);

            // 플레이어로부터 followDistance 만큼 떨어진 위치에서 생성
            float followDistance = 5f;
            float angle = UnityEngine.Random.Range(0f, 360f);
            Vector3 offset = new Vector3(Mathf.Cos(angle), 0, Mathf.Sin(angle)) * followDistance;
            petObj.transform.position = petParent.position + offset;

            petObj.name = $"Pet_{status.PetData.PetName}";

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
    /// 장착 해제 (파티에서 제거, 보유 리스트에는 남음, 인스턴스 파괴)
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
    /// 파티 펫을 순서대로 반환 (필요시 추후 Follow 시스템에 사용)
    /// </summary>
    public List<PetStatus> GetOrderedParty()
    {
        return new List<PetStatus>(partyPets);
    }

    /// <summary>
    /// 파티 상태 변경 후 호출: 팔로우 체인 갱신, UI 갱신 등 후처리 수행
    /// </summary>
    public void RefreshPartyState()
    {
        RefreshFollowChain();
        RefreshUI();
    }

    /// <summary>
    /// 각 펫의 Follower.target을 설정하여 체인 구조 구성
    /// </summary>
    private void RefreshFollowChain()
    {
        if (partyPets.Count == 0) return;

        // 첫 번째 펫은 플레이어(petParent)를 따라감
        if (partyPets[0].PetInstance != null && petParent != null)
        {
            var follower = partyPets[0].PetInstance.GetComponent<Follower>();
            if (follower != null)
                follower.target = petParent;
        }

        // 두 번째 펫부터는 바로 앞 펫을 따라감
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
    /// 펫 UI 갱신 트리거
    /// </summary>
    private void RefreshUI()
    {
        // TODO: UI 갱신 처리
        // 예: UIManager.Instance.RefreshPartyUI(partyPets);
        Debug.Log("파티 UI 갱신 처리");
    }
}