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

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        UpdateWeaponVisibility();
    }

    // Update is called once per frame
    void Update()
    {
        float fillAmount = Mathf.Clamp01(1 - ((guns[currentGun].nextFire - Time.time) / guns[currentGun].fireRate));
        ammoBar.value = fillAmount;
        if (Input.GetMouseButtonDown(1) || Input.GetButtonDown("js0"))
        {
            guns[currentGun].Shoot();   
        }
        if (Input.GetAxis("Mouse ScrollWheel") > 0f)
        {
            currentGun = (currentGun + 1) % guns.Count;
            guns[currentGun].Equip();
            UpdateWeaponVisibility();
        }
        else if (Input.GetAxis("Mouse ScrollWheel") < 0f)
        {
            currentGun = (currentGun - 1 + guns.Count) % guns.Count;
            guns[currentGun].Equip();
            UpdateWeaponVisibility();
        }
    }
    void UpdateWeaponVisibility()
    {
        for (int i = 0; i < guns.Count; i++)
        {
            guns[i].gameObject.SetActive(i == currentGun);
        }
    }
}
