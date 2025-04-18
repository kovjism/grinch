using NUnit.Framework;
using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;

public class PresentOpener : MonoBehaviour
{
    public GameObject gunPickupPrefab; 
    [SerializeField] private AudioClip giftSoundClip;
    private bool isOpened = false;

    private List<Color> colors = new List<Color> {
        Color.white,
        new Color32(255, 215, 0, 255),   // Gold
        new Color32(30, 144, 255, 255)   // Dodger Blue
    };

    public void Open()
    {
        SoundFXManager.instance.PlaySoundFXClip(giftSoundClip, transform, 0.2f);

        if (isOpened) return;

        isOpened = true;
        // Spawn the gun at the present's position
        Instantiate(gunPickupPrefab, transform.position, transform.rotation);

        Outline outline = GetComponent<Outline>();
        if (outline == null)
        {
            outline = gameObject.AddComponent<Outline>();
        }

        outline.OutlineWidth = 5f;
        outline.OutlineColor = Color.Lerp(colors[PlayerPrefs.GetInt("UIContrastMode", 0)], Color.red, 0.5f);
        outline.OutlineMode = Outline.Mode.OutlineVisible;
        outline.enabled = false;

        // Make sure it has a collider for raycast detection
        if (GetComponent<Collider>() == null)
        {
            BoxCollider collider = gameObject.AddComponent<BoxCollider>();
            collider.size = new Vector3(0.3f, 0.2f, 1f); // Adjust size to match gun
        }

        // Destroy the present
        Destroy(gameObject);
    }
}
