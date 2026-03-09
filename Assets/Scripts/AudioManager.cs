using UnityEngine;
using System.Collections.Generic;

public class AudioManager : MonoBehaviour
{
    public static AudioManager instance;

    public AudioSource sfxSource;
    public AudioSource musicSource;

    public List<AudioClip> sfxClips = new List<AudioClip>();
    public List<AudioClip> musicClips = new List<AudioClip>();

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public void PlaySFX(string clipName, float volume = 1f)
    {
        AudioClip clip = sfxClips.Find(x => x.name == clipName);
        if (clip != null)
            sfxSource.PlayOneShot(clip, volume);
    }

    public void PlayMusic(string clipName, bool loop = true)
    {
        AudioClip clip = musicClips.Find(x => x.name == clipName);
        if (clip != null)
        {
            musicSource.clip = clip;
            musicSource.loop = loop;
            musicSource.Play();
        }
    }

    public void StopMusic()
    {
        musicSource.Stop();
    }
}