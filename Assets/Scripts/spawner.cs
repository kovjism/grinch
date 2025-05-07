using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using TMPro;
public class spawner : MonoBehaviour
{
    public GameObject enemy;
    public GameObject santaEnemy;

    public Transform player;
    public int numEnemies;
    private Transform cameraTransform;

    public float timeBetweenWaves = 3f;

    private int currentWave = 1;
    private List<GameObject> activeEnemies = new List<GameObject>();
    private bool waveInProgress = false;
    private float waveTimer = 0f;
    public TMP_Text waveText;
    public TMP_Text rewardText;
    public GameObject rewardPresentWave1Prefab;
    public GameObject rewardPresentWave2Prefab;
    GameObject rewardToSpawn = null;

    [SerializeField] private AudioClip victorySoundClip;
    //private float minDistance = 60f;
    //private float maxDistance = 70f;
    
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        // circle
        //for (int i = 0; i < numEnemies; i++)
        //{
        //    float randomAngle = Random.Range(0f, 360f); // Random angle in degrees
        //    float randomDistance = Random.Range(minDistance, maxDistance);

        //    // Convert polar coordinates to Cartesian
        //    float spawnX = player.position.x + randomDistance * Mathf.Cos(randomAngle * Mathf.Deg2Rad);
        //    float spawnZ = player.position.z + randomDistance * Mathf.Sin(randomAngle * Mathf.Deg2Rad);

        //    Vector3 spawnPosition = new Vector3(spawnX, player.position.y, spawnZ);
        //    Instantiate(enemy, spawnPosition, Quaternion.identity);
        //}
        cameraTransform = Camera.main.transform;
        SpawnWave(numEnemies);

     
    }

    // Update is called once per frame
    void Update()
    {
    // Clean up null (destroyed) enemies
    activeEnemies.RemoveAll(e => e == null);

    // If all enemies are dead and no wave is in progress, start timer for next wave
    if (activeEnemies.Count == 0 && waveInProgress)
    {
        SoundFXManager.instance.PlaySoundFXClip(victorySoundClip, transform, 0.5f);
        waveInProgress = false;
        waveTimer = timeBetweenWaves;
       
        SpawnReward();
    
    }

    // Countdown to next wave
    if (!waveInProgress)
    {
        waveTimer -= Time.deltaTime;
        if (waveTimer <= 0f)
        {
            currentWave++;
            int enemiesToSpawn = numEnemies + currentWave; // optional difficulty scaling
            SpawnWave(enemiesToSpawn);
            waveText.text = $"Wave {currentWave}/5";

        }
    }
    }

    void SpawnWave(int count)
    {   
    activeEnemies.Clear();
    waveInProgress = true;


    for (int i = 0; i < count; i++)
    {
        float spawnX = Random.Range(-20f, 20f);
        float spawnZ = Random.Range(100f, 127f);
        float spawnY = player.position.y;

        Vector3 spawnPosition = new Vector3(spawnX, spawnY, spawnZ);
        GameObject newEnemy = Instantiate(enemy, spawnPosition, Quaternion.identity);
        activeEnemies.Add(newEnemy);
    }
        float santaX = Random.Range(-20f, 20f);
        float santaZ = Random.Range(100f, 127f);
        float santaY = player.position.y;

        Vector3 santaPosition = new Vector3(santaX, santaY, santaZ);
        GameObject santa = Instantiate(santaEnemy, santaPosition, Quaternion.identity);
        activeEnemies.Add(santa);
    
    }
    void SpawnReward()
{
    Vector3 forwardOffset = cameraTransform.forward.normalized * 5f; // In front of camera
    Vector3 leftOffset = -cameraTransform.right.normalized * 1f;     // To the left of camera
    Vector3 spawnPosition = cameraTransform.position + forwardOffset + leftOffset;
    spawnPosition.y = player.position.y; // Keep it level with player (optional)

    rewardToSpawn = null;

    if (currentWave == 1)
    {
        rewardToSpawn = rewardPresentWave1Prefab;
    }
    else if (currentWave == 2)
    {
        rewardToSpawn = rewardPresentWave2Prefab;
    }

    if (rewardToSpawn != null)
    {
        StartCoroutine(ShowRewardMessage("You got a reward!", 3f));
        Instantiate(rewardToSpawn, spawnPosition, Quaternion.identity);
    }
}

    IEnumerator ShowRewardMessage(string message, float duration)
{
    rewardText.text = message;
    rewardText.gameObject.SetActive(true);
    yield return new WaitForSeconds(duration);
    rewardText.gameObject.SetActive(false);
}





}
