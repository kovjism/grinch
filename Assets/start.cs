using Palmmedia.ReportGenerator.Core;
using UnityEngine;
using UnityEngine.SceneManagement;

public class start : MonoBehaviour
{
    public void StartGame()
    {
        SceneManager.LoadScene("grinch");
    }

    public void Options()
    {
        Canvas settings = GameObject.Find("Menu").GetComponent<Canvas>();
        settings.enabled = false;
        Canvas options = GameObject.Find("Options Menu").GetComponent<Canvas>();
        options.enabled = true;
    }

    public void QuitGame()
    {
        Application.Quit();
    }
}
