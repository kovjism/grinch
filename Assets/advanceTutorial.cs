using UnityEngine;

public class advanceTutorial : MonoBehaviour
{
    public tutorialManager tutorialManager; // Assign your tutorial controller

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            tutorialManager.tutorialStage += 1;
            Destroy(gameObject); // Optional: Remove trigger after use
        }
    }
}
