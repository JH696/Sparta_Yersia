using UnityEngine;

public class PetController : BaseCharacter, ILevelable
{
    public int Level { get; private set; } = 1;
    public int CurrentExp { get; private set; } = 0;
    public int ExpToNextLevel => 50 * Level;

    private void Start()
    {
        Debug.Log($"펫 스탯 확인: HP {CurrentHp}/{MaxHp}, MP {CurrentMana}/{MaxMana}, Attack {Attack}, Defense {Defense}, Luck {Luck}, Speed {Speed}");
    }

    private void Update()
    {
        // 테스트용: 키 입력 시 데미지 입거나 회복
        if (Input.GetKeyDown(KeyCode.K))  // K 누르면 힐 10
        {
            Heal(10f);
            Debug.Log($"펫 힐 받음: 현재 체력 {CurrentHp}/{MaxHp}");
        }
        if (Input.GetKeyDown(KeyCode.L))  // L 누르면 데미지 20
        {
            TakeDamage(20f);
            Debug.Log($"펫 데미지 입음: 현재 체력 {CurrentHp}/{MaxHp}");
        }

        // 테스트용: E 키 누르면 경험치 30 추가
        if (Input.GetKeyDown(KeyCode.E))
        {
            AddExp(20);
            Debug.Log($"펫 경험치: {CurrentExp} / {ExpToNextLevel}, 레벨: {Level}");
        }
    }

    // 경험치 추가 메서드
    public void AddExp(int amount)
    {
        CurrentExp += amount;
        while (CurrentExp >= ExpToNextLevel)
        {
            CurrentExp -= ExpToNextLevel;
            LevelUp();
        }
    }

    public void LevelUp()
    {
        Level++;
        // TODO: 레벨업 시 스탯 증가 및 UI 갱신 처리
        Debug.Log($"펫 레벨업! 현재 레벨: {Level}");
    }
}
