using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.EventSystems;

public class start : MonoBehaviour
{
    public GameObject OptionsFirst;
    private Canvas settings;
    private Canvas options;
    private void Start()
    {
        settings = GameObject.Find("Menu").GetComponent<Canvas>();
        options = GameObject.Find("Options Menu").GetComponent<Canvas>();
        settings.enabled = true;
        options.enabled = false;
    }

    public void StartGame()
    {
        SceneManager.LoadScene("grinch");
    }

    public void Options()
    {
        settings.enabled = false;
        options.enabled = true;
        EventSystem.current.SetSelectedGameObject(null);
        EventSystem.current.SetSelectedGameObject(OptionsFirst);
    }

    public void QuitGame()
    {
        Application.Quit();
    }
}