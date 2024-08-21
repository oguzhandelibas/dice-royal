using System;
using System.Collections.Generic;
using ODProjects.PoolModule.Enums;

namespace ODProjects.PoolModule.Extentions
{
    public class ObjectPoolExtention
    {
        private readonly Dictionary<EffectType, AbstractObjectPool> _pools;

        public ObjectPoolExtention()
        {
            _pools = new Dictionary<EffectType, AbstractObjectPool>();
        }

        public void AddObjectPool<T>(Func<T> factoryMethod, Action<T> turnOnCallback, Action<T> turnOffCallback, EffectType poolName, int initialStock = 0, bool isDynamic = true)
        {
            UnityEngine.Debug.Log($"{poolName} Added to Pool");
            if (!_pools.ContainsKey(poolName))
                _pools.Add(poolName, new ObjectPool<T>(factoryMethod, turnOnCallback, turnOffCallback, initialStock, isDynamic));
        }

        public ObjectPool<T> GetObjectPool<T>(EffectType poolName)
        {
            return (ObjectPool<T>)_pools[poolName];
        }

        public T GetObject<T>(EffectType poolName)
        {
            return ((ObjectPool<T>)_pools[poolName]).GetObject();
        }

        public void ReturnObject<T>(T o, EffectType poolName)
        {
            ((ObjectPool<T>)_pools[poolName]).ReturnObject(o);
        }
        public void RemovePool(EffectType poolName)
        {
            _pools[poolName] = null;
        }
    }
}