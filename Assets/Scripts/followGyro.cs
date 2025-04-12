using UnityEngine;

public class followGyro : MonoBehaviour
{
    [SerializeField] private Quaternion baseRotation = new Quaternion(0, 0, 1, 0);
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        GyroManager.Instance.enableGyro();
    }

    // Update is called once per frame
    void Update()
    {
        transform.localRotation = GyroManager.Instance.getGyroRotation() * baseRotation;
    }
}
