using UnityEngine;
using TMPro;
using UnityEngine.UI;
using System.Collections.Generic;

public class TextSizeManager : MonoBehaviour
{
    [Header("UI Reference")]
    public Slider textSizeSlider; // Assign in inspector

    [Header("Scale Settings")]
    public float defaultScale = 1f;     // slider base value = 1

    private Dictionary<TextMeshProUGUI, float> originalSizes = new();

    private const string textSizeKey = "TextSizeScale";

    void Start()
    {
        // Find all TMP elements in scene
        var allTextElements = FindObjectsByType<TextMeshProUGUI>(FindObjectsSortMode.None);

        foreach (var tmp in allTextElements)
        {
            if (tmp != null && !originalSizes.ContainsKey(tmp))
                originalSizes[tmp] = tmp.fontSize;
        }

        // Load saved scale
        float savedScale = PlayerPrefs.GetFloat(textSizeKey, defaultScale);
        textSizeSlider.value = savedScale;
        UpdateTextSizes(savedScale);

        // Hook up slider listener
        textSizeSlider.onValueChanged.AddListener(OnSliderValueChanged);
    }

    private void OnSliderValueChanged(float value)
    {
        PlayerPrefs.SetFloat(textSizeKey, value);
        PlayerPrefs.Save();
        UpdateTextSizes(value);
    }

    private void UpdateTextSizes(float scale)
    {
        foreach (var entry in originalSizes)
        {
            if (entry.Key != null)
                entry.Key.fontSize = entry.Value * scale;
        }
    }
}
