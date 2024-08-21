using System;
using System.Collections.Generic;
using LevelEditor;
using ODProjects.PoolModule.Signals;
using UnityEngine;

public enum SO_Type
{
    GameSignals,
    PoolSignals,
    InventorySignals,
    PlayerSignals,
    DiceSignals,
    LevelDatas,
    SpriteData,
    DiceData,
    PlayerData,
    InventoryData
}

public static class SO_Manager
{
    private static readonly Dictionary<SO_Type, string> Paths = new Dictionary<SO_Type, string>
    {
        { SO_Type.GameSignals, "ScriptableObjects/Signal/GameSignals"},
        { SO_Type.PoolSignals, "ScriptableObjects/Signal/CD_PoolSignals"},
        { SO_Type.InventorySignals, "ScriptableObjects/Signal/InventorySignals"},
        { SO_Type.PlayerSignals, "ScriptableObjects/Signal/PlayerSignals"},
        { SO_Type.DiceSignals, "ScriptableObjects/Signal/DiceSignals"},
        { SO_Type.LevelDatas, "ScriptableObjects/Data/LevelDatas"},
        { SO_Type.SpriteData, "ScriptableObjects/Data/SpriteData"},
        { SO_Type.DiceData, "ScriptableObjects/Data/DiceData"},
        { SO_Type.PlayerData, "ScriptableObjects/Data/PlayerData"},
        { SO_Type.InventoryData, "ScriptableObjects/Data/InventoryData"}
    };

    private static readonly Dictionary<SO_Type, ScriptableObject> _cache = new Dictionary<SO_Type, ScriptableObject>();

    public static T Get<T>() where T : ScriptableObject
    {
        var type = GetScriptableObjectType<T>();
        return LoadScriptableObject<T>(type);
    }
    
    private static T LoadScriptableObject<T>(SO_Type type) where T : ScriptableObject
    {
        if (_cache.TryGetValue(type, out var value))
        {
            return value as T;
        }

        if (Paths.TryGetValue(type, out string path))
        {
            T scriptableObject = Resources.Load<T>(path);
            if (scriptableObject != null)
            {
                _cache[type] = scriptableObject;
                return scriptableObject;
            }
            else
            {
                Debug.LogError($"ScriptableObject of type {type} at path {path} could not be loaded.");
            }
        }
        else
        {
            Debug.LogError($"Path for ScriptableObject of type {type} not found.");
        }

        return null;
    }

    private static SO_Type GetScriptableObjectType<T>() where T : ScriptableObject
    {
        if (typeof(T) == typeof(GameSignals))
            return SO_Type.GameSignals;
        if (typeof(T) == typeof(PoolSignals))
            return SO_Type.PoolSignals;
        if (typeof(T) == typeof(InventorySignals))
            return SO_Type.InventorySignals;
        if (typeof(T) == typeof(PlayerSignals))
            return SO_Type.PlayerSignals;
        if (typeof(T) == typeof(DiceSignals))
            return SO_Type.DiceSignals;
        if (typeof(T) == typeof(LevelDatas))
            return SO_Type.LevelDatas;
        if (typeof(T) == typeof(SpriteData))
            return SO_Type.SpriteData;
        if (typeof(T) == typeof(DiceData))
            return SO_Type.DiceData;
        if (typeof(T) == typeof(PlayerData))
            return SO_Type.PlayerData;
        if (typeof(T) == typeof(InventoryData))
            return SO_Type.InventoryData;
        
        throw new ArgumentException($"Unsupported ScriptableObject type: {typeof(T)}");
    }
}