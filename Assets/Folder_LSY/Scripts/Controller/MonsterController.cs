using UnityEngine;

public class MonsterController : BaseCharacter
{
    [Header("몬스터 데이터")]
    [SerializeField, Tooltip("몬스터의 이름과 ID가 포함된 데이터")] private MonsterData monsterData;

    //protected override void Awake()
    //{
    //    base.Awake();
    //    if (monsterData == null || monsterData.WorldSprite == null) return;

    //    var spriteRenderer = GetComponent<SpriteRenderer>();
    //    if (spriteRenderer != null)
    //    {
    //        spriteRenderer.sprite = monsterData.WorldSprite;
    //    }
    //}

    private void Start()
    {
        Debug.Log($"몬스터 스탯 확인: HP {CurrentHp}/{MaxHp}, MP {CurrentMana}/{MaxMana}, Attack {Attack}, Defense {Defense}, Luck {Luck}, Speed {Speed}");
    }
}