using System.Runtime.CompilerServices;
using TMPro;
using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class Player_Status : MonoBehaviour
{
    [SerializeField] private GameObject grinchCanvas;
    private TextMeshProUGUI rageModeText;
    private int rageStatus;
    private Gyroscope gyro;
    Vector3 rotationRate;
    private CharacterMovement movementScript;
    private bool canShowRageMessage;

    // take player damage
    [SerializeField] private int playerHealth = 10; // Set as needed
    [SerializeField] private AudioClip damageSoundClip;
    [SerializeField] private int maxHealth = 10;
    [SerializeField] private int currentHealth;
    [SerializeField] private Slider healthBarSlider;


    void Start()
    {
        movementScript = this.GetComponent<CharacterMovement>();
        GyroManager.Instance.enableGyro();
        gyro = Input.gyro;
        rageModeText = grinchCanvas.GetComponentInChildren<TextMeshProUGUI>();
        rageStatus = 0;
        canShowRageMessage = true;

        // player health --------------
        currentHealth = maxHealth;

        if (healthBarSlider != null)
        {
            healthBarSlider.maxValue = maxHealth;
            healthBarSlider.value = currentHealth;
        }

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
            //rageStatus = 0;
        }
    }
    private void ActivateRage()
    {
        //start 5 second countdown
        //StartCoroutine(RageCoroutine());
        movementScript.speed = 18f;
    }
    //private IEnumerator RageCoroutine()
    //{
    //    float originalSpeed = movementScript.speed;
    //    movementScript.speed = 18f;

    //    //// Wait during rage
    //    //yield return new WaitForSeconds(5f);

    //    //HideGrinchCanvas();
    //    //movementScript.speed = originalSpeed;
    //}
    private void ResetRageMessage()
    {
        canShowRageMessage = true;
    }
    private void HideGrinchCanvas()
    {
        grinchCanvas.SetActive(false);
    }

    public void TakeDamage(int damage)
    {
        currentHealth -= damage;
        currentHealth = Mathf.Clamp(currentHealth, 0, maxHealth);

        if (healthBarSlider != null)
            healthBarSlider.value = currentHealth;

        if (currentHealth <= 0)
        {
            Debug.Log("Player has died!");
            // You can call GameOverPanel.SetActive(true) or similar here
        }

        if (damageSoundClip != null)
            SoundFXManager.instance.PlaySoundFXClip(damageSoundClip, transform, 0.3f);
    }

}
