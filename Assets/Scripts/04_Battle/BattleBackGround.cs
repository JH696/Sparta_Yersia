using UnityEngine;

public class BattleBackGround : MonoBehaviour
{
    [SerializeField] private SpriteRenderer sprRenderer;

    [SerializeField] private Sprite[] backGrounds = new Sprite[3];

    private void Start()
    {
        E_StageType nowStage = BattleManager.Instance.CurrentEncounter.Stage;

        switch (nowStage)
        {
            case E_StageType.Upper:
                sprRenderer.sprite = backGrounds[0];
                break;
            case E_StageType.Middle:
                sprRenderer.sprite = backGrounds[1];
                break;
            case E_StageType.Lower:
                sprRenderer.sprite = backGrounds[2];
                break;
            case E_StageType.Deep:
                sprRenderer.sprite = backGrounds[3];
                break;
            default:
                sprRenderer.sprite = backGrounds[0];
                break;
        }
    }
}
