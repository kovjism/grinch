using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;

public class menus : MonoBehaviour
{
    public GameObject firstButton;
    public GameObject guns;
    public bool open = false;

    private Canvas settings;
    private Canvas options;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        settings = GameObject.Find("Settings").GetComponent<Canvas>();
        options = GameObject.Find("Options Menu").GetComponent<Canvas>();
        settings.enabled = false;
        options.enabled = false;
        guns.SetActive(true);
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.M))
        {
            settings.enabled = true;
            guns.SetActive(false);
            open = true;
            EventSystem.current.SetSelectedGameObject(null);
            EventSystem.current.SetSelectedGameObject(firstButton);
        }
    }

    public void QuitToMenu()
    {
        SceneManager.LoadScene("startingMenu");
    }
}
