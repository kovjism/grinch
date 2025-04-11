using UnityEngine;
using UnityEngine.AI;

public class enemy : MonoBehaviour
{
    public int health = 3;
    [SerializeField] private AudioClip damageSoundClip;
    private NavMeshAgent agent;
    private Transform player;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        player = GameObject.FindGameObjectWithTag("Player").transform;
    }

    // Update is called once per frame
    void Update()
    {
        agent.SetDestination(player.position);
    }

    
    public void takeDamage(int damage)
    {
        //play ouch sound
        SoundFXManager.instance.PlaySoundFXClip(damageSoundClip, transform, 1f);

        health -= damage;
        if (health <= 0)
        {
            //play death sound?
            Destroy(gameObject);
        }

    }
}
