using System;
using UnityEngine;
using static UnityEditorInternal.ReorderableList;

public class tutorial_manager : MonoBehaviour
{
    public Canvas tutorial;
    public Canvas rageMode;
    public Canvas guns;
    public AudioSource sfx;
    public AudioSource music;

    private int tutorialStage = 0;


    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        tutorial.enabled = true;
        rageMode.enabled = false;
        guns.enabled = false;
        sfx.enabled = false;
        music.enabled = false;
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.M))
        {
            tutorial.enabled = false;
        }

        switch (tutorialStage)
        {
            case 0:
                
                break;
            case 1:
                
                break;
            case 2:
                
                break;
        }
    }
}
