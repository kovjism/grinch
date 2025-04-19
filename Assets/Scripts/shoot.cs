using NUnit.Framework;
using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;
using Unity.VisualScripting;

public class shoot : MonoBehaviour
{
    // public objects
    public List<gun> guns;
    public Slider ammoBar;
    public GameObject gunIconPrefab;
    public Transform iconDisplay;
    public List<GameObject> icons;

    // private variables
    private int currentGun = 0;
    private Gyroscope gyro;
    Vector3 rotationRate;
    private bool canConfirm = true;
    private float nodCooldown = 1f;
    private float lastNodTime = 0f;
    private float iconSpacing = 150f;
    private Vector3 selectedScale = new Vector3(1.3f, 1.3f, 1f);
    private Vector3 normalScale = Vector3.one;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        GyroManager.Instance.enableGyro();
        gyro = Input.gyro;
        UpdateWeapons();
    }

    // Update is called once per frame
    void Update()
    {
        if (guns.Count == 0)
        {
            return;
        }
        rotationRate = gyro.rotationRate;
        float fillAmount = Mathf.Clamp01(1 - ((guns[currentGun].nextFire - Time.time) / guns[currentGun].fireRate));
        ammoBar.value = fillAmount;
        if (Input.GetMouseButtonDown(1) || Input.GetButtonDown("js7"))
        {
            guns[currentGun].Shoot();   
        }
        if (Input.GetAxis("Mouse ScrollWheel") > 0f || Input.GetButtonDown("js5") || (rotationRate.x < -0.8f && canConfirm))
        {
            canConfirm = false;
            lastNodTime = Time.time;
            currentGun = (currentGun + 1) % guns.Count;
            guns[currentGun].Equip();
            UpdateWeapons();
        }
        else if (Input.GetAxis("Mouse ScrollWheel") < 0f || Input.GetButtonDown("js4"))
        {
            currentGun = (currentGun - 1 + guns.Count) % guns.Count;
            guns[currentGun].Equip();
            UpdateWeapons();
        }
        if (!canConfirm && Time.time - lastNodTime > nodCooldown)
        {
            canConfirm = true;
        }
    }
    void UpdateWeapons()
    {
        for (int i = 0; i < guns.Count; i++)
        {
            guns[i].gameObject.SetActive(i == currentGun);
            RectTransform rt = icons[i].GetComponent<RectTransform>();
            if (rt != null)
            {
                rt.localScale = (i == currentGun) ? selectedScale : normalScale;
            }
        }
    }

    // Add this method to your shoot class
    public bool AddGun(GameObject gunPrefab)
    {
        Debug.LogError("Adding Gun!");
        if (gunPrefab == null) return false;

        // Check if this gun is already in our inventory
        gun gunComponent = gunPrefab.GetComponent<gun>();
        if (gunComponent == null)
        {
            Debug.LogError("Gun prefab doesn't have a gun component!");
            return false;
        }

        // Check if we already have this gun type
        foreach (gun existingGun in guns)
        {
            if (existingGun.GetType() == gunComponent.GetType())
            {
                Debug.Log("You already have this gun type!");
                return false;
            }
        }

        // Instantiate the gun as a child of this object
        GameObject newGun = Instantiate(gunPrefab, transform);
        newGun.GetComponent<Outline>().enabled = false;

        // Reset transform to make sure it's in view
        newGun.transform.localPosition = new Vector3(0.4f, -0.2f, 1f);  // later change this to the EXACT position
        newGun.transform.localRotation = Quaternion.identity;

        gun newGunComponent = newGun.GetComponent<gun>();

        // Setup camera reference for the new gun
        newGunComponent.cameraObject = guns[0].cameraObject;

        // Add to our guns list
        guns.Add(newGunComponent);

        // Initially disable the gun since it's not currently selected
        newGun.SetActive(false);

        // If this is our first gun, select it
        if (guns.Count == 1)
        {
            currentGun = 0;
            UpdateWeapons();
        }

        GameObject newIcon = Instantiate(gunIconPrefab, iconDisplay);
        Image iconImage = newIcon.GetComponent<Image>();
        iconImage.sprite = newGun.GetComponent<gun>().icon;

        RectTransform rt = newIcon.GetComponent<RectTransform>();
        rt.localScale = Vector3.one;
        rt.sizeDelta = new Vector2(100, 100); // or whatever looks good

        // Calculate new icon position
        Vector3 newPos = new Vector3(iconSpacing * (guns.Count - 1), 0f, 0f);
        rt.localPosition = newPos;

        icons.Add(newIcon);

        return true;
    }
}
