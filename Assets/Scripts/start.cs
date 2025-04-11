using Palmmedia.ReportGenerator.Core;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.EventSystems;

public class start : MonoBehaviour
{
    public GameObject OptionsFirst;

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
        EventSystem.current.SetSelectedGameObject(null);            
        EventSystem.current.SetSelectedGameObject(OptionsFirst);     
    }   

    public void QuitGame()
    {
        Application.Quit();
    }
}
