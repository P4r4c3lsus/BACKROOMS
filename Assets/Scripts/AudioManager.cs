using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

[Serializable]
public class Sound
{
    public string name;
    public AudioClip clip;

    [Range(0f, 1f)]
    public float volume = 1f;
}

[Serializable]
public class SceneMusic
{
    public string sceneName;
    public string musicName;
}

public class AudioManager : MonoBehaviour
{
    public static AudioManager Instance { get; private set; }

    [Header("Lista de sonidos")]
    [SerializeField] private List<Sound> sounds = new();

    [Header("Música por escena")]
    [SerializeField] private List<SceneMusic> sceneMusic = new();

    [Header("Audio Sources")]
    [SerializeField] private AudioSource musicSource;
    [SerializeField] private AudioSource sfxSource;

    [Header("Transiciones")]
    [SerializeField] private float fadeDuration = 1.5f;

    private Dictionary<string, Sound> soundDictionary;

    private Coroutine fadeCoroutine;

    private float musicVolume = 1f;
    private float sfxVolume = 1f;

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
        musicSource.loop = true;

        sfxSource.playOnAwake = false;
        sfxSource.loop = false;

        musicSource.spatialBlend = 0f;
        sfxSource.spatialBlend = 0f;

        soundDictionary = new Dictionary<string, Sound>();

        foreach (Sound sound in sounds)
        {
            if (sound.clip == null || string.IsNullOrEmpty(sound.name))
                continue;

            if (!soundDictionary.ContainsKey(sound.name))
                soundDictionary.Add(sound.name, sound);
        }
    }

    private void Start()
    {
        PlaySceneMusic(SceneManager.GetActiveScene().name);
    }

    private void OnEnable()
    {
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    private void OnDisable()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        PlaySceneMusic(scene.name);
    }

    private void PlaySceneMusic(string sceneName)
    {
        foreach (SceneMusic music in sceneMusic)
        {
            if (music.sceneName == sceneName)
            {
                PlayMusic(music.musicName);
                return;
            }
        }
    }

    public void PlayMusic(string soundName)
    {
        if (!soundDictionary.TryGetValue(soundName, out Sound sound))
        {
            Debug.LogWarning("Música no encontrada: " + soundName);
            return;
        }

        if (musicSource.clip == sound.clip && musicSource.isPlaying)
            return;

        if (fadeCoroutine != null)
            StopCoroutine(fadeCoroutine);

        fadeCoroutine = StartCoroutine(FadeMusic(sound));
    }

    private IEnumerator FadeMusic(Sound newSound)
    {
        float duration = Mathf.Max(0f, fadeDuration);

        float startVolume = musicSource.volume;

        if (musicSource.isPlaying && duration > 0f)
        {
            float elapsed = 0f;

            while (elapsed < duration)
            {
                elapsed += Time.unscaledDeltaTime;

                musicSource.volume = Mathf.Lerp(
                    startVolume,
                    0f,
                    Mathf.Clamp01(elapsed / duration)
                );

                yield return null;
            }
        }

        musicSource.Stop();

        musicSource.clip = newSound.clip;
        musicSource.volume = 0f;
        musicSource.Play();

        float targetVolume = newSound.volume * musicVolume;

        if (duration > 0f)
        {
            float elapsed = 0f;

            while (elapsed < duration)
            {
                elapsed += Time.unscaledDeltaTime;

                musicSource.volume = Mathf.Lerp(
                    0f,
                    newSound.volume * musicVolume,
                    Mathf.Clamp01(elapsed / duration)
                );

                yield return null;
            }
        }

        musicSource.volume = targetVolume;

        fadeCoroutine = null;
    }

    public void PlaySFX(string soundName)
    {
        if (soundDictionary.TryGetValue(soundName, out Sound sound))
        {
            sfxSource.PlayOneShot(
                sound.clip,
                sound.volume * sfxVolume
            );
        }
        else
        {
            Debug.LogWarning("Sonido no encontrado: " + soundName);
        }
    }

    public void StopMusic()
    {
        if (fadeCoroutine != null)
        {
            StopCoroutine(fadeCoroutine);
            fadeCoroutine = null;
        }

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
        musicVolume = Mathf.Clamp01(volume);

        if (fadeCoroutine == null)
        {
            foreach (Sound sound in sounds)
            {
                if (sound.clip == musicSource.clip)
                {
                    musicSource.volume = sound.volume * musicVolume;
                    break;
                }
            }
        }
    }

    public void SetSFXVolume(float volume)
    {
        sfxVolume = Mathf.Clamp01(volume);
    }
}