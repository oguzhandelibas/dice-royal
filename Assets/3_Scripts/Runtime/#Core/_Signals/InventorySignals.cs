using System;
using System.Collections;
using System.Collections.Generic;
using AYellowpaper.SerializedCollections;
using LevelEditor;
using UnityEngine;

[CreateAssetMenu(fileName = "InventorySignals", menuName = "ScriptableObjects/Signals/InventorySignals")]
public class InventorySignals : ScriptableObject
{
    public Func<Dictionary<SelectedElement, InventoryElement>> GetInventoryElements;
    public Action<SelectedElement, int> AddInventoryElement;
    public Func<SelectedElement, int> GetInventoryElementCount;
    
}
