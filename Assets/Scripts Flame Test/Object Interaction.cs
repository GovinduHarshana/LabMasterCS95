using UnityEngine;

public class ObjectInteractions : MonoBehaviour
{
    private PracticalSteps practicalSteps;

    void Start()
    {
        practicalSteps = FindFirstObjectByType<PracticalSteps>();
    }

    public void OnObjectInteraction()
    {
        if (practicalSteps != null)
        {
            Debug.Log("Calling NextStep()...");
            practicalSteps.NextStep();  // Calls NextStep() on interaction
        }
        else
        {
            Debug.LogError("PracticalSteps script not found!");
        }
    }
}
