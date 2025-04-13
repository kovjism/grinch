using NUnit.Framework;
using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;

public class shoot : MonoBehaviour
{
    // public objects
    public List<gun> guns;
    public Slider ammoBar;

    // private variables
    private int currentGun = 0;
    private Gyroscope gyro;
    Vector3 rotationRate;
    private bool canConfirm = true;
    private float nodCooldown = 1f;
    private float lastNodTime = 0f;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        GyroManager.Instance.enableGyro();
        gyro = Input.gyro;
        UpdateWeaponVisibility();
    }

    // Update is called once per frame
    void Update()
    {
        rotationRate = gyro.rotationRate;
        float fillAmount = Mathf.Clamp01(1 - ((guns[currentGun].nextFire - Time.time) / guns[currentGun].fireRate));
        ammoBar.value = fillAmount;
        if (Input.GetMouseButtonDown(1) || Input.GetButtonDown("js5"))
        {
            guns[currentGun].Shoot();   
        }
        if (Input.GetAxis("Mouse ScrollWheel") > 0f || Input.GetButtonDown("js7") || (rotationRate.x < -0.8f && canConfirm))
        {
            canConfirm = false;
            lastNodTime = Time.time;
            currentGun = (currentGun + 1) % guns.Count;
            guns[currentGun].Equip();
            UpdateWeaponVisibility();
        }
        else if (Input.GetAxis("Mouse ScrollWheel") < 0f || Input.GetButtonDown("js8"))
        {
            currentGun = (currentGun - 1 + guns.Count) % guns.Count;
            guns[currentGun].Equip();
            UpdateWeaponVisibility();
        }
        if (!canConfirm && Time.time - lastNodTime > nodCooldown)
        {
            canConfirm = true;
        }
    }
    void UpdateWeaponVisibility()
    {
        for (int i = 0; i < guns.Count; i++)
        {
            guns[i].gameObject.SetActive(i == currentGun);
        }
    }

    // Add this method to your shoot class
    public bool AddGun(GameObject gunPrefab)
    {
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
            UpdateWeaponVisibility();
        }

        return true;
    }
}
