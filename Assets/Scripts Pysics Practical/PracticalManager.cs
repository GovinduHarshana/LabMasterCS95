using UnityEngine;
using UnityEngine.UI;

public class PracticalManager : MonoBehaviour
{
    public Toggle paperRiderToggle; // Reference to the Paper Rider Position Toggle
    public Toggle tuningForkToggle; // Reference to the Tuning Fork Touch Sonometer Toggle
    public Toggle allConditionsToggle; // Reference to the Toggle for all conditions
    public AudioSource sonometerAudioSource; // Reference to the Sonometer AudioSource
    public WeightHanger weightHanger; // Reference to the WeightHanger script
    public MovableBridge movableBridge; // Reference to the MovableBridge script

    // Define 3 sets of correct values (length in cm, weight in kg)
    private readonly (float length, float weight)[] correctValues = new (float, float)[]
    {
        (30f, 1.5f), // Example: 30 cm length, 1.5 kg weight
        (40f, 2.0f), // Example: 40 cm length, 2.0 kg weight
        (50f, 2.5f)  // Example: 50 cm length, 2.5 kg weight
    };

    public GameObject paperRider; // Reference to the Paper Rider object
    public float animationDuration = 1f; // Duration of the paper rider removal animation

    private void Update()
    {
        // Check all conditions
        bool weightCondition = weightHanger.totalWeight > 0f;
        bool paperRiderCondition = paperRiderToggle.isOn;
        bool soundCondition = sonometerAudioSource.isPlaying;
        bool tuningForkCondition = tuningForkToggle.isOn;

        // Update the All Conditions Toggle
        if (weightCondition && paperRiderCondition && soundCondition && tuningForkCondition)
        {
            allConditionsToggle.isOn = true;

            // Check if the current length and weight match any of the correct values
            CheckForCorrectValues();
        }
        else
        {
            allConditionsToggle.isOn = false;
        }
    }

    private void CheckForCorrectValues()
    {
        float currentLength = movableBridge.GetCurrentLength(); // Get current length from MovableBridge
        float currentWeight = weightHanger.totalWeight; // Get current weight from WeightHanger

        // Check if the current values match any of the correct values
        foreach (var value in correctValues)
        {
            if (Mathf.Abs(currentLength - value.length) < 1f && Mathf.Abs(currentWeight - value.weight) < 0.1f)
            {
                // Trigger the paper rider removal animation
                StartCoroutine(RemovePaperRider());
                break;
            }
        }
    }

    private System.Collections.IEnumerator RemovePaperRider()
    {
        // Simple animation: Move the paper rider downward
        Vector3 startPosition = paperRider.transform.position;
        Vector3 endPosition = startPosition + Vector3.down * 2f; // Move downward by 2 units

        float elapsedTime = 0f;
        while (elapsedTime < animationDuration)
        {
            paperRider.transform.position = Vector3.Lerp(startPosition, endPosition, elapsedTime / animationDuration);
            elapsedTime += Time.deltaTime;
            yield return null;
        }

        // Disable the paper rider after the animation
        paperRider.SetActive(false);
    }
}