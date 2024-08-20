using System.Threading.Tasks;
using UnityEngine;

public class DiceManager : MonoBehaviour
{
    public int totalDiceValue;
    [SerializeField] private Transform diceParent;
    [SerializeField] private DiceBehaviour diceBehaviourPrefab;
    [SerializeField] private DiceData diceData;
    [SerializeField] private DiceRotationData diceRotationData;

    private DiceBehaviour[] _dices;
    public Vector3 rollForce = new Vector3(0, 5, 10);
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
        totalDiceValue = 0;
        int index = 0;
        foreach (var dice in diceData.diceValues)
        {
            _dices[index].gameObject.SetActive(true);
            _dices[index].transform.position = new Vector3(Random.Range(1.5f, 2.5f), Random.Range(0.5f, 1.5f), -2);
            _dices[index].Roll(rollForce, _rollTorque, diceRotationData.GetIndicatorRotation(dice));
            totalDiceValue += (int)(dice + 1);
            index++;
        }

        await Task.Delay(1500);

        SO_Manager.Get<PlayerSignals>().InitializeMovement?.Invoke();
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