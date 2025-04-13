using System.Runtime.CompilerServices;
using TMPro;
using UnityEngine;
using System.Collections;

public class Player_Status : MonoBehaviour
{
    [SerializeField] private GameObject grinchCanvas;
    private TextMeshProUGUI rageModeText;
    private int rageStatus;
    private Gyroscope gyro;
    Vector3 rotationRate;
    private CharacterMovement movementScript;
    private bool canShowRageMessage;
    void Start()
    {
        movementScript = this.GetComponent<CharacterMovement>();
        GyroManager.Instance.enableGyro();
        gyro = Input.gyro;
        rageModeText = grinchCanvas.GetComponentInChildren<TextMeshProUGUI>();
        rageStatus = 0;
        canShowRageMessage = true;
    }
    void Update()
    {
        rotationRate = gyro.rotationRate;
        CheckForRage(rageStatus);
    }
    private void CheckForRage(int currentRage)
    {
        //check for right tilt phone movement
        if (currentRage == 0)
        {
            rageModeText.text = "Rage Mode is Ready!\n(Tilt Right to Start)";
            grinchCanvas.SetActive(true);
            //Invoke("HideGrinchCanvas", 2);

            if (rotationRate.y > 0.5f)
            {
                rageStatus++;
                return;
            }
            //if (canShowRageMessage)
            //{
            //    //add cooldown
            //    canShowRageMessage = false;
            //    Invoke("ResetRageMessage", 5f);
            //}
        }
        //check for left tilt phone movement
        else if (currentRage == 1)
        {
            rageModeText.text = "Rage Mode Started...\n(Tilt Left to Continue)";
            grinchCanvas.SetActive(true);
            //Invoke("HideGrinchCanvas", 2);

            if (rotationRate.y < -0.5f)
            {
                rageStatus++;
                return;
            }

            //add cooldown
            //canShowRageMessage = false;
            //Invoke("ResetRageMessage", 5f);
        }
        //check for up tilt phone movement
        else if (currentRage == 2)
        {
            rageModeText.text = "Rage Mode Charging...\n(Tilt Up to Activate)";
            grinchCanvas.SetActive(true);
            //Invoke("HideGrinchCanvas", 2);

            if (rotationRate.x < -0.5f)
            {
                rageStatus++;
                return;
            }

            //add cooldown
            //canShowRageMessage = false;
            //Invoke("ResetRageMessage", 5f);
        }
        else if (currentRage == 3)
        {
            rageModeText.text = "Rage Mode Activated!";
            grinchCanvas.SetActive(true);
            ActivateRage();
            rageStatus = 0;
        }
    }
    private void ActivateRage()
    {
        //start 5 second countdown
        StartCoroutine(RageCoroutine());
    }
    private IEnumerator RageCoroutine()
    {
        float originalSpeed = movementScript.speed;
        movementScript.speed = 18f;

        // Wait during rage
        yield return new WaitForSeconds(5f);

        HideGrinchCanvas();
        movementScript.speed = originalSpeed;
    }
    private void ResetRageMessage()
    {
        canShowRageMessage = true;
    }
    private void HideGrinchCanvas()
    {
        grinchCanvas.SetActive(false);
    }
}
