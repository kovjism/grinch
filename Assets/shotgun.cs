using UnityEngine;
using System.Collections;

public class shotgun : gun
{
    private int shots = 8;
    private float spread = 5f;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        fireRate = 1f;
        damage = 1;
        recoilAngle = 7f;
        recoilSpeed = 50f;
        resetSpeed = 5f;
        recoilDistance = 1f;
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

        for (int i = 0; i < shots; i++)
        {
            GameObject bullet = Instantiate(bulletPrefab, muzzle.position, muzzle.rotation);
            Rigidbody rb = bullet.GetComponent<Rigidbody>();
            bullet.GetComponent<bullet>().SetDamage(damage);

            float spreadX = Random.Range(-spread, spread);
            float spreadY = Random.Range(-spread, spread);

            Vector3 direction = Quaternion.Euler(spreadY, spreadX, 0) * cameraObject.transform.forward;
            rb.linearVelocity = direction * bulletSpeed;
        }

        StartCoroutine(Recoil());
    }

    public override void Equip()
    {
        transform.localRotation = originalRotation;
    }

    IEnumerator Recoil()
    {
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
    }
}

