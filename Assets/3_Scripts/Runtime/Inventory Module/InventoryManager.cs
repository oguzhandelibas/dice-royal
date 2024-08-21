using System.Collections;
using System.Collections.Generic;
using LevelEditor;
using UnityEngine;

public class InventoryManager : MonoBehaviour
{
    private InventorySignals _inventorySignals;
    private InventoryData _inventoryData;
    
    
    private Dictionary<SelectedElement, InventoryElement> GetElements()
    {
        return _inventoryData.inventoryElements;
    }

    private int GetElementCount(SelectedElement selectedElement)
    {
        return _inventoryData.inventoryElements[selectedElement].count;
    }

    private void AddElement(SelectedElement selectedElement, int count)
    {
        _inventoryData.inventoryElements[selectedElement].count += count;
    }
    
    private void OnEnable()
    {
        _inventorySignals = SO_Manager.Get<InventorySignals>();
        _inventoryData = SO_Manager.Get<InventoryData>();
        
        _inventorySignals.GetInventoryElements += GetElements;
        _inventorySignals.AddInventoryElement += AddElement;
        _inventorySignals.GetInventoryElementCount += GetElementCount;
    }

    private void OnDisable()
    {
        _inventorySignals.GetInventoryElements -= GetElements;
        _inventorySignals.AddInventoryElement -= AddElement;
        _inventorySignals.GetInventoryElementCount -= GetElementCount;
    }
}
