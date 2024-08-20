using AYellowpaper.SerializedCollections;
using UnityEngine;

[CreateAssetMenu(fileName = "PlayerData", menuName = "ScriptableObjects/Data/PlayerData")]
public class PlayerData : ScriptableObject
{
    public SerializedDictionary<PlayerType, GameObject> playerPrefabs;
}