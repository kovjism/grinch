using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class UIContrastManager : MonoBehaviour
{
    public TMP_Dropdown contrastDropdown;
    public Camera mainCamera;

    [Header("Color Themes")]
    public Color defaultBG = new Color32(0, 100, 0, 255); // Dark green
    public Color highContrastBG = Color.black;
    public Color colorblindBG = new Color32(50, 50, 50, 255);

    public Color defaultText = Color.black;
    public Color highContrastText = Color.white;
    public Color colorblindText = Color.white;

    public Color defaultUIBase = Color.white;
    public Color highContrastUIBase = new Color32(255, 215, 0, 255); // Gold
    public Color colorblindUIBase = new Color32(30, 144, 255, 255);  // Dodger Blue

    void Start()
    {
        contrastDropdown.onValueChanged.AddListener(ApplyThemeFromDropdown);

        int savedIndex = PlayerPrefs.GetInt("UIContrastMode", 0);
        contrastDropdown.value = savedIndex;
        ApplyThemeFromDropdown(savedIndex);
    }

    void ApplyThemeFromDropdown(int index)
    {
        PlayerPrefs.SetInt("UIContrastMode", index);

        switch (index)
        {
            case 0:
                ApplyTheme(defaultBG, defaultUIBase, defaultText);
                break;
            case 1:
                ApplyTheme(highContrastBG, highContrastUIBase, highContrastText);
                break;
            case 2:
                ApplyTheme(colorblindBG, colorblindUIBase, colorblindText);
                break;
        }
    }

    void ApplyTheme(Color bgColor, Color uiColor, Color textColor)
    {
        // Change camera background
        if (mainCamera != null)
            mainCamera.backgroundColor = bgColor;

        // Change all UI elements: Buttons, Sliders, Dropdowns
        var buttons = FindObjectsByType<Button>(FindObjectsSortMode.None);
        var sliders = FindObjectsByType<Slider>(FindObjectsSortMode.None);
        var dropdowns = FindObjectsByType<TMP_Dropdown>(FindObjectsSortMode.None);

        foreach (var btn in buttons)
            SetSelectableColors(btn, uiColor);

        foreach (var sld in sliders)
            SetSelectableColors(sld, uiColor);

        foreach (var dd in dropdowns)
            SetSelectableColors(dd, uiColor);

        // Update text color for all TMP_Texts
        var texts = FindObjectsByType<TMP_Text>(FindObjectsSortMode.None);
        foreach (var txt in texts)
            txt.color = textColor;
    }

    void SetSelectableColors(Selectable selectable, Color color)
    {
        var colors = selectable.colors;
        colors.normalColor = color;
        colors.highlightedColor = Color.Lerp(color, Color.red, 0.5f);
        colors.pressedColor = Color.Lerp(color, Color.black, 0.2f);
        colors.selectedColor = colors.highlightedColor;
        selectable.colors = colors;
    }
}