using AYellowpaper.SerializedCollections;
using UnityEngine;

[CreateAssetMenu(fileName = "EffectData", menuName = "ScriptableObjects/Data/EffectData", order = 1)]
public class EffectData : ScriptableObject
{
    [SerializedDictionary("Effect Type", "Effect Prefab")]
    public SerializedDictionary<EffectType, GameObject> effectPrefabs;
}