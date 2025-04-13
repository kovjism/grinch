using UnityEngine;
using UnityEngine.UI;

public class volumeControl : MonoBehaviour
{
    [Header("References")]
    public Slider volumeSlider;              // Individual music slider
    public AudioSource musicSource;          // Target audio source

    private string key;
    private const string masterKey = "MasterVolume";

    void Start()
    {
        key = gameObject.name;
        float savedVolume = PlayerPrefs.GetFloat(key, volumeSlider.value);
        volumeSlider.value = savedVolume;

        // Apply combined volume at start
        UpdateEffectiveVolume();

        // Listen for user changing music slider
        volumeSlider.onValueChanged.AddListener(delegate {
            SaveAndApply();
        });
    }

    public void SaveAndApply()
    {
        PlayerPrefs.SetFloat(key, volumeSlider.value);
        PlayerPrefs.Save();
        UpdateEffectiveVolume();
    }

    public void UpdateEffectiveVolume()
    {
        float master = PlayerPrefs.GetFloat(masterKey, 0.5f);
        musicSource.volume = volumeSlider.value * master;
    }
}
