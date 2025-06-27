using UnityEngine;

public class PetController : BaseCharacter, ILevelable
{
    [Header("펫 데이터")]
    [SerializeField, Tooltip("펫의 이름과 ID가 포함된 데이터")] private PetData petData;

    public int Level { get; private set; } = 1;
    public int CurrentExp { get; private set; } = 0;
    public int ExpToNextLevel => 50 * Level;

    private int evoStage = 0;

    protected override void Awake()
    {
        base.Awake();
        if (petData == null || petData.StatData == null) return;
        Stat.InitFromData(petData.StatData);
    }

    private void Start()
    {
        Debug.Log($"펫 스탯 확인: HP {CurrentHp}/{MaxHp}, MP {CurrentMana}/{MaxMana}, Attack {Attack}, Defense {Defense}, Luck {Luck}, Speed {Speed}");
        ApplyEvoSprite(evoStage);
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

        if (Input.GetKeyDown(KeyCode.P))
        {
            Debug.Log($"펫 스탯 확인: HP {CurrentHp}/{MaxHp}, MP {CurrentMana}/{MaxMana}, Attack {Attack}, Defense {Defense}, Luck {Luck}, Speed {Speed}");
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
        Debug.Log($"펫 레벨업! 현재 레벨: {Level}");
        //Stat.MultiplyStats(1.1f);
        TryEvolve();
    }

    private void TryEvolve()
    {
        if (evoStage >= petData.evoLevels.Length) return;

        if (Level >= petData.evoLevels[evoStage].Level)
        {
            evoStage++;
            ApplyEvolutionData(evoStage);
            Debug.Log($"펫 진화 단계: {evoStage}");
        }
    }

    private void ApplyEvolutionData(int stage)
    {
        if (petData == null || petData.sprites == null || stage >= petData.sprites.Length) return;

        ApplyEvoSprite(stage);

        // 진화 시 현재 스탯에 배율 곱하기 (영구적 증가)
        Stat.MultiplyStats(petData.StatMultiplier);
    }

    private void ApplyEvoSprite(int stage)
    {
        var spriteData = petData.sprites[stage];
        var spriteRenderer = GetComponent<SpriteRenderer>();
        if (spriteRenderer == null || spriteData == null || spriteData.WorldSprite == null) return;
        
        spriteRenderer.sprite = spriteData.WorldSprite;
    }
}
