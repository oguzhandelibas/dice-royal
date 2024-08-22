using System;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "PlayerSignals", menuName = "ScriptableObjects/Signals/PlayerSignals")]
public class PlayerSignals :ScriptableObject
{
    public Action<Transform> PlayerInitialized;
    public Func<Transform> GetPlayerTransform;
    public Func<PlayerDirection> GetPlayerDirection;
    public Action<List<TileData>> OnGameReadyToPlay;
    public Action<int> InitializeMovement;
    public Action<int> MoveTargetPosition;
    public Action<bool> MovementComplete;

}