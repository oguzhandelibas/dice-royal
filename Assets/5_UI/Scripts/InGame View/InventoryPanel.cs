using System.Collections.Generic;
using LevelEditor;
using UnityEngine;

public class InventoryPanel : MonoBehaviour
{
    [SerializeField] private Transform inventoryParent;
    [SerializeField] private InventoryElementBehaviour inventoryElementBehaviourPrefab;
    
    private Dictionary<SelectedElement, InventoryElementBehaviour> _inventoryElements = new Dictionary<SelectedElement, InventoryElementBehaviour>();
    private InventorySignals _inventorySignals;
    
    private void Initialize()
    {
        Dictionary<SelectedElement,InventoryElement> inventoryElements = SO_Manager.Get<InventorySignals>().GetInventoryElements?.Invoke();
        foreach (var element in inventoryElements)
        {
            var inventoryElement = Instantiate(inventoryElementBehaviourPrefab, inventoryParent);
            inventoryElement.Initialize(element.Value.icon, element.Value.count);
            _inventoryElements.Add(element.Key, inventoryElement);
        }
    }
    
    private void UpdateElements(SelectedElement arg1, int arg2)
    {
        _inventoryElements[arg1].AddElementCount(arg2);
    }
    
    #region EVENT SUBSCRIPTION
    
    private void OnEnable()
    {
        // Event subscription
        SO_Manager.Get<GameSignals>().OnGameStart += Initialize;
        SO_Manager.Get<InventorySignals>().AddInventoryElement += UpdateElements;
    }

    private void OnDisable()
    {
        // Event unsubscription
        SO_Manager.Get<GameSignals>().OnGameStart -= Initialize;
        SO_Manager.Get<InventorySignals>().AddInventoryElement -= UpdateElements;
    }

    #endregion
}
