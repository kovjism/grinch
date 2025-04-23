using System;
using System.Drawing;
using TMPro;
using UnityEngine;
using static UnityEditorInternal.ReorderableList;

public class tutorialManager : MonoBehaviour
{
    public Canvas tutorial;
    public Canvas rageMode;
    public GameObject guns;
    public RectTransform ammobar;

    GameObject dimPlane = null;

    public int tutorialStage = 0;


    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        tutorial.enabled = true;
        rageMode.enabled = false;
        guns.SetActive(false);
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
            case 1:
                guns.SetActive(true);
                tutorial.GetComponentInChildren<TextMeshProUGUI>().text = "This is your starting gun. Press R2 to shoot.";
                if (Input.GetMouseButtonDown(1))
                    tutorialStage += 1;
                break;
            case 2:
                if (dimPlane == null)
                {
                    dimPlane = GameObject.CreatePrimitive(PrimitiveType.Quad);
                    dimPlane.transform.SetParent(Camera.main.transform);
                    dimPlane.transform.localPosition = new Vector3(0, 0, 0.1f); // just in front of camera
                    dimPlane.transform.localRotation = new Quaternion(0f, 0f, 0f, 0f);
                    dimPlane.transform.localScale = new Vector3(0.2f, 0.12f, 0.1f);
                    dimPlane.GetComponent<Renderer>().material = Resources.Load<Material>("Materials/DimCutout");
                    dimPlane.GetComponent<Renderer>().material.SetVector("_HoleCenter", new Vector4(0.025f, -0.13f, 0, 0));
                    dimPlane.GetComponent<Renderer>().material.SetVector("_HoleSize", new Vector4(0.04f, 0.2f, 0, 0));
                    tutorial.GetComponentInChildren<TextMeshProUGUI>().rectTransform.anchoredPosition = new Vector3(80f, 50f, 0f);
                    tutorial.GetComponentInChildren<TextMeshProUGUI>().text = "Here is your reload bar. Shoot again to move on (R2).";
                }
                if (Input.GetMouseButtonDown(1))
                {
                    tutorialStage += 1;
                    GameObject.Destroy(dimPlane);
                }
                break;
            case 3:
                if (dimPlane == null)
                {
                    dimPlane = GameObject.CreatePrimitive(PrimitiveType.Quad);
                    dimPlane.transform.SetParent(Camera.main.transform);
                    dimPlane.transform.localPosition = new Vector3(0, 0, 0.1f); // just in front of camera
                    dimPlane.transform.localRotation = new Quaternion(0f, 0f, 0f, 0f);
                    dimPlane.transform.localScale = new Vector3(0.2f, 0.12f, 0.1f);
                    dimPlane.GetComponent<Renderer>().material = Resources.Load<Material>("Materials/DimCutout");
                    dimPlane.GetComponent<Renderer>().material.SetVector("_HoleCenter", new Vector4(-0.32f, -0.4f, 0, 0));
                    dimPlane.GetComponent<Renderer>().material.SetVector("_HoleSize", new Vector4(0.35f, 0.2f, 0, 0));
                    tutorial.GetComponentInChildren<TextMeshProUGUI>().rectTransform.anchoredPosition = new Vector3(-620f, -280f, 0f);
                    tutorial.GetComponentInChildren<TextMeshProUGUI>().rectTransform.sizeDelta = new Vector2(700, 50);
                    tutorial.GetComponentInChildren<TextMeshProUGUI>().text = "Here is your gun display. Your display will grow as you unlock more guns. Shoot again to move on (R2).";
                }
                if (Input.GetMouseButtonDown(1))
                {
                    tutorialStage += 1;
                    GameObject.Destroy(dimPlane);
                }
                break;
            case 4:
                tutorial.GetComponentInChildren<TextMeshProUGUI>().rectTransform.anchoredPosition = new Vector3(0f, -400f, 0f);
                tutorial.GetComponentInChildren<TextMeshProUGUI>().rectTransform.sizeDelta = new Vector2(1460, 50);
                tutorial.GetComponentInChildren<TextMeshProUGUI>().text = "Move to the next room.";
                break;
        }
    }
}
