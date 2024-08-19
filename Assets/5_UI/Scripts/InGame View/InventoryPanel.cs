using LevelEditor;
using UnityEngine;

public class InventoryPanel : MonoBehaviour
{
    [SerializeField] private Transform inventoryParent;
    [SerializeField] private InventoryElement inventoryElementPrefab;

    private void Initialize()
    {
        SpriteData spriteData = SO_Manager.Get<SpriteData>();

        foreach (var sprite in spriteData.Sprites)
        {
            if(sprite.Value == null) continue;
            var element = Instantiate(inventoryElementPrefab, inventoryParent);
            element.Initialize(sprite.Value, 3);
        }
    }
    
    #region EVENT SUBSCRIPTION
    
    private void OnEnable()
    {
        // Event subscription
        SO_Manager.Get<GameSignals>().OnGameStart += Initialize;
    }
    
    private void OnDisable()
    {
        // Event unsubscription
        SO_Manager.Get<GameSignals>().OnGameStart -= Initialize;
    }

    #endregion
}
