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
        AudioManager.Instance.PlayAudioEffect(AudioType.ButtonClick);
        SO_Manager.Get<DiceSignals>().RollDices?.Invoke();
        goButton.interactable = false;
    }

    private void ActivateGoButton(bool b)
    {
        goButton.interactable = true;
    }
    
    private void DeactivateGoButton(int i)
    {
        goButton.interactable = false;
    }

    #region EVENT SUBSCRIPTION

    private void OnEnable()
    {
        //SO_Manager.Get<PlayerSignals>().MoveTargetPosition += DeactivateGoButton;
        SO_Manager.Get<PlayerSignals>().MovementComplete += ActivateGoButton;
    }

    private void OnDisable()
    {
        //SO_Manager.Get<PlayerSignals>().MoveTargetPosition -= DeactivateGoButton;
        SO_Manager.Get<PlayerSignals>().MovementComplete -= ActivateGoButton;
    }

    #endregion
}