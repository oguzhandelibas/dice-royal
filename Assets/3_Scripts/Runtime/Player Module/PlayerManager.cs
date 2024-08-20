using UnityEngine;

public class PlayerManager : MonoBehaviour
{
    [SerializeField] private DiceManager diceManager;
    [SerializeField] private PlayerType playerType;
    [SerializeField] private PlayerBehaviour playerPrefab;
    [SerializeField] private Transform playerParent;
    private bool onMovement = false;
    public void SetPlayerType(PlayerType type)
    {
        playerType = type;
    }
    
    private void InitializePlayer(Vector3[] positions)
    {
        PlayerBehaviour player = Instantiate(playerPrefab, playerParent);
        player.Initialize(playerType, positions);
    }

    private void MovePlayer()
    {
        if(onMovement) return;
        int moveCount = diceManager.totalDiceValue;
        SO_Manager.Get<PlayerSignals>().MoveTargetPosition?.Invoke(moveCount);
        onMovement = true;
    }

    private void MovementCompleted(bool isCompleted)
    {
        onMovement = !isCompleted;
    }
    
    
    #region EVETN SUBSCRIPTION

    private void OnEnable()
    {
        PlayerSignals playerSignals = SO_Manager.Get<PlayerSignals>();
        playerSignals.InitializeMovement += MovePlayer;
        playerSignals.MovementComplete += MovementCompleted;
        playerSignals.OnGameReadyToPlay += InitializePlayer;
    }

    private void OnDisable()
    {
        PlayerSignals playerSignals = SO_Manager.Get<PlayerSignals>();
        playerSignals.InitializeMovement -= MovePlayer;
        playerSignals.MovementComplete -= MovementCompleted;
        playerSignals.OnGameReadyToPlay -= InitializePlayer;
    }

    #endregion
}
