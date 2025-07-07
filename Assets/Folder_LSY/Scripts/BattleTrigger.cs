using UnityEngine;

public class BattleTrigger : MonoBehaviour
{
    [SerializeField] private EScene battleScene;

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (!other.CompareTag("Player")) return;
        SceneLoader.LoadScene(battleScene);
    }
}