using TMPro;
using UnityEngine;

public class Player_Status : MonoBehaviour
{
    [SerializeField] private GameObject grinchCanvas;
    private TextMeshProUGUI rageModeText;
    private int rageStatus;
    void Start()
    {
        Input.gyro.enabled = true;
        rageModeText = grinchCanvas.GetComponentInChildren<TextMeshProUGUI>();
        rageStatus = 0;
    }
    void Update()
    {
        //add 10 second delay per check function call
        checkForRage(rageStatus);
    }
    private void checkForRage(int currentRage)
    {
        //add check for phone movement
        if (currentRage == 0)
        {
            grinchCanvas.SetActive(true);
            Invoke("hideGrinchCanvas", 2);
            rageStatus++;
            //try instantiate (new object)
            //destroy(object, 2 [seconds?])
        }
        else if (currentRage == 1)
        {
            rageModeText.text = "Rage Mode Charging...\n(Tilt Left to Continue)";
            grinchCanvas.SetActive(true);
            Invoke("hideGrinchCanvas", 2);
            rageStatus++;
        }
        else if (currentRage == 2)
        {
            rageModeText.text = "Rage Mode Charging...\n(Tilt Up to Activate)";
            grinchCanvas.SetActive(true);
            Invoke("hideGrinchCanvas", 2);
            rageStatus++;
        }
        else if (currentRage == 3)
        {
            rageModeText.text = "Rage Mode Activated!";
            grinchCanvas.SetActive(true);
            Invoke("hideGrinchCanvas", 2);
            rageStatus = 0;
        }
    }
}
