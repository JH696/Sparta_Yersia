using UnityEngine;
using System;

[Serializable]
public class DropItemData
{
    public ItemData itemData;
    [Range(0f, 1f)] public float dropChance = 0.5f;
    public int amount = 1;
}