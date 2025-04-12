using UnityEngine;

public class PresentOpener : MonoBehaviour
{
    public GameObject gunPickupPrefab; 
    [SerializeField] private AudioClip giftSoundClip;
    private bool isOpened = false;

    public void Open()
    {
        SoundFXManager.instance.PlaySoundFXClip(giftSoundClip, transform, 0.2f);

        if (isOpened) return;

        isOpened = true;
        // Spawn the gun at the present's position
        Instantiate(gunPickupPrefab, transform.position, transform.rotation);

        // Destroy the present
        Destroy(gameObject);
    }
}
