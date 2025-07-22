using System.Collections.Generic;
using UnityEngine;

public class B_ActionHandler : MonoBehaviour
{
    [Header("타겟 목록")]
    [SerializeField] private List<CharacterStatus> targets = new List<CharacterStatus>();

    [Header("최대 타겟 수")]
    [SerializeField] private int maxCount = 1;

    [Header("타겟팅 여부")]
    [SerializeField] private bool isTargeting;

    [Header("슬롯 매니저")]
    [SerializeField] private B_SlotManager slotManager;


    public List<CharacterStatus> Targets => targets;

    public void StartTargeting(int maxCount)
    {
        targets.Clear();
        this.maxCount = maxCount;
        isTargeting = true;
    }

    private void Update()
    {
        if (isTargeting)
        {
            if (Input.GetMouseButtonDown(0))
            {
                Vector2 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
                RaycastHit2D hit = Physics2D.Raycast(mousePos, Vector2.zero);

                CharacterStatus target = hit.collider?.GetComponent<B_Slot>().Character;

                if (target != null && targets.Count < maxCount)
                {
                    if (target.IsDead)
                    {
                        Debug.Log("이미 죽은 캐릭터입니다.");
                        return;
                    }

                    targets.Add(target);
                    Debug.Log($"타겟 추가: {target}");
                }
            }
        }
    }

    // 대상 반환 초기화 

    public void EndTargeting(bool isDead)
    {
        Debug.Log($"타겟팅 종료, isDead: {isDead}");

        targets.Clear();
        isTargeting = false;
        slotManager.ClearCurrentSlot();
    }
}
