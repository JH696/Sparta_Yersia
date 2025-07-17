using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEditor.Rendering;
using UnityEngine;

public class Test_Pet : MonoBehaviour 
{
    public PetStatus status;
    public SpriteRenderer worldSpri;

    public void Start()
    {
        ChangeS();
    }

    public void ChangeS() // 실제 적용
    {
        worldSpri.sprite = status.GetPetSprite().WorldSprite;
    }
}

public abstract class CharacterStatus
{
    public PetData data;
    public CharacterStats stat;
    public bool IsDead;

    public abstract void Init(PetData data);

    public virtual void TakeDamage(float amount)
    {
        if (IsDead) return;

        stat.SetCurrentHp(stat.CurrentHp - amount);

        if (IsDead)
        {
            CharacterDie();
        }
    }

    public virtual void RecoverHealth(float amount)
    {
        if (IsDead) return;

        stat.SetCurrentHp(amount);
    }

    public virtual void ConsumeMana(float amount)
    {
        if (IsDead) return;

        stat.SetCurrentMana(-amount);
    }

    public virtual void RecoverMana(float amount)
    {
        if (IsDead) return;

        stat.SetCurrentMana(amount);
    }

    public virtual void CharacterDie()
    {
        IsDead = true;
    }

    public virtual void Revive()
    {
        stat.CurrentHp = 1;
        IsDead = false;
    }
}

public class playerStatus : CharacterStatus
{
    public int YP;

    public override void Init(PetData data)
    {
        this.data = data;

        CharacterStats cStat = new CharacterStats();
        cStat.SetBaseStats(data.GetComponent<StatData>());
        this.stat = cStat;

        YP = 0;
    }

}

[System.Serializable]
public class PetStatus : CharacterStatus
{
    public int EvoLevel;

    public override void Init(PetData data)
    {
        this.data = data;

        CharacterStats cStat = new CharacterStats();
        cStat.SetBaseStats(data.GetComponent<StatData>());
        this.stat = cStat;

        EvoLevel = 1;
        stat.LevelUP += EvoLevelUp;
    }

    public PetStatus(PetData data, CharacterStats s)
    {
        stat.LevelUP += EvoLevelUp;
    }

    public void EvoLevelUp()
    {
        EvoLevel++;
    }

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

public class SaveManger // 싱글톤, 씬 이동시 데이터 저장, 불러오기
{
    public static List<PetStatus> Pets;
}

public class PlayerParty2 : MonoBehaviour // 파티 편성 기능
{
    public List<PetStatus> curPets; // 보유 중인 전체 펫

    public List<PetStatus> partyPets; // 장착한 펫

    //프리팹 (빈 오브젝트 + 펫 스크립트, 팔로워)

    public void Awake()
    {
        curPets = SaveManger.Pets; // 전체 펫 불러오기
        // 파티 펫도 불러오기
    }

    public void AddPet(PetStatus status)
    {
        // 보유 중인지 확인하고 보유 중이라면 장착 리스트에 포함 시킨다
    }

    public void SaveCurPets() // 전투 복귀시 전체 펫을 잃지 않기 위해
    {
        // 세이브 매니저로 넘겨주기
    }

    public void SavePartyPets() // 전투에서 참조 
    {
        // 세이브 매니저로 넘겨주기
    }
}

public class Follower : MonoBehaviour
{
    public Transform target;

    private void Start()
    {
       target = GetComponentInParent<Transform>();  
    }

    private void Update()
    {
        // 추적 기능
    }
}
