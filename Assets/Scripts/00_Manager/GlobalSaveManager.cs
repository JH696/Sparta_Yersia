using UnityEngine;

public class GlobalSaveManager : MonoBehaviour
{
    public static PlayerSaveData playerSaveData;
    public static void Save(Player player)
    {
        playerSaveData = player.makeSaveData();
    }

    public static void Load(Player player)
    {
        if (playerSaveData == null)
        {
            Debug.LogError("PlayerSaveData is null. Cannot load data.");
            return;
        }
        player.LoadData(playerSaveData);
    }
}