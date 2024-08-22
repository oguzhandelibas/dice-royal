using ODProjects;
using UnityEngine;

public class AudioManager : AbstractSingleton<AudioManager>
{
    [SerializeField] private AudioData audioData;
    [SerializeField] private AudioSource effectSource;
    
    public void PlayAudioEffect(AudioType audioType)
    {
        if (effectSource.isPlaying)
        {
            effectSource.Stop();
            effectSource.clip = null;
        }
        
        effectSource.clip = audioData.audioEffects[audioType];
        effectSource.Play();
    }
}
