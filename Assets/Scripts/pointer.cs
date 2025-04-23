using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.TextCore.Text;
using UnityEngine.UI;
using static UnityEngine.UI.Image;

public class pointer : MonoBehaviour
{
    public GameObject menu;                 // settings menu
    public GameObject guns;
    public GameObject reticle;
    public GameObject crosshair;
    public GameObject gunPickupPrefab; // presents pickup
    public GameObject character;
    public LayerMask ground;            // plane (for teleport)

    public float distance;              // length of raycast
    private Outline lastObject;         // last object outlined

    private GameObject heldObject = null;
    private Transform grabAnchor; // an empty child of camera for holding objects

    private GameObject throwableObject = null;
    //public float throwStrength = 10000f;

    private shoot shootScript;

    private Vector3 offset = new Vector3(0f, -0.1f, 0f);    // offset to move pointer starting point below camera

    private LineRenderer lineRenderer;
    private bool showTrajectory = false;
    private int resolution = 30;
    private float timeStep = 0.1f;

    private bool grabbing = false;
    private Vector3 targetPoint;
    private Vector3 trajectoryVelocity;
    private float defaultThrow = 20f;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        distance = 50f;
        grabAnchor = new GameObject("GrabAnchor").transform;
        grabAnchor.SetParent(transform); // attach to pointer (camera probably)
        grabAnchor.localPosition = new Vector3(0f, 0f, 6f); // position 2m in front

        shootScript = guns.GetComponent<shoot>();
        // Get reference to the shoot script on the guns GameObject
        if (shootScript == null)
        {
            Debug.LogError("No shoot script found on guns GameObject!");
        }
        lineRenderer = GetComponent<LineRenderer>();      // get line renderer
        lineRenderer.startWidth = 0.2f;                  // set starting width
        lineRenderer.endWidth = 0.2f;                    // set ending width
    }

    // Update is called once per frame
    void Update()
    {
        //Vector2 aimInput = new Vector2(Input.GetAxis("RightStickHorizontal"), Input.GetAxis("RightStickVertical"));
        //targetPoint += transform.right * aimInput.x * 3f + transform.up * aimInput.y * 3f;
        if (showTrajectory)
        {
            targetPoint = GetTargetPoint();
            trajectoryVelocity = CalculateVelocity(throwableObject.transform.position, targetPoint, 1.5f);
            ShowTrajectory(throwableObject.transform.position, trajectoryVelocity);
        } else
        {
            HideTrajectory();
        }
        if (menu.GetComponent<menus>().open)        // if settings or inventory open, do not show pointer
        {
            if (lastObject != null)                                         // if there is a last object outlined
            {
                lastObject.enabled = false;                                 // disable outline for last object
            }
        } else
        {
            Vector3 start = transform.position + transform.TransformDirection(offset);          // start raycast below camera
            Vector3 end = start + transform.forward * distance;                                 // end raycast in direction faced at distance away

            Ray ray = new Ray(start, transform.forward);                                        // generate ray
            RaycastHit hit;                                                                     // get raycast hits

            if (Physics.Raycast(ray, out hit, distance))                                        // if raycast hits something
            {
                end = hit.point;
                if ((Input.GetButtonDown("js3") || Input.GetKeyDown("t")) && ((1 << hit.collider.gameObject.layer) & ground) != 0)    // if no menu is open, 'Y' is pressed and raycast hit is on ground, teleport
                {
                    character.GetComponent<CharacterController>().enabled = false;                                  // allow teleportation
                    character.transform.position = new Vector3(end.x, character.transform.position.y, end.z);       // set new location (keep y for same height)
                    character.GetComponent<CharacterController>().enabled = true;                                   // re-enable movement
                    start = transform.position + transform.TransformDirection(offset);                              // set new raycast starting point
                }
                // --- Door interaction ---
                if (Input.GetButtonDown("js2") || Input.GetKeyDown(KeyCode.E)) // Your shoot/interact button
                {
                    Transform hitTransform = hit.collider.transform;

                    // Check if the object (or its parent) has a DoorController script
                    DoorController door = hitTransform.GetComponentInParent<DoorController>();
                    if (door != null)
                    {
                        door.ToggleDoor(); // Open/close the door
                    }
                }

                // --- Lamp interaction ---
                if (Input.GetButtonDown("js2") || Input.GetKeyDown(KeyCode.E)) // Or same key as door
                {
                    Transform hitTransform = hit.collider.transform;
                    LampToggle lamp = hitTransform.GetComponentInParent<LampToggle>();
                    if (lamp != null)
                    {
                        lamp.ToggleLamp();
                    }
                }

                // --- Throwable object pickup ---
                if (Input.GetButtonDown("js2") || Input.GetKeyDown(KeyCode.E)) // T to pick up/throw
                {
                    if (grabbing)
                    {
                        HideTrajectory();
                        showTrajectory = false;
                        trajectoryVelocity = CalculateVelocity(throwableObject.transform.position, end, 1.5f);
                        // Throw
                        throwableObject.transform.SetParent(null);
                        Rigidbody rb = throwableObject.GetComponent<Rigidbody>();
                        rb.isKinematic = false;
                        //rb.AddForce(transform.forward * throwStrength, ForceMode.VelocityChange);
                        //rb.AddForce(throwableObject.transform.forward * throwStrength, ForceMode.Impulse);
                        rb.linearVelocity = trajectoryVelocity;
                        //rb.linearVelocity = transform.forward * throwStrength;

                        ThrowableDamage dmgScript = throwableObject.GetComponent<ThrowableDamage>();
                        if (dmgScript != null)
                        {
                            dmgScript.SetTransparency(false);  // Make opaque when thrown
                        }

                        throwableObject = null;

                        // Reequip the gun after throwing
                        if (guns != null)
                        {
                            guns.SetActive(true);
                        }
                        grabbing = false;
                    } else
                    {
                        Transform hitTransform = hit.collider.transform;

                        if (hitTransform.CompareTag("Throwable"))
                        {
                            if (throwableObject == null)
                            {
                                // Pick up (attach to camera or hand)
                                throwableObject = hitTransform.gameObject;
                                Rigidbody rb = throwableObject.GetComponent<Rigidbody>();
                                rb.isKinematic = true;
                                throwableObject.transform.SetParent(transform); // Attach to pointer/camera
                                throwableObject.transform.localPosition = new Vector3(1, -0.5f, 2); // Position in front of player
                                trajectoryVelocity = CalculateVelocity(throwableObject.transform.position, end, 1.5f);
                                ShowTrajectory(throwableObject.transform.position, trajectoryVelocity);
                                showTrajectory = true;

                                // Reset hasHit so it can damage again
                                ThrowableDamage dmgScript = throwableObject.GetComponent<ThrowableDamage>();
                                if (dmgScript != null)
                                {
                                    dmgScript.ResetHit();
                                    dmgScript.SetTransparency(true);  // Make transparent when picked up
                                }

                                // Unequip the gun when picking up throwable
                                if (guns != null)
                                {
                                    guns.SetActive(false);
                                }
                                grabbing = true;
                            }
                        }
                    }
                }

                // --- Gun pickup interaction ---
                if (Input.GetButtonDown("js2") || Input.GetKeyDown(KeyCode.E)) // E to pick up gun
                {
                    Transform hitTransform = hit.collider.transform;
                    PresentOpener present = hitTransform.GetComponent<PresentOpener>();
                    if (present != null)
                    {
                        present.Open();
                        return;
                    }
                    GunPickup gunPickup = hitTransform.GetComponent<GunPickup>();

                    if (gunPickup != null && gunPickup.canBePickedUp)
                    {
                        // Call the method on shoot script to add the gun
                        if (shootScript.AddGun(gunPickup.gunPrefab))
                        {
                            // Destroy the pickup object after successful pickup
                            Destroy(hitTransform.gameObject);
                            Debug.Log("Picked up " + gunPickup.gunType);
                        }
                    }
                }


                Outline outline = hit.collider.GetComponent<Outline>();         // get outline component

                if (lastObject != null && outline != lastObject)                // if new object is hovered, remove old outline
                {
                    lastObject.enabled = false;
                }
                if (outline != null)                                            // if outline script exists, enable outline 
                {
                    outline.enabled = true;
                    lastObject = outline;
                }

                // === Grabbing Logic ===
                if (Input.GetButtonDown("js2") || Input.GetKeyDown(KeyCode.E))
                {
                    if (heldObject == null)
                    {
                        // Try to grab object
                        if (hit.collider.CompareTag("Grabbable")) // or use layer check
                        {
                            heldObject = hit.collider.gameObject;
                            heldObject.transform.SetParent(grabAnchor);
                            heldObject.transform.localPosition = Vector3.zero;
                            heldObject.GetComponent<Rigidbody>().isKinematic = true;
                            GyroManager.Instance.enableFollowGyro(heldObject);
                        }
                    }
                    else
                    {
                        // Release object
                        heldObject.transform.SetParent(null);
                        GyroManager.Instance.disableFollowGyro(heldObject);
                        Rigidbody rb = heldObject.GetComponent<Rigidbody>();
                        if (rb != null)
                            rb.isKinematic = false;

                        heldObject = null;
                    }
                }
            }
            else
            {
                if (lastObject != null)                 // if there is last object hovered
                {
                    lastObject.enabled = false;         // remove outline
                }
                if (grabbing && throwableObject != null && (Input.GetButtonDown("js2") || Input.GetKeyDown(KeyCode.E)))
                {
                    HideTrajectory();
                    showTrajectory = false;
                    targetPoint = GetTargetPoint();
                    trajectoryVelocity = CalculateVelocity(throwableObject.transform.position, targetPoint, 1.5f);
                    // Throw
                    throwableObject.transform.SetParent(null);
                    Rigidbody rb = throwableObject.GetComponent<Rigidbody>();
                    rb.isKinematic = false;
                    //rb.AddForce(transform.forward * throwStrength, ForceMode.VelocityChange);
                    //rb.AddForce(throwableObject.transform.forward * throwStrength, ForceMode.Impulse);
                    rb.linearVelocity = trajectoryVelocity;
                    //rb.linearVelocity = transform.forward * throwStrength;

                    ThrowableDamage dmgScript = throwableObject.GetComponent<ThrowableDamage>();
                    if (dmgScript != null)
                    {
                        dmgScript.SetTransparency(false);  // Make opaque when thrown
                    }

                    throwableObject = null;

                    // Reequip the gun after throwing
                    if (guns != null)
                    {
                        guns.SetActive(true);
                    }
                    grabbing = false;
                }
            }
            
        }
    }
    public static Vector3 CalculateVelocity(Vector3 origin, Vector3 target, float arcHeight)
    {
        arcHeight = Mathf.Clamp(arcHeight, 0.1f, (target - origin).magnitude * 0.5f);
        Vector3 direction = target - origin;
        Vector3 horizontal = new Vector3(direction.x, 0f, direction.z);
        float distance = horizontal.magnitude;

        float yOffset = direction.y;

        float initialY = Mathf.Sqrt(-2f * Physics.gravity.y * arcHeight);
        float timeUp = initialY / -Physics.gravity.y;
        float fallHeight = Mathf.Max(0.1f, yOffset - arcHeight); // Prevent negative
        float timeDown = Mathf.Sqrt(2f * fallHeight / -Physics.gravity.y);
        float totalTime = timeUp + timeDown;

        Vector3 velocity = horizontal / totalTime;
        velocity.y = initialY;

        return velocity;
    }

    public void ShowTrajectory(Vector3 startPos, Vector3 startVelocity)
    {
        lineRenderer.positionCount = resolution;

        for (int i = 0; i < resolution; i++)
        {
            float time = i * timeStep;
            Vector3 pos = startPos + startVelocity * time + 0.5f * Physics.gravity * time * time;
            lineRenderer.SetPosition(i, pos);

            // Optional: break on collision
            if (i > 0 && Physics.Raycast(lineRenderer.GetPosition(i - 1), pos - lineRenderer.GetPosition(i - 1), out RaycastHit hit, Vector3.Distance(pos, lineRenderer.GetPosition(i - 1))))
            {
                lineRenderer.positionCount = i + 1;
                lineRenderer.SetPosition(i, hit.point);
                break;
            }
        }
    }

    public void HideTrajectory()
    {
        lineRenderer.positionCount = 0;
    }

    private Vector3 GetTargetPoint()
    {
        Vector3 origin = transform.position + transform.TransformDirection(offset);
        Ray ray = new Ray(origin, transform.forward);
        if (Physics.Raycast(ray, out RaycastHit hit, distance, ~0))
        {
            return hit.point;
        }
        return origin + transform.forward * defaultThrow;
    }
}