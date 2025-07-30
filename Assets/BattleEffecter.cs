using UnityEngine;

public class BattleEffecter : MonoBehaviour
{
    [SerializeField] private B_Slot slot;

    [SerializeField] private Animator animator;

    [SerializeField] private float invokedDamage;

    public B_Slot Slot
    {
        get { return slot; }

        private set { slot = value; }
    }

    public void SetEffecter(string triggerName, float Damage)
    {
        invokedDamage = Damage;

        animator.SetTrigger(triggerName);
    }

    public void GainDamage()
    {
        Debug.Log("게인 데미지");

        slot.Character.TakeDamage(invokedDamage);

        invokedDamage = 0;
    }
}
