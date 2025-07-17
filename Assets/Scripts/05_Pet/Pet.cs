using UnityEngine;

public class Pet : MonoBehaviour
{
    [Header("펫의 상태 정보")]
    public PetStatus status;

    [Header("월드에서 보여질 스프라이트")]
    public SpriteRenderer worldSprite;

    /// <summary>
    private void Start()
    {
        ChangeSprite();
    }

    /// <summary>
    /// 현재 상태에 따른 스프라이트로 변경 적용
    /// </summary>
    public void ChangeSprite()
    {
        if (status == null) return;

        PetSprite sprite = status.GetPetSprite();
        if (sprite == null || sprite.WorldSprite == null) return;

        worldSprite.sprite = sprite.WorldSprite;
    }
}

//using UnityEngine;

//public class Pet : BaseCharacter, ILevelable
//{
//    [Header("펫 데이터")]
//    [SerializeField, Tooltip("펫의 이름과 ID가 포함된 데이터")]
//    private PetData petData;
//    [SerializeField] private CharacterSkill skill;

//    //public override Sprite Icon => petData.Icon;

//    public PetData PetData => petData; // 읽기 전용
//    public CharacterSkill Skill => skill; // 읽기 전용

//    // 레벨, 경험치
//    public int Level { get; private set; } = 1;
//    public int CurrentExp { get; private set; } = 0;
//    public int ExpToNextLevel => 50 * Level;

//    private void Awake()
//    {
//        if (petData == null) return;

//        DontDestroyOnLoad(this.gameObject);

//        InitStat(petData); // 스탯 초기화
//        skill.Init(petData.startingSkills);
//        Level = 1;
//        CurrentExp = 0;

//        ApplyEvoSprite(petData.CurrentEvoStage);
//    }

//    private void Update()
//    {
//        // 테스트용 키
//        if (Input.GetKeyDown(KeyCode.H)) HealHP(10f);
//        if (Input.GetKeyDown(KeyCode.J)) TakeDamage(20f);
//        if (Input.GetKeyDown(KeyCode.E)) AddExp(30);
//    }

//    // 경험치 추가 메서드
//    public void AddExp(int amount)
//    {
//        CurrentExp += amount;
//        while (CurrentExp >= ExpToNextLevel)
//        {
//            CurrentExp -= ExpToNextLevel;
//            LevelUp();
//        }
//    }

//    public void LevelUp()
//    {
//        Level++;
//        Debug.Log($"펫 레벨업 현재 레벨: {Level}");
//        float multiplier = PetData == null ? 1.1f : PetData.StatMultiplierPerLevel;
//        Stat.MultiplyStats(multiplier);
//        TryEvolve();
//    }

//    private void TryEvolve()
//    {
//        if (petData == null || petData.evoLevels == null) return;

//        int currentStage = petData.CurrentEvoStage;
//        if (currentStage >= petData.evoLevels.Length) return;

//        if (Level >= petData.evoLevels[currentStage].Level)
//        {
//            // 진화 단계 증가
//            petData.CurrentEvoStage++;
//            ApplyEvolutionData(petData.CurrentEvoStage);
//            Debug.Log($"펫 진화됨 현재 단계: {petData.CurrentEvoStage}");
//        }
//    }

//    private void ApplyEvolutionData(int stage)
//    {
//        ApplyEvoSprite(stage);
//        // 진화 시 스탯 증가
//        Stat.MultiplyStats(petData.StatMultiplier);
//    }

//    private void ApplyEvoSprite(int stage)
//    {
//        if (petData == null) return;

//        var spriteData = petData.sprites.Length > stage ? petData.sprites[stage] : null;
//        if (spriteData == null || spriteData.WorldSprite == null) return;

//        var spriteRenderer = GetComponent<SpriteRenderer>();
//        if (spriteRenderer != null)
//            spriteRenderer.sprite = spriteData.WorldSprite;
//    }
//}