using UnityEngine;

public class PresentOpener : MonoBehaviour
{
    public GameObject gunPickupPrefab; 

    private bool isOpened = false;

    public void Open()
    {
        if (isOpened) return;

        isOpened = true;

        // Spawn the gun at the present's position
        Instantiate(gunPickupPrefab, transform.position, transform.rotation);

        // Destroy the present
        Destroy(gameObject);
    }
}
