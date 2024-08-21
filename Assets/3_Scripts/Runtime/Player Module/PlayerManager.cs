using System.Collections.Generic;
using ODProjects;
using UnityEngine;

public class PlayerManager : MonoBehaviour
{
    [SerializeField] private DiceManager diceManager;
    [SerializeField] private PlayerType playerType;
    [SerializeField] private PlayerBehaviour playerPrefab;
    [SerializeField] private Transform playerParent;
    private bool _onMovement = false;
    private PlayerSignals _playerSignals;
    
    private void SetPlayerType(PlayerType type)
    {
        playerType = type;
    }
    
    private void InitializePlayer(List<TileData> tileDatas)
    {
        PlayerBehaviour player = Instantiate(playerPrefab, Vector3.zero, Quaternion.identity, playerParent);
        player.Initialize(playerType, tileDatas);
        _playerSignals.PlayerInitialized?.Invoke(player.transform);
        GameManager.Instance.isGameStarted = true;
    }

    private void MovePlayer()
    {
        if(_onMovement) return;
        int moveCount = diceManager.totalDiceValue;
        _playerSignals.MoveTargetPosition?.Invoke(moveCount);
        _onMovement = true;
    }

    private void MovementCompleted(bool isCompleted)
    {
        _onMovement = !isCompleted;
    }
    
    
    #region EVETN SUBSCRIPTION

    private void OnEnable()
    {
        _playerSignals = SO_Manager.Get<PlayerSignals>();
        _playerSignals.SetPlayerType += SetPlayerType;
        _playerSignals.InitializeMovement += MovePlayer;
        _playerSignals.MovementComplete += MovementCompleted;
        _playerSignals.OnGameReadyToPlay += InitializePlayer;
    }

    private void OnDisable()
    {
        _playerSignals.SetPlayerType -= SetPlayerType;
        _playerSignals.InitializeMovement -= MovePlayer;
        _playerSignals.MovementComplete -= MovementCompleted;
        _playerSignals.OnGameReadyToPlay -= InitializePlayer;
    }

    #endregion
}
