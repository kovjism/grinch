using UnityEngine;
using UnityEngine.AI;
using System.Collections;

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
            // Check if the current targetItem is now taken
            if (targetItem != null)
            {
                PickupItem pickupScript = targetItem.GetComponent<PickupItem>();
                if (pickupScript == null || pickupScript.isTaken)
                {
                    targetItem = null; // Clear it so we can look for a new one
                }
            }

            // If no target or previous was taken, find a new one
            if (targetItem == null)
            {
                targetItem = FindClosestPickup();
            }

            // If we found a valid target, go to it
            if (targetItem != null)
            {
                agent.SetDestination(targetItem.transform.position);

                float distance = Vector3.Distance(transform.position, targetItem.transform.position);
                if (distance < 2f)
                {
                    PickUpItem(targetItem);
                }
            }
            else
            {
                // No gifts left, attack the player
                GameObject player = GameObject.FindGameObjectWithTag("Player");
                if (player != null)
                {
                    agent.SetDestination(player.transform.position);
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
            PickupItem pickupItem = obj.GetComponent<PickupItem>();
            if (pickupItem != null && pickupItem.isTaken) continue; // Skip already taken

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

        PickupItem pickupItem = item.GetComponent<PickupItem>();
        if (pickupItem != null) pickupItem.isTaken = true; // mark as taken

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
            PickupItem pickupItem = carriedItem.GetComponent<PickupItem>();
            if (pickupItem != null) pickupItem.isTaken = false;
            // If the item has a Rigidbody, let it fall to the ground
            Rigidbody rb = carriedItem.GetComponent<Rigidbody>();
            if (rb != null)
            {
                rb.isKinematic = false; // Make the Rigidbody dynamic so it can fall
                rb.useGravity = true; // Ensure gravity is enabled
            }

            // Position the item a little above the ground so it falls down
            carriedItem.transform.position = new Vector3(transform.position.x, transform.position.y + 1f, transform.position.z);

            // Create a new GameObject to handle the gift destruction
            GameObject destroyer = new GameObject("GiftDestroyer");
            GiftDestroyer destroyerScript = destroyer.AddComponent<GiftDestroyer>();
            destroyerScript.Initialize(carriedItem);
        }

        // Destroy the enemy object (the enemy escapes)
        Destroy(gameObject);
    }

    public void takeDamage(int damage)
    {
        //play ouch sound
        SoundFXManager.instance.PlaySoundFXClip(damageSoundClip, transform, 0.15f);

        currentHealth -= damage;
        if (currentHealth <= 0)
        {
          
            DropItem();
            Destroy(gameObject);
        }
        else {
            healthbar.UpdateHealthBar(maxHealth, currentHealth);
         }
    }

    void DropItem()
    {
        if (carriedItem != null)
        {
            carriedItem.transform.SetParent(null);
            carriedItem.GetComponent<Collider>().enabled = true;

            PickupItem pickupItem = carriedItem.GetComponent<PickupItem>();
            if (pickupItem != null) pickupItem.isTaken = false;

            Rigidbody rb = carriedItem.GetComponent<Rigidbody>();
            if (rb != null)
            {
                rb.isKinematic = false;
                rb.useGravity = true;
            }

            carriedItem.transform.position = new Vector3(transform.position.x, transform.position.y + 1f, transform.position.z);
        }
    }

    
}

public class GiftDestroyer : MonoBehaviour
{
    private GameObject giftToDestroy;

    public void Initialize(GameObject gift)
    {
        giftToDestroy = gift;
        StartCoroutine(DestroyGift());
    }

    private IEnumerator DestroyGift()
    {
        yield return new WaitForSeconds(1f);
        
        if (giftToDestroy != null)
        {
            Destroy(giftToDestroy);
        }
        Destroy(gameObject); // Destroy this helper object as well
    }
}
