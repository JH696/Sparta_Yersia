using UnityEngine;

[System.Serializable]
public class PlayerStatus : CharacterStatus
{
    public PlayerParty party;
    public PlayerQuest quest;

    // 플레이어 이름
    public string PlayerName;
    // 플레이어 데이터
    public PlayerData PlayerData;
    public PlayerWallet Wallet;

    // 레벨 및 경험치
    public int Level { get; private set; } = 1;
    public int Exp { get; private set; } = 0;

    // 경험치 요구량 계산
    //public int ExpToNextLevel => PlayerData.BaseExpToLevelUp * Level;

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
}