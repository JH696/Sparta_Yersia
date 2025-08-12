using UnityEngine;

[System.Serializable]   
public class MonsterStatus : CharacterStatus
{
    // 몬스터 데이터
    public MonsterData data;

    public MonsterStatus(MonsterData data)
    {
        this.data = data;
        stat = new CharacterStats(data);
        skills = new SkillInventory(data);
    }

    public override void CharacterDie()
    {
        base.CharacterDie();
        SoundManager.Instance.PlaySFX(SFXType.Die, 0.2f);
        GameManager.player.quest.KillMonster(data); // 몬스터 처치 시 퀘스트 업데이트
    }

    public override E_SizeType GetSize()
    {
        return data.Size;
    }

    public override BattleVisuals GetBattleVisuals()
    {
        return data.BattleVisuals;
    }
}