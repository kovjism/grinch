using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class pistol : gun
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    [SerializeField] private AudioClip firingSoundClip;
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

        //play firing sound
        SoundFXManager.instance.PlaySoundFXClip(firingSoundClip, transform, 1f);

        nextFire = Time.time + fireRate;
        RaycastHit hit;
        Ray ray = new Ray(cameraObject.transform.position, cameraObject.transform.forward);

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
}
