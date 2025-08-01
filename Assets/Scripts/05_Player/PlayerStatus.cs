using UnityEngine;

[System.Serializable]
public class PlayerStatus : CharacterStatus
{   
    // 이름, 랭크, 스킬 포인트
    public string PlayerName;
    public E_Rank Rank;
    public int SkillPoints;

    // 파티, 퀘스트
    public PlayerParty party;
    public PlayerQuest quest;

    // 데이터, 지갑
    public PlayerData PlayerData;
    public PlayerWallet Wallet;


    public PlayerStatus(PlayerData data, string playerName)
    {
        this.PlayerData = data;
        this.stat = new CharacterStats(data);

        PlayerName = string.IsNullOrEmpty(playerName) ? data.Name : playerName;
        Rank = data.Rank;
        SkillPoints = 1;

        party = new PlayerParty();
        inventory = new ItemInventory();
        quest = new PlayerQuest(inventory);
        equipment = new ItemEquipment(this);
        skills = new SkillInventory(PlayerData);
        Wallet = new PlayerWallet();

        stat.LevelUP += LevelUp; // 레벨업 시 랭크 상승 처리
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

    private void LevelUp()
    {
        SkillPoints++;

        if (stat.Level % 10 == 0)
        {
            if ((int)Rank < (int)E_Rank.Expert)
            {
                Rank = (E_Rank)((int)Rank + 1);
            }
        }
    }

    public override BattleVisuals GetBattleVisuals()
    {
        return PlayerData.BattleVisuals;
    }
}
