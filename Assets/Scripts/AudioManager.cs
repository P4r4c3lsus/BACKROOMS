
using System;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class Sound
{
    public string name;
    public AudioClip clip;

    [Range(0f, 1f)]
    public float volume = 1f;
}

public class AudioManager : MonoBehaviour
{
    public static AudioManager Instance { get; private set; }

    [Header("Lista de sonidos")]
    [SerializeField] private List<Sound> sounds = new List<Sound>();

    [Header("Audio Sources")]
    [SerializeField] private AudioSource musicSource;
    [SerializeField] private AudioSource sfxSource;

    private Dictionary<string, Sound> soundDictionary;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
        DontDestroyOnLoad(gameObject);

        if (musicSource == null)
            musicSource = gameObject.AddComponent<AudioSource>();

        if (sfxSource == null)
            sfxSource = gameObject.AddComponent<AudioSource>();

        musicSource.playOnAwake = false;
        sfxSource.playOnAwake = false;

        musicSource.loop = true;
        sfxSource.loop = false;

        soundDictionary = new Dictionary<string, Sound>();

        foreach (Sound sound in sounds)
        {
            if (sound.clip == null || string.IsNullOrEmpty(sound.name))
                continue;

            if (!soundDictionary.ContainsKey(sound.name))
                soundDictionary.Add(sound.name, sound);
        }
    }

    public void PlaySFX(string soundName)
    {
        if (soundDictionary.TryGetValue(soundName, out Sound sound))
        {
            sfxSource.PlayOneShot(sound.clip, sound.volume);
        }
        else
        {
            Debug.LogWarning("Sonido no encontrado: " + soundName);
        }
    }

    public void PlayMusic(string soundName)
    {
        if (soundDictionary.TryGetValue(soundName, out Sound sound))
        {
            if (musicSource.clip == sound.clip && musicSource.isPlaying)
                return;

            musicSource.clip = sound.clip;
            musicSource.volume = sound.volume;
            musicSource.Play();
        }
        else
        {
            Debug.LogWarning("Música no encontrada: " + soundName);
        }
    }

    public void StopMusic()
    {
        musicSource.Stop();
    }

    public void PauseMusic()
    {
        musicSource.Pause();
    }

    public void ResumeMusic()
    {
        musicSource.UnPause();
    }

    public void SetMusicVolume(float volume)
    {
        musicSource.volume = Mathf.Clamp01(volume);
    }

    public void SetSFXVolume(float volume)
    {
        sfxSource.volume = Mathf.Clamp01(volume);
    }
}
