using UnityEngine;
using UnityEngine.Events;
using System;
using ODProjects.PoolModule.Enums;

namespace ODProjects.PoolModule.Signals
{
    [CreateAssetMenu(fileName = "CD_PoolSignals", menuName = "ObjectPooling/CD_PoolSignals", order = 0)]
    public class PoolSignals : ScriptableObject
    {
        public Func<EffectType, GameObject> OnGetObjectFromPool = delegate { return null; };
        public UnityAction<GameObject, EffectType> OnReleaseObjectFromPool = delegate { };
    }
}
