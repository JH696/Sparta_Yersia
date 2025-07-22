using UnityEngine;

[System.Serializable]   
public class MonsterStatus : CharacterStatus
{
    public MonsterData data;

    public MonsterStatus(MonsterData data)
    {
        this.data = data;
        this.stat = new CharacterStats(data);
        this.skills = new SkillInventory(data);
    }

    public override Sprite GetWSprite()
    {
        return data.WSprite; // 몬스터의 월드 스프라이트 반환
    }
}