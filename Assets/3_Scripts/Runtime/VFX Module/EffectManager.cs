using System.Threading.Tasks;
using ODProjects;
using ODProjects.PoolModule.Signals;
using UnityEngine;

public class EffectManager : AbstractSingleton<EffectManager>
{
    public GameObject GetEffect(EffectType effectType)
    {
        GameObject effect = SO_Manager.Get<PoolSignals>().OnGetObjectFromPool?.Invoke(effectType);
        ReleaseEffect(effect, effectType);
        return effect;
    }

    private async void ReleaseEffect(GameObject obj, EffectType effectType)
    {
        await Task.Delay(1000);
        SO_Manager.Get<PoolSignals>().OnReleaseObjectFromPool?.Invoke(obj, effectType);
    }
}
