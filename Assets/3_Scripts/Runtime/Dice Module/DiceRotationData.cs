using AYellowpaper.SerializedCollections;
using UnityEngine;

[CreateAssetMenu(fileName = "DiceRotationData", menuName = "ScriptableObjects/Data/DiceRotationData", order = 1)]
public class DiceRotationData : ScriptableObject
{
    [SerializedDictionary] public SerializedDictionary<DiceIndicatorType, Vector3> DiceIndicators;
    
    public Quaternion GetIndicatorRotation(DiceIndicatorType indicatorType)
    {
        return Quaternion.Euler(DiceIndicators[indicatorType]);
    }
}
