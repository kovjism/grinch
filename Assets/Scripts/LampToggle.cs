//using UnityEngine;

//public class LampToggle : MonoBehaviour
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

public class LampToggle : MonoBehaviour
{
    public Light lampLight; // Assign this in the Inspector

    private bool isOn = true;

    void Start()
    {
        lampLight.enabled = isOn;
    }

    public void ToggleLamp()
    {
        isOn = !isOn;
        lampLight.enabled = isOn;
    }
}
