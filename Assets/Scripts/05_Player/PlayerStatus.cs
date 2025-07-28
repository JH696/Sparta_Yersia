using UnityEngine;

[System.Serializable]
public class PlayerStatus : CharacterStatus
{
    public PlayerParty party;
    public PlayerQuest quest;
    // 인벤토리, 장비
    // 플레이어 이름
    public string PlayerName;
    // 플레이어 데이터
    public PlayerData PlayerData;
    public PlayerWallet Wallet;

    public PlayerStatus(PlayerData data, string playerName)
    {
        this.PlayerData = data;
        this.stat = new CharacterStats(data);

        PlayerName = string.IsNullOrEmpty(playerName) ? data.Name : playerName;

        party = new PlayerParty();
        inventory = new ItemInventory();
        quest = new PlayerQuest(inventory);
        equipment = new ItemEquipment(this);
        skills = new SkillInventory(PlayerData);
        Wallet = new PlayerWallet();
    }

    public void SetPlayerName(string name)
    {
        PlayerName = name;
    }

    public override Sprite GetWSprite()
    {
        // 플레이어의 월드 스프라이트 반환
        return PlayerData.WSprite;
    }
}