using System;
using ODProjects;
using UnityEngine;
using UnityEngine.Serialization;

public class GameManager : AbstractSingleton<GameManager>
{
    public bool isGameStarted = false;
    private GameSignals _gameSignals;
    private void Start()
    {
        _gameSignals = SO_Manager.Get<GameSignals>();
        
        UIManager.Instance.Show<LoadingPanelView>();
        _gameSignals.OnGameInitialize?.Invoke();
    }
}