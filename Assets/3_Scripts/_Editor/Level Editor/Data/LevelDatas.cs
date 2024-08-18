using AYellowpaper.SerializedCollections;
using LevelEditor;
using UnityEngine;

[CreateAssetMenu (fileName = "LevelDatas", menuName = "ScriptableObjects/Data/LevelDatas", order = 1)]
public class LevelDatas : ScriptableObject
{
    [SerializedDictionary("Element Type", "Description")] [SerializeField] private SerializedDictionary<LevelType, LevelData> LevelData;
    public LevelData GetLevelData(LevelType levelType) => LevelData[levelType];
}