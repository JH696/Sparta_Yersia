using UnityEngine;

public class Monster : BaseCharacter
{
    [Header("몬스터 데이터")]
    [SerializeField, Tooltip("몬스터의 이름과 ID가 포함된 데이터")]
    private MonsterData monsterData;
    [SerializeField] private CharacterSkill skill;

    public override Sprite Icon => monsterData.Icon;

    public MonsterData MonsterData => monsterData;
    public CharacterSkill Skill => skill; // 읽기 전용

    private void Awake()
    {
        if (monsterData == null) return;

        InitStat(monsterData); // 스탯 초기화
        skill.Init(monsterData.startingSkills);
        ApplyMonsterSprite();
    }

    private void ApplyMonsterSprite()
    {
        if (monsterData == null || monsterData.WorldSprite == null) return;

        var spriteRenderer = GetComponent<SpriteRenderer>();
        if (spriteRenderer != null)
        {
            spriteRenderer.sprite = monsterData.WorldSprite;
        }
    }
}