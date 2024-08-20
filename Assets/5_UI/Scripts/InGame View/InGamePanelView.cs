using UnityEngine;

public class InGamePanelView : View
{
    public void _GoButton()
    {
        SO_Manager.Get<DiceSignals>().RollDices?.Invoke();
    }
}