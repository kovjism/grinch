using System.Collections;
using UnityEngine;

public class railgun : gun
{
    public LineRenderer laser;
    public float laserDuration = 0.1f;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        fireRate = 1.5f;
        damage = 5;
        recoilAngle = 10f;
        recoilSpeed = 50f;
        resetSpeed = 5f;
        recoilDistance = 1f;
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
        Ray ray = cameraObject.ScreenPointToRay(new Vector3(Screen.width / 2, Screen.height / 2, 0));
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
            else
            {
                laserEndPoint = hit.point;
                break;
            }
        }
        laser.SetPosition(1, laserEndPoint);
        yield return new WaitForSeconds(laserDuration);
        laser.enabled = false;
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
