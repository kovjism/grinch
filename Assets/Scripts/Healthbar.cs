using UnityEngine;
using UnityEngine.UI;

public class Healthbar : MonoBehaviour
{
    [SerializeField] private Image healthbarSprite;
    [SerializeField] private float reduceSpeed = 4;
    private float target = 1;

    private Transform camTransform;

    public void UpdateHealthBar(float maxHealth, float currentHealth) {
        target = currentHealth / maxHealth;
    }

    void Start()
    {
        camTransform = Camera.main.transform;
    }
    void Update() {
        transform.LookAt(transform.position - camTransform.forward);

        healthbarSprite.fillAmount = Mathf.MoveTowards(healthbarSprite.fillAmount, target, reduceSpeed * Time.deltaTime);
    }
}
