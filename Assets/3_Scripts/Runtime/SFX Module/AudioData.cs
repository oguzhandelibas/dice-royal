using AYellowpaper.SerializedCollections;
using UnityEngine;

[CreateAssetMenu(fileName = "AudioData", menuName = "ScriptableObjects/Data/AudioData", order = 1)]
public class AudioData : ScriptableObject
{
    [SerializedDictionary("Audio Type", "Audio Clip")]
    public SerializedDictionary<AudioType, AudioClip> audioEffects;
}