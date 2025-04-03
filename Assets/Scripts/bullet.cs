using UnityEngine;

public class bullet : MonoBehaviour
{
    public float lifeTime = 2f;
    public int damage = 1;
    private Vector3 target;
    private GameObject targetEnemy;
    private bool hit = false;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        Destroy(gameObject, lifeTime);
    }

    // Update is called once per frame
    void Update()
    {

    }

    // 
    void OnCollisionEnter(Collision collision)
    {
        if (!hit && collision.gameObject.CompareTag("Enemy"))
        {
            enemy enemy = collision.gameObject.GetComponent<enemy>();
            enemy.takeDamage(damage);
        }
        Destroy(gameObject);
    }

    public void SetDamage(int dmg)
    {
        damage = dmg;
    }
}
