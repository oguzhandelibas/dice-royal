using System;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public int testInt;
    private GameSignals _gameSignals;
    private void Start()
    {
        _gameSignals = SO_Manager.Get<GameSignals>();
        
        UIManager.Instance.Show<LoadingPanelView>();
        _gameSignals.OnGameInitialize?.Invoke();
    }

    private void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            //SO_Manager.Get<PlayerSignals>().MoveTargetPosition?.Invoke(testInt);
        }
    }
}