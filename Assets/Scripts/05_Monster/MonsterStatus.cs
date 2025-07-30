using UnityEngine;

[System.Serializable]   
public class MonsterStatus : CharacterStatus
{
    // 몬스터 데이터
    public MonsterData data;
    // 몬스터 스탯
    //// 몬스터 보상
    //public DropTableSO dropTable;
    //public int ExpReward;

    public MonsterStatus(MonsterData data)
    {
        this.data = data;
        stat = new CharacterStats(data);
        skills = new SkillInventory(data);
        //this.dropTable = data.dropTable;
        //this.ExpReward = data.ExpReward;
    }

    public override void CharacterDie()
    {
        base.CharacterDie();
        GameManager.player.quest.KillMonster(data); // 몬스터 처치 시 퀘스트 업데이트
    }

    public override Sprite GetWSprite()
    {
        return data.BattleVisuals.Stand; // 몬스터의 월드 스프라이트 반환
    }

}