using UnityEngine;

public class Monster : MonoBehaviour
{
    [Header("몬스터 상태 정보")]
    public MonsterStatus status;

    [Header("월드에서 보여질 스프라이트")]
    public SpriteRenderer worldSprite;

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

        if (status.MonsterData == null || status.MonsterData.MonsterSprite == null) return;

        worldSprite.sprite = status.MonsterData.MonsterSprite;
    }
}