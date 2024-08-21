using ODProjects;
using UnityEngine;
using UnityEngine.UI;

public class AudioManager : AbstractSingleton<AudioManager>
{
    [SerializeField] private AudioData audioData;
    
    [SerializeField] private AudioSource musicSource;
    [SerializeField] private AudioSource effectSource;
    [SerializeField] private Image musicButtonImage;
    [SerializeField] private Image soundButtonImage;
    
    
    
    private bool _isSoundActive = true;
    private bool _isMusicActive = false;

    protected override void Awake()
    {
        base.Awake();
 
        _isSoundActive = false;
        _isMusicActive = true;
        _SetMusicActiveness();
        _SetSoundActiveness();
    } 
    
    public void _SetMusicActiveness()
    {
        _isMusicActive = !_isMusicActive;
        if(musicButtonImage == null) return;
        musicButtonImage.color = _isMusicActive ? new Color(0.2941177f, 0.3764706f, 0.4745098f) : new Color(0.70f, 0.70f, 0.70f);
        musicSource.volume = _isMusicActive ? 0.05f : 0.0f;
    }

    public void _SetSoundActiveness()
    {
        _isSoundActive = !_isSoundActive;
        if(soundButtonImage == null) return;
        soundButtonImage.color = _isSoundActive ? new Color(0.2941177f, 0.3764706f, 0.4745098f) : new Color(0.70f, 0.70f, 0.70f);
        effectSource.volume = _isSoundActive ? 0.5f : 0.0f;
    }

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
