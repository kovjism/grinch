using UnityEngine;
using System.Collections;

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

    public IEnumerator Recoil()
    {
        Vector3 startPos = transform.localPosition;
        Quaternion startRot = transform.localRotation;

        Vector3 recoilDirection = transform.forward; // Cache the forward vector before rotation!

        float elapsedTime = 0f;
        float recoilDuration = 1f / recoilSpeed;

        // Recoil back
        while (elapsedTime < recoilDuration)
        {
            float step = Time.deltaTime * recoilSpeed;
            transform.Rotate(Vector3.right, -recoilAngle * step, Space.Self);
            transform.localPosition -= recoilDirection * recoilDistance * step;
            elapsedTime += Time.deltaTime;
            yield return null;
        }

        elapsedTime = 0f;
        float resetDuration = 1f / resetSpeed;

        // Reset forward
        while (elapsedTime < resetDuration)
        {
            float step = Time.deltaTime * resetSpeed;
            transform.Rotate(Vector3.right, recoilAngle * step, Space.Self);
            transform.localPosition += recoilDirection * recoilDistance * step;
            elapsedTime += Time.deltaTime;
            yield return null;
        }

        // Snap back to exact original just to clean up float errors
        transform.localPosition = startPos;
        transform.localRotation = startRot;
    }
}
