using System.Runtime.CompilerServices;
using TMPro;
using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using Unity.VisualScripting;
using UnityEngine.SceneManagement;
using UnityEngine.EventSystems;


public class Player_Status : MonoBehaviour
{
    [SerializeField] private GameObject grinchCanvas;
    private TextMeshProUGUI rageModeText;
    private int rageStatus;
    private float originalSpeed;
    public Image rageOverlay;
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

    // player damage taken
    private float enemyDamageCooldown = 0.2f;
    private float lastEnemyDamageTime = -999f;

    public GameObject gameOverPanel;
    public GameObject loseButton;


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

        gameOverPanel.SetActive(false);

    }
    void Update()
    {
        Quaternion deviceRotation = gyro.attitude;
        Vector3 euler = ConvertGyroRotationToUnity(deviceRotation);
        float pitch = euler.x;

        // Normalize to 0?80, where 0 is forward and ~90 is looking up
        if (pitch > 180f) pitch -= 360f;
        bool isLookingUp = pitch < -80f;

        //add cooldown

        if (isLookingUp && Input.GetButtonDown("js2"))
        {
            rageStatus = 1;
        }
        CheckForRage(rageStatus);
    }

    Vector3 ConvertGyroRotationToUnity(Quaternion q)
    {
        // Converts right-handed to Unity's left-handed coordinate system
        return new Quaternion(q.x, q.y, -q.z, -q.w).eulerAngles;
    }

private void CheckForRage(int currentRage)
    {
        //check for upward tilt phone movement
        if (currentRage == 0)
        {
            rageModeText.text = "Rage Mode is Ready!\n(Tilt Up and Press Interact)";
            grinchCanvas.SetActive(true);
            Invoke("HideGrinchCanvas", 2);

            if (canShowRageMessage)
            {
                //add cooldown
                canShowRageMessage = false;
                Invoke("ResetRageMessage", 5f);
            }
        }
        else if (currentRage == 1)
        {
            rageModeText.text = "Rage Mode Activated!";
            grinchCanvas.SetActive(true);
            ActivateRage();
            rageStatus = 0;
        }
    }
    private void ActivateRage()
    {
        //modify Grinch canvas

        //start 5 second countdown
        StartCoroutine(RageCoroutine());
        HideGrinchCanvas();
        movementScript.speed = originalSpeed;
    }
    private IEnumerator RageCoroutine()
    {
        SetOverlayAlpha(0.5f);
        originalSpeed = movementScript.speed;
        movementScript.speed = 18f;
        SetOverlayAlpha(0f);
        // Wait during rage
        yield return new WaitForSeconds(5f);
    }
    private void SetOverlayAlpha(float alpha)
    {
        if (rageOverlay != null)
        {
            Color c = rageOverlay.color;
            c.a = alpha;
            rageOverlay.color = c;
        }
    }
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
            Die();
            // You can call GameOverPanel.SetActive(true) or similar here
        }

        if (damageSoundClip != null)
            SoundFXManager.instance.PlaySoundFXClip(damageSoundClip, transform, 0.3f);
    }
    void Die()
    {
        gameOverPanel.SetActive(true);
        Time.timeScale = 0f; // pause the game
        EventSystem.current.SetSelectedGameObject(null);
        EventSystem.current.SetSelectedGameObject(loseButton);
    }

    public void RestartGame()
    {
        ResumeTime(); // if you paused time
        SceneManager.LoadScene("startingMenu"); // Use the exact scene name
    }


    // maybe call this to resume time when restarting
    public void ResumeTime()
    {
        Time.timeScale = 1f;
    }


    private void OnTriggerStay(Collider other)
    {
        if (other.CompareTag("Enemy"))
        {
            if (Time.time - lastEnemyDamageTime >= enemyDamageCooldown)
            {
                TakeDamage(5); // Adjust damage as needed
                lastEnemyDamageTime = Time.time;
            }
            //TakeDamage(1); // Adjust damage as needed
            //lastEnemyDamageTime = Time.time;
        }
    }


}
