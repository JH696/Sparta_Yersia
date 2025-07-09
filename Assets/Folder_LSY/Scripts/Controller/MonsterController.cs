using UnityEngine;

public class MonsterController : BaseCharacter
{
    [Header("몬스터 데이터")]
    [SerializeField, Tooltip("몬스터의 이름과 ID가 포함된 데이터")]
    private MonsterData monsterData;

    public MonsterData MonsterData => monsterData;

    private void Awake()
    {
        if (monsterData == null) return;

        InitStat(monsterData); // 스탯 초기화

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