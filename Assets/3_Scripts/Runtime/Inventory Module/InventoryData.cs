using System;
using System.Collections.Generic;
using AYellowpaper.SerializedCollections;
using LevelEditor;
using UnityEngine;
using UnityEngine.Serialization;

[CreateAssetMenu(fileName = "InventoryData", menuName = "ScriptableObjects/Data/InventoryData")]
public class InventoryData : ScriptableObject
{
    [SerializedDictionary] public SerializedDictionary<SelectedElement, InventoryElement> inventoryElements;

    private void OnValidate()
    {
        if (inventoryElements.Count == 0)
        {
            SpriteData spriteData = SO_Manager.Get<SpriteData>();
            inventoryElements = new SerializedDictionary<SelectedElement, InventoryElement>();
            inventoryElements.Add(SelectedElement.Strawberry, new InventoryElement { icon = spriteData.GetSprite(SelectedElement.Strawberry), count = 0 });
            inventoryElements.Add(SelectedElement.Pear, new InventoryElement { icon = spriteData.GetSprite(SelectedElement.Pear), count = 0 });
            inventoryElements.Add(SelectedElement.Apple, new InventoryElement { icon = spriteData.GetSprite(SelectedElement.Apple), count = 0 });
        }
    }

    

    
}