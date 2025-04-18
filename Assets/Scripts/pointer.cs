using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.TextCore.Text;
using UnityEngine.UI;

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
    public float throwStrength = 10000f;

    private shoot shootScript;

    private Vector3 offset = new Vector3(0f, -0.1f, 0f);    // offset to move pointer starting point below camera

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
    }

    // Update is called once per frame
    void Update()
    {
        if (menu.GetComponent<menus>().open)        // if settings or inventory open, do not show pointer
        {
            if (lastObject != null)                                         // if there is a last object outlined
            {
                lastObject.enabled = false;                                 // disable outline for last object
            }
        } else
        {                                           // show line
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
                            throwableObject.transform.localPosition = new Vector3(0, 0, 3); // Position in front of player
                        }
                        else
                        {
                            // Throw
                            throwableObject.transform.SetParent(null);
                            Rigidbody rb = throwableObject.GetComponent<Rigidbody>();
                            rb.isKinematic = false;
                            //rb.AddForce(transform.forward * throwStrength, ForceMode.VelocityChange);
                            rb.AddForce(transform.forward * throwStrength, ForceMode.Impulse);
                            rb.linearVelocity = transform.forward * throwStrength;
                            throwableObject = null;
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
            }
        }
    }

}