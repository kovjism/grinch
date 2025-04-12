using Unity.VisualScripting;
using UnityEngine;

public class GyroManager : MonoBehaviour
{
    private static GyroManager instance;
    public static GyroManager Instance
    {
        get
        {
            if (instance == null)
            {
                instance = Object.FindFirstObjectByType<GyroManager>();
                {
                    if (instance == null)
                    {
                        instance = new GameObject("Spawned GyroManager", typeof(GyroManager)).GetComponent<GyroManager>();
                    }
                }
            }
            return instance;
        }
        set
        {
            instance = value;
        }
    }
    private Gyroscope gyro;
    private Quaternion rotation;
    private bool gyroIsActive;

    public void enableGyro()
    {
        if (gyroIsActive)
            return;
        if (SystemInfo.supportsGyroscope)
        {
            gyro = Input.gyro;
            gyro.enabled = true;
            gyroIsActive = gyro.enabled;
        }
    }
    public void enableFollowGyro(GameObject parent)
    {
        parent.AddComponent<followGyro>();
    }
    public void disableFollowGyro(GameObject parent)
    {
        Destroy(parent.GetComponent<followGyro>());
    }
    public Quaternion getGyroRotation()
    { return rotation; }
}
