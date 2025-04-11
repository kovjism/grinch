using UnityEngine;
using UnityEngine.UI;

public class MusicVolumeController : MonoBehaviour
{
    [Header("References")]
    public Slider volumeSlider;          // Assign this in Inspector
    public AudioSource musicSource;      // Your background music AudioSource

    private const string volumeKey = "MusicVolume";

    void Start()
    {
        // Load saved volume or default to slider's current value
        float savedVolume = PlayerPrefs.GetFloat(volumeKey, volumeSlider.value);
        volumeSlider.value = savedVolume;
        musicSource.volume = savedVolume;

        // Listen for changes
        volumeSlider.onValueChanged.AddListener(SetVolume);
    }

    public void SetVolume(float volume)
    {
        musicSource.volume = volume;
        PlayerPrefs.SetFloat(volumeKey, volume);
        PlayerPrefs.Save();
    }
}
