using UnityEngine;
using UnityEngine.UI;

public class DummySelectionPanel : MonoBehaviour
{
    [SerializeField] private Button[] playerTypeButtons;
    private void Start()
    {
        int index = 0;
        foreach (var playerType in SO_Manager.Get<PlayerData>().playerTypes)
        {
            int capturedIndex = index;
            playerTypeButtons[index].transform.GetChild(0).GetComponent<Image>().sprite = playerType.Value.dummySprite;
            playerTypeButtons[capturedIndex].onClick.AddListener(() =>
            {
                SetPlayerType(capturedIndex);
            });
            index++;
        }
    }

    private void SetPlayerType(int i)
    {
        gameObject.SetActive(false);
        SO_Manager.Get<PlayerSignals>().SetPlayerType?.Invoke((PlayerType)i);
        SO_Manager.Get<GameSignals>().OnGameStart?.Invoke();
    }
}
