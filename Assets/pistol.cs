using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class pistol : gun
{
    private bool recoiling = false;
    
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        fireRate = 0.5f;
        damage = 3;
        recoilAngle = 5f;
        recoilSpeed = 50f;
        resetSpeed = 5f;
        recoilDistance = 0.1f;
        originalRotation = transform.localRotation;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public override void Shoot()
    {
        if (Time.time < nextFire) return;
        
        nextFire = Time.time + fireRate;
        RaycastHit hit;
        Ray ray = cameraObject.ScreenPointToRay(new Vector3(Screen.width / 2, Screen.height / 2, 0));

        Vector3 targetPoint = ray.origin + ray.direction * maxDistance;
        GameObject bullet = Instantiate(bulletPrefab, muzzle.position, muzzle.rotation);
        bullet.GetComponent<bullet>().SetDamage(damage);
        Rigidbody rb = bullet.GetComponent<Rigidbody>();

        if (Physics.Raycast(ray, out hit, maxDistance, hitLayers))
        {
            targetPoint = hit.point;
        }
        Vector3 direction = (targetPoint - muzzle.position).normalized;
        rb.linearVelocity = direction * bulletSpeed;

        StartCoroutine(Recoil());
    }
    public override void Equip()
    {
        transform.localRotation = originalRotation;
    }

    IEnumerator Recoil()
    {
        recoiling = true;

        // Tilt the weapon back
        float elapsedTime = 0;

        while (elapsedTime < (1f / recoilSpeed))
        {
            transform.RotateAround(cameraObject.transform.position, cameraObject.transform.right, -recoilAngle * Time.deltaTime * recoilSpeed);
            transform.position -= cameraObject.transform.forward * (recoilDistance * Time.deltaTime * recoilSpeed);
            elapsedTime += Time.deltaTime;
            yield return null;
        }

        elapsedTime = 0;
        while (elapsedTime < (1f / resetSpeed))
        {
            transform.RotateAround(cameraObject.transform.position, cameraObject.transform.right, recoilAngle * Time.deltaTime * resetSpeed);
            transform.position += cameraObject.transform.forward * (recoilDistance * Time.deltaTime * resetSpeed);
            elapsedTime += Time.deltaTime;
            yield return null;
        }

        recoiling = false;
    }
}
