using System;
using System.Collections.Generic;
using LevelEditor;
using UnityEngine;

public enum SO_Type
{
    GameSignals,
    InventorySignals,
    LevelDatas,
    SpriteData,
    DiceData
}

public static class SO_Manager
{
    private static readonly Dictionary<SO_Type, string> Paths = new Dictionary<SO_Type, string>
    {
        { SO_Type.GameSignals, "ScriptableObjects/Signal/GameSignals"},
        { SO_Type.InventorySignals, "ScriptableObjects/Signal/InventorySignals"},
        { SO_Type.LevelDatas, "ScriptableObjects/Data/LevelDatas"},
        { SO_Type.SpriteData, "ScriptableObjects/Data/SpriteData"},
        { SO_Type.DiceData, "ScriptableObjects/Data/DiceData"}
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
        if (typeof(T) == typeof(InventorySignals))
            return SO_Type.InventorySignals;
        if (typeof(T) == typeof(LevelDatas))
            return SO_Type.LevelDatas;
        if (typeof(T) == typeof(SpriteData))
            return SO_Type.SpriteData;
        if (typeof(T) == typeof(DiceData))
            return SO_Type.DiceData;
        
        throw new ArgumentException($"Unsupported ScriptableObject type: {typeof(T)}");
    }
}