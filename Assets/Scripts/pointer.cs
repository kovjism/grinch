using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class pointer : MonoBehaviour
{
    public Canvas menu;                 // settings menu
    public GameObject guns;
    public GameObject reticle;
    public GameObject crosshair;

    public float distance;              // length of raycast
    public Vector3 rayOrigin;           // start of raycast (for grabbing)
    public Vector3 rayDirection;        // raycast direction (for grabbing)

    private LineRenderer lr;            // line renderer (to show pointer)
    private Outline lastObject;         // last object outlined
    private GameObject lastHoveredUI;   // last button hovered

    private Vector3 offset = new Vector3(0f, -0.1f, 0f);    // offset to move pointer starting point below camera

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        distance = 50f;
        lr = GetComponent<LineRenderer>();      // get line renderer
        lr.positionCount = 2;                   // set line renderer to have 2 points
        lr.startWidth = 0.01f;                  // set starting width
        lr.endWidth = 0.05f;                    // set ending width
        lr.enabled = true;                      // show line
    }

    // Update is called once per frame
    void Update()
    {
        if (menu.isActiveAndEnabled || guns.activeSelf)        // if settings or inventory open, do not show pointer
        {
            lr.enabled = false;                                             // hide line
            if (lastObject != null)                                         // if there is a last object outlined
            {
                lastObject.enabled = false;                                 // disable outline for last object
            }
            if (Input.GetKeyDown(KeyCode.F))
            {
                guns.SetActive(false);
                reticle.SetActive(false);
                crosshair.SetActive(false);
            }
        } else
        {
            if (Input.GetKeyDown(KeyCode.F))
            {
                guns.SetActive(true);
                reticle.SetActive(true);
                crosshair.SetActive(true);
            }

            lr.enabled = true;                                                                  // show line
            Vector3 start = transform.position + transform.TransformDirection(offset);          // start raycast below camera
            Vector3 end = start + transform.forward * distance;                                 // end raycast in direction faced at distance away

            Ray ray = new Ray(start, transform.forward);                                        // generate ray
            RaycastHit hit;                                                                     // get raycast hits

            if (Physics.Raycast(ray, out hit, distance))                                        // if raycast hits something
            {
                end = hit.point;

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
            }
            else
            {
                if (lastObject != null)                 // if there is last object hovered
                {
                    lastObject.enabled = false;         // remove outline
                }
            }

            // variables for grabbing logic
            rayOrigin = start;                          
            rayDirection = transform.forward;

            // display raycast
            lr.SetPosition(0, start);                   
            lr.SetPosition(1, end);


            // for button selection with pointer
            PointerEventData pointerData = new PointerEventData(EventSystem.current);       // gets pointer data
            pointerData.position = Camera.main.WorldToScreenPoint(end);                     // gets where pointer is pointing

            List<RaycastResult> uiHits = new List<RaycastResult>();                         // list for hits
            EventSystem.current.RaycastAll(pointerData, uiHits);                            // gets raycast hits

            bool hoveredUI = false;

            foreach (RaycastResult result in uiHits)                                        // for each hit, do hover and select logic
            {
                GameObject target = result.gameObject;                                      

                if (target.GetComponent<Selectable>() != null)
                {
                    hoveredUI = true;
                    // Highlight (hover)
                    if (lastHoveredUI != target)
                    {
                        if (lastHoveredUI != null)
                            ExecuteEvents.Execute(lastHoveredUI, pointerData, ExecuteEvents.pointerExitHandler);

                        ExecuteEvents.Execute(target, pointerData, ExecuteEvents.pointerEnterHandler);
                        lastHoveredUI = target;
                    }

                    // Click
                    if (Input.GetButtonDown("js5") || Input.GetKeyDown("q"))
                    {
                        Button btn = target.GetComponent<Button>();
                        if (btn != null)
                            btn.onClick.Invoke();
                    }

                    break; // handle only first hit
                }
            }
            if (!hoveredUI && lastHoveredUI != null)
            {
                ExecuteEvents.Execute(lastHoveredUI, pointerData, ExecuteEvents.pointerExitHandler);
                lastHoveredUI = null;
            }

            //
        }
    }
}