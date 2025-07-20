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