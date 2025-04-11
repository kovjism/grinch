using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class DropdownSongSelector : MonoBehaviour
{
    [Header("References")]
    public AudioSource audioSource;
    public TMP_Dropdown dropdownMenu;
    public AudioClip[] songs;

    private const string SongPrefKey = "SelectedSongIndex";

    void Start()
    {
        // Load saved song index or default to 0
        int savedIndex = PlayerPrefs.GetInt(SongPrefKey, 0);

        // Clamp to valid range
        savedIndex = Mathf.Clamp(savedIndex, 0, songs.Length - 1);

        dropdownMenu.value = savedIndex;
        dropdownMenu.onValueChanged.AddListener(OnDropdownChanged);

        PlaySong(savedIndex);
    }

    void OnDropdownChanged(int index)
    {
        PlayerPrefs.SetInt(SongPrefKey, index);
        PlayerPrefs.Save(); // Optional, but ensures it's saved immediately
        PlaySong(index);
    }

    void PlaySong(int index)
    {
        if (index >= 0 && index < songs.Length)
        {
            audioSource.clip = songs[index];
            audioSource.Play();
        }
    }
}
