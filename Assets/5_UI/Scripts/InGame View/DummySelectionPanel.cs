using System;
using UnityEngine;
using UnityEngine.UI;

[Serializable]
public record DummyElement
{
    public Button button;
    public Image image;
    public Image childImage;
}

public class DummySelectionPanel : MonoBehaviour
{
    [SerializeField] private DummyElement[] playerTypeButtons;
    private PlayerType _currentPlayerType;

    private void Start()
    {
        _currentPlayerType = SO_Manager.Get<InventoryData>().playerType;
        int index = 0;
        foreach (var playerType in SO_Manager.Get<PlayerData>().playerTypes)
        {
            int capturedIndex = index;
            playerTypeButtons[index].image.color =
                _currentPlayerType == playerType.Key ? new Color(1, 0.89f, 0.49f) : Color.white;
            playerTypeButtons[index].childImage.sprite = playerType.Value.dummySprite;
            playerTypeButtons[capturedIndex].button.onClick.AddListener(() => { SetPlayerType(capturedIndex); });
            index++;
        }
    }

    private void SetPlayerType(int i)
    {
        PlayerType selectedPlayerType = (PlayerType)i;
        SO_Manager.Get<InventoryData>().playerType = selectedPlayerType;
        _ClosePanel();
    }

    public void _ClosePanel()
    {
        gameObject.SetActive(false);
        SO_Manager.Get<GameSignals>().OnGameStart?.Invoke();
    }
}