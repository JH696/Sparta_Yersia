using System;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class PlayerParty
{
    [Header("파티 제한")]
    [SerializeField] private int maxPartyPets = 2;

    public List<PetStatus> curPets  = new List<PetStatus>(); // 보유 중인 전체 펫

    public List<PetStatus> partyPets = new List<PetStatus>(); // 장착한 펫

    //프리팹 (빈 오브젝트 + 펫 스크립트, 팔로워)

    public PlayerParty()
    {
        // 초기화 작업
    }



    //    public void Awake()
    //    {
    //        // 전체 펫 불러오기
    //        //curPets = SaveManger.Pets;
    //        //// 파티 펫도 불러오기
    //        //partyPets = SaveManger.PartyPets ?? new List<PetStatus>();
    //    }

    //    public void AddPet(PetStatus status)
    //    {
    //        // 보유 중인지 확인하고 보유 중이라면 장착 리스트에 포함 시킨다
    //        if (status == null)
    //        {
    //            Debug.LogWarning("추가할 펫이 null입니다.");
    //            return;
    //        }

    //        // 이미 보유 중인지 확인
    //        if (!curPets.Contains(status))
    //        {
    //            Debug.LogWarning($"보유하지 않은 펫을 파티에 추가할 수 없습니다: {status.data}");
    //            return;
    //        }

    //        // 이미 파티에 포함되었는지, 최대 수 초과 체크
    //        if (partyPets.Contains(status))
    //        {
    //            Debug.Log($"이미 파티에 포함된 펫입니다: {status.data}");
    //            return;
    //        }

    //        if (partyPets.Count >= maxPartyPets)
    //        {
    //            Debug.LogWarning($"파티에는 최대 {maxPartyPets}마리의 펫만 장착할 수 있습니다.");
    //            return;
    //        }

    //        partyPets.Add(status);
    //        Debug.Log($"펫 {status.data} 파티에 추가됨");

    //        RefreshPartyState();
    //    }

    //    public void RemovePet(PetStatus status)
    //    {
    //        // 파티에서 제거
    //        if (partyPets.Remove(status))
    //        {
    //            Debug.Log($"펫 {status.data} 파티에서 제거됨");

    //            RefreshPartyState();
    //        }
    //        else
    //        {
    //            Debug.LogWarning($"파티에 없는 펫입니다: {status.data}");
    //        }
    //    }

    //    public void SaveCurPets() // 전투 복귀시 전체 펫을 잃지 않기 위해
    //    {
    //        // 세이브 매니저로 넘겨주기
    //        SaveManger.Pets = curPets;
    //        Debug.Log($"현재 보유 펫 {curPets.Count}마리 저장 처리");
    //    }

    //    public void SavePartyPets() // 전투에서 참조 
    //    {
    //        // 세이브 매니저로 넘겨주기
    //        SaveManger.PartyPets = partyPets;
    //        Debug.Log($"현재 파티 펫 {partyPets.Count}마리 저장 처리");
    //    }

    //    /// <summary>
    //    /// 파티 펫을 순서대로 반환 (필요시 추후 Follow 시스템에 사용)
    //    /// </summary>
    //    public List<PetStatus> GetOrderedParty()
    //    {
    //        return new List<PetStatus>(partyPets);
    //    }

    //    /// <summary>
    //    /// 파티 상태 변경 후 호출: 팔로우 체인 갱신, UI 갱신 등 후처리 수행
    //    /// </summary>
    //    public void RefreshPartyState()
    //    {
    //        RefreshFollowChain();
    //        RefreshUI();
    //    }

    //    private void RefreshFollowChain()
    //    {
    //        // TODO: 파티 펫 순서대로 팔로우 체인 설정
    //        // 예시: partyPets 순서대로 Follower 컴포넌트 연결
    //        for (int i = 1; i < partyPets.Count; i++)
    //        {
    //            var follower = partyPets[i].GetComponent<Follower>();
    //            if (follower != null)
    //            {
    //                follower.target = partyPets[i - 1].transform;
    //            }
    //        }
    //    }

    //    private void RefreshUI()
    //    {
    //        // TODO: UI 갱신 처리
    //        // 예) UIManager.Instance.RefreshPartyUI(partyPets);
    //        Debug.Log("파티 UI 갱신 처리");
    //    }
}

///// <summary>
///// 플레이어 게임오브젝트와 함께
///// 펫과 NPC 파티원을 관리하는 스크립트
///// 배틀씬 진입 시 파티 데이터를 넘길 때
///// 플레이어도 포함하도록 설계됨
///// </summary>
//public class PlayerParty : MonoBehaviour
//{
//    [Header("파티 인원 제한")]
//    [SerializeField] private int maxPets = 2;
//    [SerializeField] private int maxNpcs = 1;

//    [Header("플레이어 게임오브젝트")]
//    [SerializeField] private GameObject player;

//    [Header("파티 멤버 리스트 (펫, NPC 혼합)")]
//    [SerializeField] private List<GameObject> partyMembers = new List<GameObject>();

//    private void Start()
//    { 
//        if (player == null)
//        {
//            player = GameManager.Instance.Player;
//        }

//        // 실행 시 초기 상태에 맞게 팔로우 체인 갱신
//        UpdateFollowChain();
//    }

//    // 펫 추가 (최대 2마리)
//    public void AddPet(GameObject pet)
//    {
//        if (CountPets() >= maxPets || partyMembers.Contains(pet)) return;

//        partyMembers.Add(pet);
//        Debug.Log($"펫 {pet.name} 파티에 추가됨");

//        UpdateFollowChain();
//    }

//    // NPC 추가 (최대 1명)
//    public void AddNpc(GameObject npc)
//    {
//        if (CountNpcs() >= maxNpcs || partyMembers.Contains(npc)) return;

//        partyMembers.Add(npc);
//        Debug.Log($"NPC {npc.name} 파티에 추가됨");

//        UpdateFollowChain();
//    }

//    // 파티 멤버 제거
//    public void RemoveMember(GameObject member)
//    {
//        if (partyMembers.Remove(member))
//            Debug.Log($"{member.name} 파티에서 제거됨");

//        UpdateFollowChain();
//    }

//    // 현재 파티 내 펫 수 계산
//    private int CountPets()
//    {
//        int count = 0;
//        foreach (var member in partyMembers)
//        {
//            if (member.GetComponent<Pet>() != null)
//                count++;
//        }
//        return count;
//    }

//    // 현재 파티 내 NPC 수 계산
//    private int CountNpcs()
//    {
//        int count = 0;
//        foreach (var member in partyMembers)
//        {
//            if (member.GetComponent<NPC>() != null)
//                count++;
//        }
//        return count;
//    }

//    // 전체 파티 멤버 반환 (플레이어 포함)
//    public List<GameObject> GetFullPartyMembers()
//    {
//        var fullParty = new List<GameObject>();
//        fullParty.AddRange(partyMembers);
//        return fullParty;
//    }

//    // 정렬된 파티 멤버 반환 (우선순위: Player → NPC → Pet)
//    public List<GameObject> GetSortedPartyMembers()
//    {
//        var sortedList = new List<GameObject>();

//        if (player != null)
//            sortedList.Add(player); // 무조건 맨 앞

//        // NPC 1명만 추가
//        foreach (var member in partyMembers)
//        {
//            if (member.GetComponent<NPC>() != null)
//            {
//                sortedList.Add(member);
//                break;
//            }
//        }

//        // 펫은 순서대로 최대 2마리 추가
//        int petCount = 0;
//        foreach (var member in partyMembers)
//        {
//            if (member.GetComponent<Pet>() != null)
//            {
//                if (petCount >= maxPets) break;
//                sortedList.Add(member);
//                petCount++;
//            }
//        }

//        return sortedList;
//    }

//    // 정렬된 순서에 따라 따라가기 체인 설정
//    private void UpdateFollowChain()
//    {
//        var sorted = GetSortedPartyMembers();

//        for (int i = 1; i < sorted.Count; i++)
//        {
//            var follower = sorted[i].GetComponent<FollowerController>();
//            if (follower != null)
//            {
//                follower.SetFollowTarget(sorted[i - 1].transform);
//            }
//        }
//    }
//}