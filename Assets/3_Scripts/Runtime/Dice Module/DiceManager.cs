using System;
using System.Threading.Tasks;
using UnityEngine;
using Random = UnityEngine.Random;

public enum PlayerDirection
{
    Left,
    Right,
    Forward,
    Backward
}

public class DiceManager : MonoBehaviour
{
    public int totalDiceValue;
    [SerializeField] private Transform diceParent;
    [SerializeField] private DiceBehaviour diceBehaviourPrefab;
    [SerializeField] private DiceData diceData;
    [SerializeField] private DiceRotationData diceRotationData;

    private DiceBehaviour[] _dices;
    private Vector3 _rollForce = new Vector3(0, 4, 4);
    private readonly Vector3 _rollTorque = new Vector3(0.1f, 0.1f, 0.1f);

    private void Initialize()
    {
        foreach (Transform child in diceParent) Destroy(child.gameObject);
        _dices = new DiceBehaviour[diceData.diceValues.Length];
        for (int i = 0; i < _dices.Length; i++)
        {
            var dice = Instantiate(diceBehaviourPrefab, diceParent);
            _dices[i] = dice;
            dice.gameObject.SetActive(false);
        }
    }

    private async void RollDices()
    {
        Debug.Log("Rolling dices...");
        
        PlayerSignals playerSignals = SO_Manager.Get<PlayerSignals>();
        Transform playerTransform = (Transform) playerSignals.GetPlayerTransform?.Invoke();
        PlayerDirection playerDirection = (PlayerDirection) playerSignals.GetPlayerDirection?.Invoke();

        int diceXPosMultiplier = 1;
        switch (playerDirection)
        {
            case PlayerDirection.Left:
                diceXPosMultiplier = -1;
                _rollForce = new Vector3(2, 3, 0);
                break;
            case PlayerDirection.Right:
                diceXPosMultiplier = -1;
                _rollForce = new Vector3(2, 3, 0);
                break;
            case PlayerDirection.Forward:
                diceXPosMultiplier = 1;
                _rollForce = new Vector3(0, 4, 4);
                break;
            case PlayerDirection.Backward:
                diceXPosMultiplier = -1;
                _rollForce = new Vector3(2, 3, 0);
                break;
            default:
                throw new ArgumentOutOfRangeException();
        }
        
        totalDiceValue = 0;
        int index = 0;
        foreach (var dice in diceData.diceValues)
        {
            DiceBehaviour diceBehaviour = _dices[index];
            diceBehaviour.gameObject.SetActive(true);
            
            diceBehaviour.transform.position = playerTransform.position + new Vector3(diceXPosMultiplier*Random.Range(1.5f, 3.5f), Random.Range(0.5f, 1.5f), -3);
            diceBehaviour.Roll(_rollForce, _rollTorque, diceRotationData.GetIndicatorRotation(dice));
            
            totalDiceValue += (int)(dice + 1);
            index++;
            await Task.Delay(300);
        }

        await Task.Delay(1500);

        playerSignals.InitializeMovement?.Invoke();
    }

    #region EVENT SUBSCRIPTION

    private void OnEnable()
    {
        DiceSignals diceSignals = SO_Manager.Get<DiceSignals>();
        diceSignals.InitializeDices += Initialize;
        diceSignals.RollDices += RollDices;
    }

    private void OnDisable()
    {
        DiceSignals diceSignals = SO_Manager.Get<DiceSignals>();
        diceSignals.InitializeDices -= Initialize;
        diceSignals.RollDices -= RollDices;
    }

    #endregion
}