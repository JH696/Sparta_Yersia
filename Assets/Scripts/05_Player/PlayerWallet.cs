using UnityEngine;

[System.Serializable]
public class PlayerWallet
{
    public int yp;

    public int YP => yp;

    public PlayerWallet(int initialAmount = 0)
    {
        yp = Mathf.Max(0, initialAmount);
    }

    public void AddYP(int amount)
    {
        if (amount < 0) return;
        yp += amount;
    }

    public bool SpendYP(int amount)
    {
        if (amount < 0) return false;
        if (yp >= amount)
        {
            yp -= amount;
            return true;
        }
        return false;
    }

    public void SetYP(int amount)
    {
        yp = Mathf.Max(0, amount);
    }
}