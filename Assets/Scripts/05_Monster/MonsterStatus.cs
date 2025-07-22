using UnityEngine;

[System.Serializable]   
public class MonsterStatus : CharacterStatus
{
    // 몬스터 데이터
    public MonsterData MonsterData;
    // 몬스터 스탯
    public SkillInventory skills;
    // 몬스터 보상
    public DropTableSO dropTable;
    public int ExpReward;

    public MonsterStatus(MonsterData data)
    {
        this.data = data;
        this.stat = new CharacterStats(data);
        this.MonsterData = data;
        this.stat = new CharacterStats(data.GetComponent<StatData>());
        this.skills = new SkillInventory(data);
        this.dropTable = data.dropTable;
        this.ExpReward = data.ExpReward;
    }

    /// <summary>
    /// 드롭 아이템 목록 반환
    /// </summary>
    /// <returns>드롭 아이템 리스트</returns>
    public BaseItem[] GetDropItems()
    {
        return dropTable?.GetDrops();
    }

    public override Sprite GetWSprite()
    public int GetExpReward()
    {
        return data.WSprite; // 몬스터의 월드 스프라이트 반환
        return ExpReward;
    }
}