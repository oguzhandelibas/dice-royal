using UnityEngine;
using UnityEngine.UI;

public class InGamePanelView : View
{
    [SerializeField] private Button goButton;
    
    private void Start()
    {
        AudioManager.Instance.PlayAudioEffect(AudioType.CloudEffect);
        goButton.onClick.AddListener(OnGoButtonClick);
    }
    private void OnGoButtonClick()
    {
        if (!GameManager.Instance.isGameStarted) return;
        AudioManager.Instance.PlayAudioEffect(AudioType.ButtonClick);
        SO_Manager.Get<DiceSignals>().RollDices?.Invoke();
        goButton.interactable = false;
    }

    private void ActivateGoButton(bool b)
    {
        goButton.interactable = true;
    }

    #region EVENT SUBSCRIPTION

    private PlayerSignals _playerSignals;
    private void OnEnable()
    {
        _playerSignals = SO_Manager.Get<PlayerSignals>();
        _playerSignals.MovementComplete += ActivateGoButton;
    }

    private void OnDisable()
    {
        _playerSignals.MovementComplete -= ActivateGoButton;
    }

    #endregion
}