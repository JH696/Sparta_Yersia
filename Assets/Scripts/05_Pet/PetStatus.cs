using Unity.VisualScripting;
using UnityEngine;

[System.Serializable]
public class PetStatus : CharacterStatus
{
    [Header("펫 데이터")]
    public PetData PetData;

    [Header("펫의 진화 단계")]
    public int EvoLevel;

    /// <summary>
    /// 생성자 (초기 상태와 스탯 지정)
    /// </summary>
    public PetStatus(PetData data)
    {
        this.PetData = data;
        this.stat = new CharacterStats(data.GetComponent<StatData>());

        EvoLevel = 1;
        stat.LevelUP += EvoLevelUp;
    }

    /// <summary>
    /// 레벨업 시 진화 단계 상승 처리
    /// </summary>
    public void EvoLevelUp()
    {
        //    if (EvoLevel >= 3) return;

        //if (stat.Level % PetData.EvoLevel(5))
        //{
        //    EvoLevel++;
        //}
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
                return PetData.sprites[0];
            case 2:
                return PetData.sprites[1];
            case 3:
                return PetData.sprites[2];
            default:
                return null;
        }
    }
}