using System;
using AYellowpaper.SerializedCollections;
using UnityEngine;

[Serializable]
public record Dummy
{
    public GameObject dummyPrefab;
    public Sprite dummySprite;
}

[CreateAssetMenu(fileName = "PlayerData", menuName = "ScriptableObjects/Data/PlayerData")]
public class PlayerData : ScriptableObject
{
    [SerializedDictionary("Player Type", "Dummy Object")]public SerializedDictionary<PlayerType, Dummy> playerTypes;
}