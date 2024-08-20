using System;
using UnityEngine;

[CreateAssetMenu(fileName = "PlayerSignals", menuName = "ScriptableObjects/Signals/PlayerSignals")]
public class PlayerSignals :ScriptableObject
{
    public Action<Transform> PlayerInitialized;
    public Action<Vector3[]> OnGameReadyToPlay;
    public Action InitializeMovement;
    public Action<int> MoveTargetPosition;
    public Action<bool> MovementComplete;
    
}