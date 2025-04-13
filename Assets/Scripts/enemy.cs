using UnityEngine;
using UnityEngine.AI;

public class enemy : MonoBehaviour
{
    [SerializeField] private float maxHealth = 5;
    private float currentHealth;
    [SerializeField] private AudioClip damageSoundClip;
    [SerializeField] private GameObject healthbarPrefab;

    private NavMeshAgent agent;
    private Transform player;
    private menus menu;

    [SerializeField] private Healthbar healthbar;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        currentHealth = maxHealth;
        agent = GetComponent<NavMeshAgent>();
        player = GameObject.FindGameObjectWithTag("Player").transform;
        
        GameObject hb = Instantiate(healthbarPrefab, transform.position + Vector3.up * 2f, Quaternion.identity);
        hb.transform.SetParent(transform); 
        healthbar = hb.GetComponent<Healthbar>();

        Canvas canvas = hb.GetComponent<Canvas>();
        if (canvas != null) canvas.worldCamera = Camera.main;

        healthbar.UpdateHealthBar(maxHealth, currentHealth);

        menu = FindAnyObjectByType<menus>();
    }

    // Update is called once per frame
    void Update()
    {
        if (menu.open)
        {
            agent.ResetPath();
            agent.velocity = Vector3.zero;
        } else
        {
            agent.SetDestination(player.position);
        }
            
    }

    
    public void takeDamage(int damage)
    {
        //play ouch sound
        SoundFXManager.instance.PlaySoundFXClip(damageSoundClip, transform, 0.2f);

        currentHealth -= damage;
        if (currentHealth <= 0)
        {
            //play death sound?
            Destroy(gameObject);
        }
        else {
            healthbar.UpdateHealthBar(maxHealth, currentHealth);
         }

    }
}
