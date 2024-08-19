using UnityEngine;

public class GameManager : MonoBehaviour
{
    private GameSignals _gameSignals;
    private void Start()
    {
        _gameSignals = SO_Manager.Get<GameSignals>();
        
        UIManager.Instance.Show<LoadingPanelView>();
        _gameSignals.OnGameInitialize?.Invoke();
    }
}