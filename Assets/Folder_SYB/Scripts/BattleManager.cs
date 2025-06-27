using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum EBattleState
{
    Start,
    Standby,
    Player,
    Enemy,
    Result,
}
public class BattleManager : MonoBehaviour
{
    EBattleState state;
    [Header("�׽�Ʈ��")]
    [SerializeField] int maxSpeed = 100;
    //�ൿ�� ������ �ൿ�� ������Ʈ
    GameObject playObject;
    //�ൿ�� ���ϴ� ������Ʈ
    GameObject targetObject;

    //�ϴ� ���� ���ǵ�� ����Ʈ
    List<int> speeds = new List<int>();
    //���ƴٴϴ� �÷��̾� ����Ʈ
    List<GameObject> playerList = new List<GameObject>();
    //���� ���ʹ� ����Ʈ
    List<GameObject> enemyList = new List<GameObject>();
    
    // Start is called before the first frame update
    void Start()
    {
        state = EBattleState.Start;
    }

    // Update is called once per frame
    void Update()
    {
        switch (state)
        {
            case EBattleState.Start:
                BattleStart();
                break;
            case EBattleState.Standby:
                StandbyPhase();
                break;
            case EBattleState.Player:
                PlayerPhase();
                break;
            case EBattleState.Enemy:
                EnemyPhase();
                break;
            case EBattleState.Result:
                ResultPhase();
                break;
        }
    }

    IEnumerator BattleStart()
    {
        //�÷��̾� ����Ʈ�� �ִ°͵� ���� -> ��ġ��� ����? UI��?
        //���ʹ� ����Ʈ�� �ִ°͵� ����
        //Battle Start! text
        yield return new WaitForSeconds(2f);
        state = EBattleState.Standby;
    }

    IEnumerator StandbyPhase()
    {
        //�ൿ�� ���� (�ð�*��ü�� �ൿ�¼�ġ)
        //�ൿ�� ������(100?) playObject�� �����ϰ� �ൿ������ ����
        //playObject�� ���ʹ� ������(���̾�?) ? state = EBattleState.Enemy : state = EBattleState.Player;
        yield break;
    }

    IEnumerator PlayerPhase()
    {
        //playObject�� ��ư ���ְ� ����/��ų/������/�޽�/���� ����
        yield break;
    }
    IEnumerator EnemyPhase()
    {
        //�÷��̾� �� ���� ���� -> targetObject�� ����
        //���� -> state = EBattleState.Result
        yield break;
    }
    IEnumerator ResultPhase()
    {
        //������ ���� 
        //targetObject�� �׾����� Ȯ�� -> ������ ����Ʈ���� ����
        //���ܵ����� ����Ʈ�� ���� ������� Ȯ�� -> ����ִٸ�
        //�÷��̾��϶� lose()
        //���ʹ̸� win()
        yield break;
    }

    void Attack_Player()
    {
        SelectTarget();
        //������
        //targetObject.TakeDamage(playObject.stat.Attack);

    }

    void SelectTarget()
    {
        //���콺Ŀ�� �����ٴ�� �ƿ�����/ȭ��ǥ ǥ��
        //Ŭ���ϸ� targetObject�� ����
    }

    void Skill_Player()
    {
        //������ ��ų����Ʈ ������ -> 
        SelectTarget();
    }

    void SelectRandomTarget()
    {
        int i = Random.Range(0, playerList.Count);
        targetObject = playerList[i];
    }


}
