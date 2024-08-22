using System.Collections.Generic;
using UnityEngine;

public class PlayerManager : MonoBehaviour
{
    [SerializeField] private DiceManager diceManager;
    [SerializeField] private PlayerBehaviour playerPrefab;
    [SerializeField] private Transform playerParent;
    private bool _onMovement = false;
    private PlayerSignals _playerSignals;
    
    
    private void InitializePlayer(List<TileData> tileDatas)
    {
        PlayerBehaviour player = Instantiate(playerPrefab, Vector3.zero, Quaternion.identity, playerParent);
        player.Initialize(SO_Manager.Get<InventoryData>().playerType, tileDatas);
        _playerSignals.PlayerInitialized?.Invoke(player.transform);
        GameManager.Instance.isGameStarted = true;
    }

    private void MovePlayer(int moveCount)
    {
        if(_onMovement) return;
        _playerSignals.MoveTargetPosition?.Invoke(moveCount);
        _onMovement = true;
    }

    private void MovementCompleted(bool isCompleted) => _onMovement = !isCompleted;
    
    
    #region EVETN SUBSCRIPTION

    private void OnEnable()
    {
        _playerSignals = SO_Manager.Get<PlayerSignals>();
        _playerSignals.InitializeMovement += MovePlayer;
        _playerSignals.MovementComplete += MovementCompleted;
        _playerSignals.OnGameReadyToPlay += InitializePlayer;
    }

    private void OnDisable()
    {
        _playerSignals.InitializeMovement -= MovePlayer;
        _playerSignals.MovementComplete -= MovementCompleted;
        _playerSignals.OnGameReadyToPlay -= InitializePlayer;
    }

    #endregion
}
