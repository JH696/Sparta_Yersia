using System.Threading;
using UnityEngine;

public class B_CharacterSlot : MonoBehaviour
{
    [Header("등록된 캐릭터")]
    [SerializeField] private BaseCharacter character;

    [Header("캐릭터 상태 UI")]
    [SerializeField] private B_CharacterStatUI cStatUI;

    [Header("행동력, 게이지")]
    [SerializeField] private float actionPoint;
    [SerializeField] private B_ActionGauge aGauge;

    [Header("포인터")]
    [SerializeField] private GameObject pointer;

    [Header("캐릭터 헹동 중 여부")]
    [SerializeField] private bool hasTurn;

    public BaseCharacter Character => character;

    public void Start()
    {
        character.StatusChanged += UpdateStatUI;
    }

    public void OnDisable()
    {
        character.StatusChanged -= UpdateStatUI;
    }

    public virtual void SetCharSlot(GameObject character)
    {
        this.character = character.GetComponent<BaseCharacter>();
        aGauge.SetGauge(this, null);
        cStatUI.SetProfile(this.character);
        this.gameObject.SetActive(true);
    }

    public virtual void ResetSlot()
    {
        actionPoint = 0;
        //character = null;
        aGauge.ResetGauge();
        this.gameObject.SetActive(false);
    }

    public void UpdateStatUI()
    {
        cStatUI.RefreshGauge(character);

        if (character.IsDead)
        {
            Debug.Log("캐릭터가 죽었습니다");

            B_Manager.Instance.UpACount();
            ResetSlot();
        }
    }

    public void IncreaseAPoint()
    {
        if (character == null || character.IsDead) return;

        actionPoint += character.Speed * Time.deltaTime;

        if (actionPoint >= 100)
        {
            actionPoint = 0;
            TurnStart();
        }

        aGauge.RefreshGauge(actionPoint);
    }

    public void SetPointer()
    {
        pointer.SetActive(true);
    }

    public void ResetPointer()
    {
        pointer.SetActive(false);   
    }

    public CharacterSkill GetLearnedSkill()
    {
        if (character is Player player)
        {
            return player.Skill;
        }
        else if (character is NPC npc)
        {
            return npc.Skill;
        }
        else if (character is Pet pet)
        {
            return pet.Skill;
        }
        else if (Character is Monster monster)
        {
            return monster.Skill;
        }

        Debug.Log("알 수 없는 유형입니다.");
        return null;
    }

    public bool HasTurn()
    {
        return hasTurn;
    }

    private void TurnStart()
    {
        hasTurn = true;
        aGauge.gameObject.SetActive(false);
    }

    public void TurnEnd()
    {
        hasTurn = false;
        aGauge.gameObject.SetActive(true);
    }
}
