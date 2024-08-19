using UnityEngine;

public class DiceManager : MonoBehaviour
{
    [SerializeField] private DiceBehaviour diceBehaviourPrefab;
    [SerializeField] private DiceData diceData;
    [SerializeField] private DiceRotationData diceRotationData;
    
    private DiceBehaviour[] _dices;
    public Vector3 rollForce = new Vector3(0, 5, 10);
    private readonly Vector3 _rollTorque = new Vector3(0.1f, 0.1f, 0.1f);

    private void Initialize()
    {
        foreach (var t in diceData.diceValues)
        {
            var dice = Instantiate(diceBehaviourPrefab, transform);
            dice.transform.position = new Vector3(Random.Range(1.5f,2.5f), Random.Range(0.5f,1.5f), -2);
            dice.Roll(rollForce, _rollTorque, diceRotationData.GetIndicatorRotation(t));
        }
    }

    #region EVENT SUBSCRIPTION
    
    private void OnEnable()
    {
        // Event subscription
        SO_Manager.Get<GameSignals>().OnGameStart += Initialize;
    }
    
    private void OnDisable()
    {
        // Event unsubscription
        SO_Manager.Get<GameSignals>().OnGameStart -= Initialize;
    }

    #endregion
}