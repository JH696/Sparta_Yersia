using UnityEngine;

public class Pet : MonoBehaviour
{
    [Header("펫의 상태 정보")]
    public PetStatus status;

    [Header("월드에서 보여질 스프라이트")]
    public SpriteRenderer worldSprite;

    public PetData PetData => status?.PetData;
    public int Level => status?.stat.Level ?? 0;
    public int EvoLevel => status?.EvoLevel ?? 0;

    private void Awake()
    {
        if (worldSprite == null)
        {
            worldSprite = GetComponentInChildren<SpriteRenderer>();
            if (worldSprite == null)
            {
                Debug.LogError($"[{name}] worldSprite가 할당되어 있지 않고, 자식 오브젝트에서도 찾을 수 없습니다.");
            }
        }
    }

    /// <summary>
    /// PetStatus를 설정하고 스프라이트를 적용
    /// </summary>
    public void SetStatus(PetStatus newStatus)
    {
        status = newStatus;
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