using System;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class PlayerParty
{
    [Header("파티 제한")]
    [SerializeField] private int maxPartyPets = 2;

    public List<PetStatus> curPets; // 보유 중인 전체 펫

    public List<PetStatus> partyPets; // 장착한 펫

    public PlayerParty()
    {
        curPets = new List<PetStatus>();
        partyPets = new List<PetStatus>();
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
            Debug.Log($"펫 {status.PetData.name} 보유 리스트에 추가됨");
        }
        else
        {
            Debug.Log($"이미 보유 중인 펫입니다: {status.PetData.name}");
        }
    }

    /// <summary>
    /// 보유 펫 중에서 장착 (파티에 추가)
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
            Debug.LogWarning($"보유하지 않은 펫은 장착할 수 없습니다: {status.PetData.name}");
            return;
        }

        if (partyPets.Contains(status))
        {
            Debug.Log($"이미 장착된 펫입니다: {status.PetData.name}");
            return;
        }

        if (partyPets.Count >= maxPartyPets)
        {
            Debug.LogWarning($"파티에는 최대 {maxPartyPets}마리의 펫만 장착할 수 있습니다.");
            return;
        }

        partyPets.Add(status);
        Debug.Log($"펫 {status.PetData.name} 장착됨 (파티에 추가됨)");

        RefreshPartyState();
    }

    /// <summary>
    /// 장착 해제 (파티에서 제거, 보유 리스트에는 남음)
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
            Debug.LogWarning($"장착되어 있지 않은 펫입니다: {status.PetData.name}");
            return;
        }

        partyPets.Remove(status);
        Debug.Log($"펫 {status.PetData.name} 장착 해제됨 (파티에서 제거됨)");

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

    private void RefreshFollowChain()
    {
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

    private void RefreshUI()
    {
        // TODO: UI 갱신 처리
        // 예) UIManager.Instance.RefreshPartyUI(partyPets);
        Debug.Log("파티 UI 갱신 처리");
    }
}