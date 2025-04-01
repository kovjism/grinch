using UnityEngine;

public abstract class gun : MonoBehaviour
{
    public GameObject bulletPrefab;
    public Transform muzzle;
    public Camera cameraObject;
    public float bulletSpeed;
    public float maxDistance = 100f;
    public float nextFire = 0f;
    public LayerMask hitLayers;

    public int damage;
    public float fireRate;
    public float recoilAngle; 
    public float recoilSpeed; 
    public float resetSpeed;
    public float recoilDistance;
    public Quaternion originalRotation;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    
    public abstract void Shoot();
    public abstract void Equip();
}
