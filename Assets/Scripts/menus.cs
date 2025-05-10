using JetBrains.Annotations;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;

public class menus : MonoBehaviour
{
    public GameObject firstButton;
    public GameObject guns;
    public bool open = false;
    public GameObject reticle;
    public GameObject crosshair;

    private Canvas settings;
    private Canvas options;
    private Canvas controls;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        settings = GameObject.Find("Settings Menu").GetComponent<Canvas>();
        options = GameObject.Find("Options Menu").GetComponent<Canvas>();
        controls = GameObject.Find("Controls Menu").GetComponent<Canvas>();
        settings.enabled = false;
        options.enabled = false;
        controls.enabled = false;
        //guns.SetActive(true);
        reticle.SetActive(true);
        crosshair.SetActive(true);
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.M) || Input.GetButtonDown("js10"))
        {
            settings.enabled = true;
            guns.SetActive(false);
            reticle.SetActive(false);
            crosshair?.SetActive(false);
            open = true;
            EventSystem.current.SetSelectedGameObject(null);
            EventSystem.current.SetSelectedGameObject(firstButton);
        }
        //alternative submit for default controller
        //if (Input.GetButtonDown("js2"))
        //{
        //    GameObject current = EventSystem.current.currentSelectedGameObject;
        //    if (current != null)
        //    {
        //        ExecuteEvents.Execute(current, new BaseEventData(EventSystem.current), ExecuteEvents.submitHandler);
        //    }
        //}
    }
    public void CloseMenu()
    {
        open = false;
    }

    public void QuitToMenu()
    {
        SceneManager.LoadScene("startingMenu");
    }
}
