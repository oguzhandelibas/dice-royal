using System;
using UnityEngine;

[CreateAssetMenu(fileName = "CameraSignals", menuName = "ScriptableObjects/Signals/CameraSignals")]
public class CameraSignals : ScriptableObject
{
    public Action ShakeCamera = delegate { };
}