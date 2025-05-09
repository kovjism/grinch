using UnityEngine;
using System.Collections;

public class shotgun : gun
{
    private int shots = 8;
    private float spread = 5f;
    [SerializeField] private AudioClip firingSoundClip;

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

        //play firing sound
        SoundFXManager.instance.PlaySoundFXClip(firingSoundClip, transform, 1f);

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

        transform.localPosition = new Vector3(0.4f, -0.4f, 0.6f); // ← tweak these to fit your view

    }
}

