using UnityEngine;
using System.Collections.Generic;

public class AudioManager : MonoBehaviour
{
    public static AudioManager Instance { get; private set; }
    
    [Header("Volume Settings")]
    [SerializeField] private float musicVolume = 0.3f;
    [SerializeField] private float sfxVolume = 0.5f;
    [SerializeField] private float npcVolume = 0.4f;
    [SerializeField] private float environmentVolume = 0.35f;
    
    [Header("Music")]
    [SerializeField] private string backgroundMusicPath = "Audio/Music/background_music";
    
    private AudioSource musicSource;
    private AudioSource sfxSource;
    private Dictionary<string, AudioClip> cachedClips = new Dictionary<string, AudioClip>();
    
    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
            InitializeAudioSources();
            InitializeGameSystems();
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void InitializeGameSystems()
    {
        if (GameManager.Instance == null)
        {
            GameObject gameManagerObj = new GameObject("GameManager");
            gameManagerObj.AddComponent<GameManager>();
        }
    }
    
    private void Start()
    {
        PlayBackgroundMusic();
    }
    
    private void InitializeAudioSources()
    {
        musicSource = gameObject.AddComponent<AudioSource>();
        musicSource.loop = true;
        musicSource.volume = musicVolume;
        musicSource.playOnAwake = false;
        
        sfxSource = gameObject.AddComponent<AudioSource>();
        sfxSource.loop = false;
        sfxSource.volume = sfxVolume;
        sfxSource.playOnAwake = false;
    }
    
    public void PlayBackgroundMusic()
    {
        AudioClip clip = LoadClip(backgroundMusicPath);
        if (clip != null)
        {
            musicSource.clip = clip;
            musicSource.volume = musicVolume;
            musicSource.Play();
        }
    }
    
    public void StopBackgroundMusic()
    {
        musicSource.Stop();
    }
    
    public void PlaySFX(string clipPath)
    {
        AudioClip clip = LoadClip(clipPath);
        if (clip != null)
        {
            sfxSource.PlayOneShot(clip, sfxVolume);
        }
    }
    
    public void PlayAttackSound()
    {
        PlaySFX("Audio/SFX/attack_sword");
    }
    
    public void PlayDigSound()
    {
        PlaySFX("Audio/SFX/attack_sword");
    }
    
    public void PlayAnimalSound(string animalType)
    {
        string path = "Audio/SFX/" + animalType.ToLower();
        AudioClip clip = LoadClip(path);
        if (clip != null)
        {
            sfxSource.PlayOneShot(clip, npcVolume);
        }
    }
    
    public void PlayMonsterSound()
    {
        AudioClip clip = LoadClip("Audio/SFX/attack_sword");
        if (clip != null)
        {
            sfxSource.PlayOneShot(clip, npcVolume);
        }
    }
    
    public void PlayHarvestSound()
    {
        AudioClip clip = LoadClip("Audio/SFX/chicken");
        if (clip != null)
        {
            sfxSource.PlayOneShot(clip, environmentVolume);
        }
    }
    
    public void PlayHurtSound()
    {
        AudioClip clip = LoadClip("Audio/SFX/attack_sword");
        if (clip != null)
        {
            sfxSource.PlayOneShot(clip, sfxVolume);
        }
    }
    
    private AudioClip LoadClip(string path)
    {
        if (cachedClips.ContainsKey(path))
        {
            return cachedClips[path];
        }
        
        AudioClip clip = Resources.Load<AudioClip>(path);
        if (clip != null)
        {
            cachedClips[path] = clip;
        }
        return clip;
    }
    
    public void SetMusicVolume(float volume)
    {
        musicVolume = Mathf.Clamp01(volume);
        if (musicSource != null)
        {
            musicSource.volume = musicVolume;
        }
    }
    
    public void SetSFXVolume(float volume)
    {
        sfxVolume = Mathf.Clamp01(volume);
    }
}
