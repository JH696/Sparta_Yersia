using UnityEngine;

[System.Serializable]
public class PlayerStatus : CharacterStatus
{
    public PlayerParty party;
    public PlayerQuest quest;
    // 인벤토리, 장비
    public ItemInventory inventory;
    public ItemEquipment equipment;
    // 스킬
    public SkillInventory skills;
    // 플레이어 이름
    public string PlayerName;
    // 플레이어 데이터
    public PlayerData PlayerData;
    public PlayerWallet wallet;

    // 레벨 및 경험치
    public int Level { get; private set; } = 1;
    public int Exp { get; private set; } = 0;

    // 경험치 요구량 계산
    public int ExpToNextLevel => PlayerData.BaseExpToLevelUp * Level;

    public PlayerStatus(PlayerData data, string playerName)
    {
        PlayerName = playerName;

        this.PlayerData = data;
        this.stat = new CharacterStats(data);

        party = new PlayerParty();
        quest = new PlayerQuest();
        inventory = new ItemInventory();
        skills = new SkillInventory();
        PlayerName = playerName;
        wallet = new PlayerWallet();
    }

    // 플레이어만 가질 수 있는 기능들
    public void SetLevel(int level)
    {
        Level = Mathf.Max(1, level);
    }

    public void SetExp(int exp)
    {
        Exp = Mathf.Max(0, exp);
    }

    public bool TryAddExp(int amount)
    {
        Exp += amount;
        bool leveledUp = false;

        while (Exp >= ExpToNextLevel)
        {
            Exp -= ExpToNextLevel;
            Level++;
            leveledUp = true;
        }

        return leveledUp;
    }
}