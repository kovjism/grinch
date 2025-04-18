using UnityEngine;

public class GunPickup : MonoBehaviour
{
    // Reference to the gun prefab that should be added to player's inventory
    public GameObject gunPrefab;

    // Gun type information - useful if you want to check what type of gun it is
    public string gunType;

    // Optional - if you want to disable pickup until certain conditions are met
    public bool canBePickedUp = true;

    // Optional visual effect for when gun is highlighted
    private Outline outline;

    void Start()
    {
        // Add outline component if it doesn't exist
        outline = GetComponent<Outline>();
        if (outline == null)
        {
            outline = gameObject.AddComponent<Outline>();
            outline.OutlineWidth = 5f;
            outline.OutlineColor = Color.yellow;
            outline.enabled = false;
        }

        // Make sure it has a collider for raycast detection
        if (GetComponent<Collider>() == null)
        {
            BoxCollider collider = gameObject.AddComponent<BoxCollider>();
            collider.size = new Vector3(0.3f, 0.2f, 1f); // Adjust size to match gun
        }
    }
}