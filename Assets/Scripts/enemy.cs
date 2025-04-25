using UnityEngine;
using UnityEngine.AI;

public class enemy : MonoBehaviour
{
    [SerializeField] private float maxHealth = 5;
    private float currentHealth;
    [SerializeField] private AudioClip damageSoundClip;
    [SerializeField] private GameObject healthbarPrefab;

    [SerializeField] private string pickupTag = "Pickup";
    [SerializeField] private Transform holdPoint;
    private Transform escapePoint;

    private GameObject carriedItem;
    private bool hasItem = false;
    private GameObject targetItem;
    

    private NavMeshAgent agent;
    private menus menu;
    [SerializeField] private Healthbar healthbar;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        escapePoint = GameObject.FindGameObjectWithTag("EscapePoint").transform;

        currentHealth = maxHealth;
        agent = GetComponent<NavMeshAgent>();
        
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
            return;
        } 
        if (!hasItem)
        {
            if (targetItem == null)
            {
                targetItem = FindClosestPickup();
            }

            if (targetItem != null)
            {
                agent.SetDestination(targetItem.transform.position);

                float distance = Vector3.Distance(transform.position, targetItem.transform.position);
                if (distance < 2f)
                {
                    PickUpItem(targetItem);
                }
            }
        }
        else
        {
            agent.SetDestination(escapePoint.position);

            float escapeDistance = Vector3.Distance(transform.position, escapePoint.position);
            if (escapeDistance < 1.5f)
            {
                Escape();
            }
        }
    }
    GameObject FindClosestPickup()
    {
        GameObject[] pickups = GameObject.FindGameObjectsWithTag(pickupTag);
        GameObject closest = null;
        float closestDistance = Mathf.Infinity;

        foreach (GameObject obj in pickups)
        {
            float dist = Vector3.Distance(transform.position, obj.transform.position);
            if (dist < closestDistance)
            {
                closest = obj;
                closestDistance = dist;
            }
        }

        return closest;
    }

    void PickUpItem(GameObject item)
    {
        carriedItem = item;
        hasItem = true;

        item.transform.SetParent(holdPoint);
        item.transform.localPosition = Vector3.zero;
        item.GetComponent<Collider>().enabled = false;

        Rigidbody rb = item.GetComponent<Rigidbody>();
        if (rb != null) rb.isKinematic = true;
    }

    void Escape()
{
    // Check if the enemy is carrying an item
    if (carriedItem != null)
    {
        // Drop the item near the enemy
        carriedItem.transform.SetParent(null); // Unset the holdPoint parent
        carriedItem.GetComponent<Collider>().enabled = true; // Re-enable the collider so other enemies can pick it up

        // If the item has a Rigidbody, let it fall to the ground
        Rigidbody rb = carriedItem.GetComponent<Rigidbody>();
        if (rb != null)
        {
            rb.isKinematic = false; // Make the Rigidbody dynamic so it can fall
            rb.useGravity = true; // Ensure gravity is enabled
        }

        // Position the item a little above the ground so it falls down
        carriedItem.transform.position = new Vector3(transform.position.x, transform.position.y + 1f, transform.position.z);
    }

    // Destroy the enemy object (the enemy escapes)
    Destroy(gameObject);
}
    
    public void takeDamage(int damage)
    {
        //play ouch sound
        SoundFXManager.instance.PlaySoundFXClip(damageSoundClip, transform, 0.2f);

        currentHealth -= damage;
        if (currentHealth <= 0)
        {
          
            DropItem();
            Destroy(gameObject);
        }
        else {
            healthbar.UpdateHealthBar(maxHealth, currentHealth);
         }

void DropItem()
{
    if (carriedItem != null)
    {
        // Unset the parent and re-enable the collider
        carriedItem.transform.SetParent(null); // Remove from the holdPoint
        carriedItem.GetComponent<Collider>().enabled = true; // Re-enable the collider for interactions

        // If the item has a Rigidbody, enable physics for it
        Rigidbody rb = carriedItem.GetComponent<Rigidbody>();
        if (rb != null)
        {
            rb.isKinematic = false; // Make the Rigidbody dynamic
            rb.useGravity = true;   // Enable gravity for the item
        }

        // Position the item slightly above the ground to avoid clipping into the floor
        carriedItem.transform.position = new Vector3(transform.position.x, transform.position.y + 1f, transform.position.z); // Adjust this value if needed
    }
}
    }
}
