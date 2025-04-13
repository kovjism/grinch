using System;
using Unity.VisualScripting;
using UnityEngine;

public class SoundFXManager : MonoBehaviour
{
    public static SoundFXManager instance;
    private float minSoundInterval = 0.5f;
    [SerializeField] private AudioSource soundFXObject;
    private AudioSource[] audioSources = new AudioSource[32];
    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
    }

    public void PlaySoundFXClip(AudioClip audioClip, Transform spawnTransform, float volume)
    {
        foreach (AudioSource source in audioSources)
        {
            if (source != null)
            if (source.isPlaying && source.clip.name == audioClip.name && source.time <= minSoundInterval)
                return;
        }
        AudioSource audioSource = Instantiate(soundFXObject, spawnTransform.position, Quaternion.identity);
        audioSource.clip = audioClip;
        audioSource.volume = volume * PlayerPrefs.GetFloat("SFX Volume", 0.5f);
        audioSource.Play();
        for (int i = 0; i < 32; i++)
        {
            if (audioSources[i] == null)
            {
                audioSources[i] = audioSource;
                break;
            }
        }
        float clipLength = audioSource.clip.length;
        Destroy(audioSource.gameObject, clipLength);
        for (int i = 0; i < 32; i++)
        {
            if (audioSources[i] == audioClip)
            {
                audioSources[i] = null;
                break;
            }
        }
    }
}
