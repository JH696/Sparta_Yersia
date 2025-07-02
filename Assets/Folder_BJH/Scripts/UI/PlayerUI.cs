using UnityEngine;

public class PlayerUI : MonoBehaviour
{
    public void ShowPlayerUI()
    {
        this.gameObject.SetActive(true);
    }

    public void HidePlayerUI()
    {
        this.gameObject.SetActive(false);
    }   
}
