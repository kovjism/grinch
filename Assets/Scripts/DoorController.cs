//using UnityEngine;

//public class DoorController : MonoBehaviour
//{
//    // Start is called once before the first execution of Update after the MonoBehaviour is created
//    void Start()
//    {

//    }

//    // Update is called once per frame
//    void Update()
//    {

//    }
//}

using UnityEngine;

public class DoorController : MonoBehaviour
{
    public bool isOpen = false;
    public float openAngle = 90f;
    public float speed = 2f;
    private Quaternion closedRotation;
    private Quaternion openRotation;
    [SerializeField] private AudioClip doorSoundClip;

    void Start()
    {
        closedRotation = transform.rotation;
        openRotation = Quaternion.Euler(transform.eulerAngles + new Vector3(0, openAngle, 0));
    }
    
    void Update()
    {
        if (isOpen)
            transform.rotation = Quaternion.Lerp(transform.rotation, openRotation, Time.deltaTime * speed);
        else
            transform.rotation = Quaternion.Lerp(transform.rotation, closedRotation, Time.deltaTime * speed);
    }

    public void ToggleDoor()
    {
        SoundFXManager.instance.PlaySoundFXClip(doorSoundClip, transform, 0.2f);
        isOpen = !isOpen;
    }
}
