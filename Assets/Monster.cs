using UnityEngine;

public class Monster : MonoBehaviour
{
    [Header("몬스터 상태")]
    [SerializeField] private MonsterStatus status;

    [Header("몬스터 데이터")]
    [SerializeField] private MonsterData monsterData;

    public MonsterStatus Status => status; // 읽기 전용

    private void Start()
    {
        if (status == null)
        {
            status = new MonsterStatus(monsterData); 
        }
    }
}
