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
    [Header("테스트용")]
    [SerializeField] int maxSpeed = 100;
    //행동력 다차서 행동할 오브젝트
    GameObject playObject;
    //행동에 당하는 오브젝트
    GameObject targetObject;

    //일단 각자 스피드들 리스트
    List<int> speeds = new List<int>();
    //돌아다니는 플레이어 리스트
    List<GameObject> playerList = new List<GameObject>();
    //만난 에너미 리스트
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
        //플레이어 리스트에 있는것들 생성 -> 위치잡고 생성? UI로?
        //에너미 리스트에 있는것들 생성
        //Battle Start! text
        yield return new WaitForSeconds(2f);
        state = EBattleState.Standby;
    }

    IEnumerator StandbyPhase()
    {
        //행동력 증가 (시간*개체별 행동력수치)
        //행동력 다차면(100?) playObject에 저장하고 행동력증가 멈춤
        //playObject가 에너미 맞으면(레이어?) ? state = EBattleState.Enemy : state = EBattleState.Player;
        yield break;
    }

    IEnumerator PlayerPhase()
    {
        //playObject의 버튼 켜주고 공격/스킬/아이템/휴식/도주 선택
        yield break;
    }
    IEnumerator EnemyPhase()
    {
        //플레이어 중 랜덤 선택 -> targetObject에 저장
        //공격 -> state = EBattleState.Result
        yield break;
    }
    IEnumerator ResultPhase()
    {
        //데미지 정산 
        //targetObject가 죽었는지 확인 -> 맞으면 리스트에서 제외
        //제외됐을때 리스트가 전부 비었는지 확인 -> 비어있다면
        //플레이어일때 lose()
        //에너미면 win()
        yield break;
    }

    void Attack_Player()
    {
        SelectTarget();
        //데미지
        //targetObject.TakeDamage(playObject.stat.Attack);

    }

    void SelectTarget()
    {
        //마우스커서 가져다대면 아웃라인/화살표 표시
        //클릭하면 targetObject에 저장
    }

    void Skill_Player()
    {
        //보유한 스킬리스트 펼쳐짐 -> 
        SelectTarget();
    }

    void SelectRandomTarget()
    {
        int i = Random.Range(0, playerList.Count);
        targetObject = playerList[i];
    }


}
