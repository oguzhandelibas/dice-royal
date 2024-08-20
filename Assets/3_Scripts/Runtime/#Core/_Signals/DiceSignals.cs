using System;
using UnityEngine;

[CreateAssetMenu(fileName = "DiceSignals", menuName = "ScriptableObjects/Signals/DiceSignals")]
public class DiceSignals : ScriptableObject
{
    public Action InitializeDices;
    public Action RollDices;
}