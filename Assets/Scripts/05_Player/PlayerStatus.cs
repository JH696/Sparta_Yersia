
using UnityEngine;

[System.Serializable]
public class PlayerStatus : CharacterStatus
{
    public PlayerParty party;
    public PlayerQuest quest;

    // 플레이어 이름
    public string PlayerName; // 플레이어 이름

    public PlayerData PlayerData;
    public PlayerParty Party => party;

    public PlayerStatus(PlayerData data, string playerName)
    {
        PlayerName = playerName;

        this.stat = new CharacterStats(data);

        party = new PlayerParty();
        quest = new PlayerQuest();
        inventory = new ItemInventory();
        equipment = new ItemEquipment(this);
        skills = new SkillInventory(data);
        PlayerData = data;
    }

    public override Sprite GetWSprite()
    {
        return PlayerData.WSprite;
    }


    // 플레이어만 가질 수 있는 기능들    
}

//public class PlayerParty2 : MonoBehaviour // 파티 편성 기능
//{
//    public List<PetStatus> curPets; // 보유 중인 전체 펫

//    public List<PetStatus> partyPets; // 장착한 펫

//    //프리팹 (빈 오브젝트 + 펫 스크립트, 팔로워)

//    public void Awake()
//    {
//        curPets = SaveManger.Pets; // 전체 펫 불러오기
//        // 파티 펫도 불러오기
//    }

//    public void AddPet(PetStatus status)
//    {
//        // 보유 중인지 확인하고 보유 중이라면 장착 리스트에 포함 시킨다
//    }

//    public void SaveCurPets() // 전투 복귀시 전체 펫을 잃지 않기 위해
//    {
//        // 세이브 매니저로 넘겨주기
//    }

//    public void SavePartyPets() // 전투에서 참조 
//    {
//        // 세이브 매니저로 넘겨주기
//    }
//}
