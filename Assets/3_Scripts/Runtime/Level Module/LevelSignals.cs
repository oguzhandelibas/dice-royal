using System;
using UnityEngine;
using UnityEngine.Events;

namespace LevelEditor
{
    [CreateAssetMenu(menuName = "ScriptableObjects/Signals/LevelSignals", fileName = "SD_LevelSignals", order = 0)]
    public class LevelSignals : ScriptableObject
    {
        public UnityAction OnLevelInitialize = delegate { };
        public UnityAction OnLevelSuccessful = delegate { };
        public UnityAction OnLevelFailed = delegate { };
        public UnityAction OnNextLevel = delegate { };
        public UnityAction OnRestartLevel = delegate { };
        public UnityAction OnLevelFinished = delegate { };
        public UnityAction OnDecreaseMoveCount = delegate { };
        
        public Func<int> OnGetLevelCount = delegate { return 0; };
    }
}
