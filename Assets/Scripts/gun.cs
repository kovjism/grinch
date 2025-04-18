using UnityEngine;
using System.Collections;

public abstract class gun : MonoBehaviour
{
    public GameObject bulletPrefab;
    public Transform muzzle;
    public Sprite icon;
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

        float elapsedTime = 0f;
        float recoilDuration = 1f / recoilSpeed;

        // Get recoil direction in local space
        Vector3 localRecoilDir = transform.InverseTransformDirection(cameraObject.transform.forward);

        while (elapsedTime < recoilDuration)
        {
            float step = Time.deltaTime * recoilSpeed;
            transform.localRotation *= Quaternion.Euler(-recoilAngle * step, 0f, 0f);
            transform.localPosition -= localRecoilDir * recoilDistance * step;
            elapsedTime += Time.deltaTime;
            yield return null;
        }

        elapsedTime = 0f;
        float resetDuration = 1f / resetSpeed;

        while (elapsedTime < resetDuration)
        {
            float t = elapsedTime / resetDuration;
            transform.localPosition = Vector3.Lerp(transform.localPosition, startPos, t);
            transform.localRotation = Quaternion.Slerp(transform.localRotation, startRot, t);
            elapsedTime += Time.deltaTime;
            yield return null;
        }

        transform.localPosition = startPos;
        transform.localRotation = startRot;
    }
}
