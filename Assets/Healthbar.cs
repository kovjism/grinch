using UnityEngine;
using UnityEngine.UI;

public class Healthbar : MonoBehaviour
{
    [SerializeField] private Image healthbarSprite;
    [SerializeField] private float reduceSpeed = 3;
    private float target = 1;

    public void UpdateHealthBar(float maxHealth, float currentHealth) {
        target = currentHealth / maxHealth;
    }

    void Update() {
        healthbarSprite.fillAmount = Mathf.MoveTowards(healthbarSprite.fillAmount, target, reduceSpeed * Time.deltaTime);
    }
}
