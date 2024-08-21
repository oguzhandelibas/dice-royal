using System.Collections.Generic;
using LevelEditor;
using UnityEngine;

public class InventoryPanel : MonoBehaviour
{
    [SerializeField] private Transform inventoryParent;
    [SerializeField] private InventoryElementBehaviour inventoryElementBehaviourPrefab;
    
    private readonly Dictionary<SelectedElement, InventoryElementBehaviour> _inventoryElementBehaviours = new Dictionary<SelectedElement, InventoryElementBehaviour>();
    private InventorySignals _inventorySignals;
    
    private void Initialize()
    {
        Dictionary<SelectedElement, InventoryElement> inventoryElements = _inventorySignals.GetInventoryElements?.Invoke();
        foreach (var element in inventoryElements)
        {
            var inventoryElement = Instantiate(inventoryElementBehaviourPrefab, inventoryParent);
            inventoryElement.Initialize(element.Value.icon, element.Value.count);
            _inventoryElementBehaviours.Add(element.Key, inventoryElement);
        }
    }
    
    private void UpdateElements(SelectedElement arg1, int arg2)
    {
        _inventoryElementBehaviours[arg1].AddElementCount(arg2);
    }
    
    #region EVENT SUBSCRIPTION
    
    private void OnEnable()
    {
        SO_Manager.Get<GameSignals>().OnGameStart += Initialize;
        _inventorySignals = SO_Manager.Get<InventorySignals>();
        
        _inventorySignals.AddInventoryElement += UpdateElements;
    }

    private void OnDisable()
    {
        SO_Manager.Get<GameSignals>().OnGameStart -= Initialize;
        _inventorySignals.AddInventoryElement -= UpdateElements;
    }

    #endregion
}
