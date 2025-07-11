using System.Collections.Generic;
using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class B_Manager : MonoBehaviour
{
    public static B_Manager Instance;

    [SerializeField] private EScene BattleScene;

    [SerializeField] private B_Characters chars;

    public event System.Action InBattle;

    private IEnumerator Battle(List<GameObject> monsters)
    {
        AsyncOperation asyncOperation = SceneManager.LoadSceneAsync("BattleScene");

        while (!asyncOperation.isDone) yield return null;
        
        StartBattle(monsters);
    }

    public void EnterBattle(List<GameObject> monsters)
    {
        //SceneLoader.LoadScene(BattleScene);
        StartCoroutine(Battle(monsters));
    }

    private void Awake()
    {
        if (Instance != null)
        {
            Destroy(gameObject);
            return;
        }
        else
        {
            Instance = this;
        }
    }

    public void SetCharacters(B_Characters chars)
    {
        this.chars = chars;
    }


    private void StartBattle(List<GameObject> monsters)
    {
        chars.SetAllySlots();
        chars.SetEnemySlots(monsters);

        InBattle?.Invoke();
    }
}
