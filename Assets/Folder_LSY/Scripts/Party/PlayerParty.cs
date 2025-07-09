using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 플레이어 게임오브젝트와 함께
/// 펫과 NPC 파티원을 관리하는 스크립트
/// 배틀씬 진입 시 파티 데이터를 넘길 때
/// 플레이어도 포함하도록 설계됨
/// </summary>
public class PlayerParty : MonoBehaviour
{
    [Header("파티 인원 제한")]
    [SerializeField] private int maxPets = 2;
    [SerializeField] private int maxNpcs = 1;

    [Header("플레이어 게임오브젝트")]
    [SerializeField] private GameObject player;

    [Header("파티 멤버 리스트 (펫, NPC 혼합)")]
    [SerializeField] private List<GameObject> partyMembers = new List<GameObject>();

    private void Awake()
    {
        if (player == null)
        {
            Debug.LogError("Player GameObject가 할당되어 있지 않습니다.");
        }
    }

    // 펫 추가 (최대 2마리)
    public void AddPet(GameObject pet)
    {
        if (CountPets() >= maxPets || partyMembers.Contains(pet)) return;

        partyMembers.Add(pet);
        Debug.Log($"펫 {pet.name} 파티에 추가됨");
    }

    // NPC 추가 (최대 1명)
    public void AddNpc(GameObject npc)
    {
        if (CountNpcs() >= maxNpcs || partyMembers.Contains(npc)) return;

        partyMembers.Add(npc);
        Debug.Log($"NPC {npc.name} 파티에 추가됨");
    }

    // 파티 멤버 제거
    public void RemoveMember(GameObject member)
    {
        if (partyMembers.Remove(member))
            Debug.Log($"{member.name} 파티에서 제거됨");
    }

    // 현재 파티 내 펫 수 계산
    private int CountPets()
    {
        int count = 0;
        foreach (var member in partyMembers)
        {
            if (member.GetComponent<PetController>() != null)
                count++;
        }
        return count;
    }

    // 현재 파티 내 NPC 수 계산
    private int CountNpcs()
    {
        int count = 0;
        foreach (var member in partyMembers)
        {
            if (member.GetComponent<NPC>() != null)
                count++;
        }
        return count;
    }

    // 전체 파티 멤버 반환 (플레이어 포함)
    public List<GameObject> GetFullPartyMembers()
    {
        var fullParty = new List<GameObject>();
        if (player != null) fullParty.Add(player);
        fullParty.AddRange(partyMembers);
        return fullParty;
    }
}