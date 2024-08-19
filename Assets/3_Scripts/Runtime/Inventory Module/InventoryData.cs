using System;
using System.Collections.Generic;
using AYellowpaper.SerializedCollections;
using LevelEditor;
using UnityEngine;

[CreateAssetMenu(fileName = "InventoryData", menuName = "ScriptableObjects/Data/InventoryData")]
public class InventoryData : ScriptableObject
{
    [SerializedDictionary] public SerializedDictionary<SelectedElement, InventoryElement> _inventoryElements;

    private void OnValidate()
    {
        if (_inventoryElements.Count == 0)
        {
            SpriteData spriteData = SO_Manager.Get<SpriteData>();
            _inventoryElements = new SerializedDictionary<SelectedElement, InventoryElement>();
            _inventoryElements.Add(SelectedElement.Strawberry, new InventoryElement { icon = spriteData.GetSprite(SelectedElement.Strawberry), count = 0 });
            _inventoryElements.Add(SelectedElement.Pear, new InventoryElement { icon = spriteData.GetSprite(SelectedElement.Pear), count = 0 });
            _inventoryElements.Add(SelectedElement.Apple, new InventoryElement { icon = spriteData.GetSprite(SelectedElement.Apple), count = 0 });
        }
    }

    private Dictionary<SelectedElement, InventoryElement> GetElements()
    {
        return _inventoryElements;
    }

    private int GetElementCount(SelectedElement selectedElement)
    {
        return _inventoryElements[selectedElement].count;
    }

    private void AddElement(SelectedElement selectedElement, int count)
    {
        _inventoryElements[selectedElement].count += count;
    }

    private void OnEnable()
    {
        InventorySignals inventorySignals = SO_Manager.Get<InventorySignals>();
        inventorySignals.GetInventoryElements += GetElements;
        inventorySignals.AddInventoryElement += AddElement;
        inventorySignals.GetInventoryElementCount += GetElementCount;
    }

    private void OnDisable()
    {
        InventorySignals inventorySignals = SO_Manager.Get<InventorySignals>();
        inventorySignals.GetInventoryElements -= GetElements;
        inventorySignals.AddInventoryElement -= AddElement;
        inventorySignals.GetInventoryElementCount -= GetElementCount;
    }
}