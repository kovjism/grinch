using UnityEngine;

public class Teleportation : MonoBehaviour
{
    public Transform player; // assign Character object
    public float heightOffset = 1f; // set height to avoid sinking into the ground after teleporting

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetButtonDown("js3"))     // js3 for android
        {
            Ray ray = new Ray(Camera.main.transform.position, Camera.main.transform.forward);
            RaycastHit hit;

            if (Physics.Raycast(ray, out hit))
            {
                if (hit.collider.CompareTag("Floor"))
                {
                    player.GetComponent<CharacterController>().enabled = false;
                    player.position = hit.point + Vector3.up * heightOffset;
                    player.GetComponent<CharacterController>().enabled = true;
                }
            }
        }
    }
}
