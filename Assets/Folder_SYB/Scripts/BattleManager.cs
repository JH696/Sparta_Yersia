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
    [SerializeField] int maxGauge = 100;
    GameObject player;
    GameObject enemy;
    CharacterStatData characterStatData;
    bool isEnemy = false;
    //행동에 당하는 오브젝트
    GameObject targetObject;
    BaseCharacter baseCharacter;
    //전체 리스트
    List<GameObject> allCharacter = new List<GameObject>();
    //돌아다니는 플레이어 리스트
    List<GameObject> playerList = new List<GameObject>();
    //만난 에너미 리스트
    List<GameObject> enemyList = new List<GameObject>();
    //행동력 다차서 행동할 오브젝트 리스트
    List<GameObject> playObjectList = new List<GameObject>();
    private void Awake()
    {
        allCharacter.Clear();
        playerList.Clear();
        enemyList.Clear();
        playObjectList.Clear();
        //플레이어리스트에 플레이어 넣고 에너미에 에너미 넣고
        playerList.Add(player);
        enemyList.Add(enemy);
        //전체엔 둘다 넣고
        allCharacter.AddRange(playerList);
        allCharacter.AddRange(enemyList);
        foreach (var character in allCharacter)
        {
            //겟컴포넌트 받기

        }

    }
    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(BattleStart());
    }

    // Update is called once per frame
    void Update()
    {

    }

    IEnumerator BattleStart()
    {
        //플레이어 리스트에 있는것들 생성 -> 위치잡고 생성? UI로?
        //에너미 리스트에 있는것들 생성
        //Battle Start! text
        yield return new WaitForSeconds(2f);
        state = EBattleState.Standby;
        while (state == EBattleState.Standby)
        {
            //행동력 증가(시간* 개체별 행동력수치)
            for (int i = 0; i < allCharacter.Count; i++)
            {
                //if (allCharacter[i].isDie) continue;
                //allCharacter[i].playGauge += allCharacter[i].Speed * time.deltatime;

                //if (allCharacter[i].playGauge >= maxGauge)
                //{
                //    //행동력 다차면 playObject에 저장
                //    playObjectList.Add(allCharacter[i]);
                //}
            }
            //행동력 다찬 애들 존재하면
            //반복문 돌려서 인덱스 첫번째 컴페어 태그해서 에너미면 에너미 어택
            if (playObjectList == null)
                continue;
            for (int i = 0;i < playObjectList.Count; i++)
            {
                if (playObjectList[i].CompareTag("enemy"))
                {
                    state = EBattleState.Enemy;
                    //enemy attack
                }
                else
                {
                    state = EBattleState.Player;
                    //player attack
                }

            }

        }

    }

    IEnumerator StandbyPhase()
    {
        
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
        SelectRandomTarget(playerList);
        //플레이어 중 랜덤 선택 -> targetObject에 저장
        //공격 -> state = EBattleState.Result
        yield break;
    }
    IEnumerator ResultPhase()
    {
        //데미지 정산 
        //targetObject가 죽었는지 확인 isDie
        //리스트가 다죽어있다면 -> isDie 제외 카운트세고 0이면?
        //플레이어일때 lose()
        //에너미면 win()
        yield break;
    }

    void Attack_Player()
    {
        //임시 랜덤공격
        SelectRandomTarget(enemyList);
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

    void SelectRandomTarget(List<GameObject> target)
    {
        int i = Random.Range(0, target.Count);
        targetObject = target[i];
    }


}
