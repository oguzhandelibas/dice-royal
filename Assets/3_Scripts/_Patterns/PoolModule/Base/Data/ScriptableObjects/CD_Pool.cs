using UnityEngine;
using AYellowpaper.SerializedCollections;
using ODProjects.PoolModule.Enums;

namespace ODProjects.PoolModule.Data.ScriptableObjects
{
    [CreateAssetMenu(fileName = "CD_Pool", menuName = "ObjectPooling/CD_Pool", order = 0)]
    public class CD_Pool : ScriptableObject
    {
        [SerializeField] public SerializedDictionary<EffectType, PoolData> PoolDataDictionary = new SerializedDictionary<EffectType, PoolData>();
        public bool HasThisType(EffectType type) => PoolDataDictionary.ContainsKey(type);
    }
}
