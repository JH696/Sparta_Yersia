using UnityEngine;

public class CharacterSlot : MonoBehaviour
{
    [Header("캐릭터")]
    [SerializeField] private BaseCharacter curChar;

    [Header("캐릭터 스킬")]
    [SerializeField] private CharacterSkill curSkill;

    [Header("행동 게이지")]
    [SerializeField] private ActionGauge actionGauge;

    [Header("행동력")]
    [SerializeField] private float actionPoint;

    [Header("행동 중 여부")]
    [SerializeField] private bool isActing = false;

    private void Start()
    {
        Test_BattleManager.Instance.OnAction += SomeoneActing;
    }
    private void OnDisable()
    {
        Test_BattleManager.Instance.OnAction -= SomeoneActing;
    }

    void Update()
    {
        IncreaseActionPoints(curChar.Speed * Time.deltaTime);
    }

    public BaseCharacter GetCurChar()
    {
        return curChar;
    }

    public CharacterSkill GetCurSkill()
    {
        return curSkill;
    }

    public void LinkGauge(ActionGauge actionGauge)
    {
        this.actionGauge = actionGauge; 
    }

    // 행동력 증가
    private void IncreaseActionPoints(float amount)
    {
        if (curChar == null || curChar.IsDead || isActing) return;

        actionPoint += amount;

        if (actionPoint >= 100f)
        {
            actionPoint = 0f;
            actionGauge.HideGauge();
            Test_BattleManager.Instance.StartAction(this);
        }

        actionGauge.RefreshGauge(actionPoint);
    }

    private void SomeoneActing()
    {
        isActing = true;
    }
}
