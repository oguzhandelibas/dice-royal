using System;
using UnityEngine;

[Serializable]
public record InventoryElement
{
    public Sprite icon;
    public int count;
}