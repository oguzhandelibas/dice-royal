using System;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    private GameSignals _gameSignals;
    private void Start()
    {
        _gameSignals = SO_Manager.Get<GameSignals>();
        _gameSignals.OnGameStart?.Invoke();
    }
}