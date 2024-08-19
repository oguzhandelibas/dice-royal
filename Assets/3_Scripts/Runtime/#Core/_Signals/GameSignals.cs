using System;
using UnityEngine;

[CreateAssetMenu(fileName = "GameSignals", menuName = "ScriptableObjects/Signals/GameSignals")]
public class GameSignals : ScriptableObject
{
    public Action OnGameInitialize = delegate { };
    public Action OnGameStart = delegate { };
}
