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

    private void Start()
    { 
        if (player == null)
        {
            player = GameManager.Instance.Player;
        }

        // 실행 시 초기 상태에 맞게 팔로우 체인 갱신
        UpdateFollowChain();
    }

    // 펫 추가 (최대 2마리)
    public void AddPet(GameObject pet)
    {
        if (CountPets() >= maxPets || partyMembers.Contains(pet)) return;

        partyMembers.Add(pet);
        Debug.Log($"펫 {pet.name} 파티에 추가됨");

        UpdateFollowChain();
    }

    // NPC 추가 (최대 1명)
    public void AddNpc(GameObject npc)
    {
        if (CountNpcs() >= maxNpcs || partyMembers.Contains(npc)) return;

        partyMembers.Add(npc);
        Debug.Log($"NPC {npc.name} 파티에 추가됨");

        UpdateFollowChain();
    }

    // 파티 멤버 제거
    public void RemoveMember(GameObject member)
    {
        if (partyMembers.Remove(member))
            Debug.Log($"{member.name} 파티에서 제거됨");

        UpdateFollowChain();
    }

    // 현재 파티 내 펫 수 계산
    private int CountPets()
    {
        int count = 0;
        foreach (var member in partyMembers)
        {
            if (member.GetComponent<Pet>() != null)
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
        fullParty.AddRange(partyMembers);
        return fullParty;
    }

    // 정렬된 파티 멤버 반환 (우선순위: Player → NPC → Pet)
    public List<GameObject> GetSortedPartyMembers()
    {
        var sortedList = new List<GameObject>();

        if (player != null)
            sortedList.Add(player); // 무조건 맨 앞

        // NPC 1명만 추가
        foreach (var member in partyMembers)
        {
            if (member.GetComponent<NPC>() != null)
            {
                sortedList.Add(member);
                break;
            }
        }

        // 펫은 순서대로 최대 2마리 추가
        int petCount = 0;
        foreach (var member in partyMembers)
        {
            if (member.GetComponent<Pet>() != null)
            {
                if (petCount >= maxPets) break;
                sortedList.Add(member);
                petCount++;
            }
        }

        return sortedList;
    }

    // 정렬된 순서에 따라 따라가기 체인 설정
    private void UpdateFollowChain()
    {
        var sorted = GetSortedPartyMembers();

        for (int i = 1; i < sorted.Count; i++)
        {
            var follower = sorted[i].GetComponent<FollowerController>();
            if (follower != null)
            {
                follower.SetFollowTarget(sorted[i - 1].transform);
            }
        }
    }
}