using Unity.VisualScripting;
using UnityEngine;

[System.Serializable]
public class PetStatus : CharacterStatus
{
    [Header("펫의 진화 단계")]
    public int EvoLevel;

    // PetData 필드 (private 유지, 외부 직접 접근 제한)
    private new PetData data;

    /// <summary>
    /// PetData에 대한 읽기 전용 프로퍼티 (외부에서 접근 가능)
    /// </summary>
    public PetData Data => data;

    /// <summary>
    /// PetData를 받아 상태 초기화
    /// </summary>
    /// <param name="data">펫의 데이터</param>
    public override void Init(PetData data)
    {
        this.data = data;

        // CharacterStats 생성 및 데이터 기반 초기화
        CharacterStats cStat = new CharacterStats();
        cStat.SetBaseStats(data.GetComponent<StatData>());
        this.stat = cStat;

        EvoLevel = 1;
        stat.LevelUP += EvoLevelUp;
    }

    /// <summary>
    /// 생성자 (초기 상태와 스탯 지정)
    /// </summary>
    /// <param name="data">펫 데이터</param>
    /// <param name="s">캐릭터 스탯</param>
    public PetStatus(PetData data, CharacterStats s)
    {
        this.data = data;
        this.stat = s;
        stat.LevelUP += EvoLevelUp;
    }

    /// <summary>
    /// 레벨업 시 진화 단계 상승 처리
    /// </summary>
    public void EvoLevelUp()
    {
        EvoLevel++;
    }

    /// <summary>
    /// 현재 진화 단계에 맞는 스프라이트 반환
    /// </summary>
    /// <returns>PetSprite 또는 null</returns>
    public PetSprite GetPetSprite()
    {
        switch (EvoLevel)
        {
            case 1:
                return data.sprites[0];
            case 2:
                return data.sprites[1];
            case 3:
                return data.sprites[2];
            default:
                return null;
        }
    }
}