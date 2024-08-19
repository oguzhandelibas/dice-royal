using UnityEngine;
using UnityEngine.Serialization;
using Random = System.Random;

[CreateAssetMenu(fileName = "DiceData", menuName = "ScriptableObjects/Data/DiceData", order = 1)]
public class DiceData : ScriptableObject
{
    public int DiceCount
    {
        get => diceValues.Length;
        set
        {
            diceValues = new DiceIndicatorType[value];
            for (int i = 0; i < diceValues.Length; i++)
            {
                diceValues[i] = (DiceIndicatorType) new Random().Next(0, 6);
            }
        }
    }

    public DiceIndicatorType[] diceValues;
    
}
