using System.Collections;
using UnityEngine;

public class railgun : gun
{
    public LineRenderer laser;
    public float laserDuration = 0.1f;
    [SerializeField] private AudioClip firingSoundClip;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        fireRate = 1.5f;
        damage = 5;
        recoilAngle = 10f;
        recoilSpeed = 50f;
        resetSpeed = 5f;
        recoilDistance = 0.5f;
        maxDistance = 200f;
        originalRotation = transform.localRotation;
        laser.enabled = false;
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

        StartCoroutine(Laser());
        StartCoroutine(Recoil());
    }

    public override void Equip()
    {
        transform.localRotation = originalRotation;
    }

    IEnumerator Laser()
    {
        laser.enabled = true;
        laser.SetPosition(0, muzzle.position);

        // Get the direction from crosshair
        Ray ray = new Ray(cameraObject.transform.position, cameraObject.transform.forward);
        RaycastHit[] hits = Physics.RaycastAll(ray, maxDistance, hitLayers);

        System.Array.Sort(hits, (a, b) => a.distance.CompareTo(b.distance));

        Vector3 laserEndPoint = muzzle.position + cameraObject.transform.forward * maxDistance;

        // Process all objects hit
        foreach (RaycastHit hit in hits)
        {
            if (hit.collider.CompareTag("Enemy"))
            {
                enemy enemyScript = hit.collider.GetComponent<enemy>();
                enemyScript.takeDamage(damage);
            }
            laserEndPoint = hit.point;
        }
        laser.SetPosition(1, laserEndPoint);
        yield return new WaitForSeconds(laserDuration);
        laser.enabled = false;
    }
}
