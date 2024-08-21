using ODProjects;
using UnityEngine;

public class EffectManager : AbstractSingleton<EffectManager>
{
    [SerializeField] private EffectData effectData;
    
    public GameObject GetEffect(EffectType effectType)
    {
        return effectData.effectPrefabs[effectType];
    }
}
