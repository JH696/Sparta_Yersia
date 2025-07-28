using UnityEngine;

public class Monster : MonoBehaviour
{
    [Header("몬스터 상태")]
    [SerializeField] private MonsterStatus status;

    [Header("몬스터 데이터")]
    [SerializeField] private MonsterData monsterData;

    [Header("월드에서 보여질 스프라이트")]
    public SpriteRenderer worldSprite;

    public MonsterStatus Status => status; // 읽기 전용

    private void Start()
    {
        if (status == null)
        {
            status = new MonsterStatus(monsterData); 
        }

        ChangeSprite();
    }



    private void ChangeSprite()
    {
        if (status == null) return;
        worldSprite.sprite = monsterData.WSprite;
    }
}
