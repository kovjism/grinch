using UnityEngine;
using UnityEngine.UI;

public class masterVolumeController : MonoBehaviour
{
    public Slider masterSlider;               // Master volume slider
    private const string masterKey = "MasterVolume";

    void Start()
    {
        float saved = PlayerPrefs.GetFloat(masterKey, masterSlider.value);
        masterSlider.value = saved;

        // Apply to all controllers
        ApplyToAllMusic();

        // Listen for changes
        masterSlider.onValueChanged.AddListener(SetMasterVolume);
    }

    public void SetMasterVolume(float value)
    {
        PlayerPrefs.SetFloat(masterKey, value);
        PlayerPrefs.Save();
        ApplyToAllMusic();
    }

    private void ApplyToAllMusic()
    {
        volumeControl[] allMusic = FindObjectsByType<volumeControl>(FindObjectsSortMode.None);
        foreach (var mvc in allMusic)
        {
            mvc.UpdateEffectiveVolume();
        }
    }
}
