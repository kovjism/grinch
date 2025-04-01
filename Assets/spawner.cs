using UnityEngine;

public class spawner : MonoBehaviour
{
    public GameObject enemy;
    public Transform player;
    public int numEnemies;

    private float minDistance = 60f;
    private float maxDistance = 70f;
    
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        for (int i = 0; i < numEnemies; i++)
        {
            float randomAngle = Random.Range(0f, 360f); // Random angle in degrees
            float randomDistance = Random.Range(minDistance, maxDistance);

            // Convert polar coordinates to Cartesian
            float spawnX = player.position.x + randomDistance * Mathf.Cos(randomAngle * Mathf.Deg2Rad);
            float spawnZ = player.position.z + randomDistance * Mathf.Sin(randomAngle * Mathf.Deg2Rad);

            Vector3 spawnPosition = new Vector3(spawnX, player.position.y, spawnZ);
            Instantiate(enemy, spawnPosition, Quaternion.identity);
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
