using Cinemachine;
using UnityEngine;

public class VCamController : MonoBehaviour
{
    [Tooltip("플레이어가 영역에 들어올 때 우선순위")]
    public int activePriority = 20;
    [Tooltip("기본 우선순위")]
    public int defaultPriority = 5;

    private CinemachineVirtualCamera _vcam;

    void Awake()
    {
        _vcam = GetComponent<CinemachineVirtualCamera>();
        _vcam.Priority = defaultPriority;
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
            _vcam.Priority = activePriority;
    }

    void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
            _vcam.Priority = defaultPriority;
    }
}
