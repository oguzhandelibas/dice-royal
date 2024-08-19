using UnityEngine;
using Random = UnityEngine.Random;

[CreateAssetMenu(fileName = "DiceData", menuName = "ScriptableObjects/Data/DiceData", order = 1)]
public class DiceData : ScriptableObject
{
    public DiceIndicatorType[] diceValues;

    public void SetDiceValue(int index, int value)
    {
        diceValues[index] = (DiceIndicatorType) value;
    }
    
    public void CreateEmptyDiceValues(int count)
    {
        diceValues = new DiceIndicatorType[count];
        for (int i = 0; i < count; i++)
        {
            diceValues[i] = DiceIndicatorType.One;
        }
    }
    
    public void CreateRandomDiceValues(int count)
    {
        for (int i = 0; i < count; i++)
        {
            diceValues[i] = (DiceIndicatorType) Random.Range(0, 6);
        }
    }
}
