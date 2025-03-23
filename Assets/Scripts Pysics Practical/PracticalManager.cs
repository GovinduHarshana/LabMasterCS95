using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class PracticalManager : MonoBehaviour
{
    public Toggle paperRiderToggle; 
    public Toggle tuningForkToggle;
    public Toggle allConditionsToggle;
    public AudioSource sonometerAudioSource; // Reference to the Sonometer AudioSource
    public WeightHanger weightHanger; // Reference to the WeightHanger script
    public MovableBridge movableBridge; // Reference to the MovableBridge script

    // Define 3 sets of correct values (length in cm, weight in kg)
    private readonly (float length, float weight)[] correctValues = new (float, float)[]
    {
        (30f, 0.5f), // Example: 30 cm length, 0.5 kg weight
        (40f, 1.0f), // Example: 40 cm length, 1.0 kg weight
        (50f, 1.5f)  // Example: 50 cm length, 1.5 kg weight
    };

    public GameObject paperRider;
    public Transform table; // Reference to the table's Transform
    public float animationDuration = 1f; // Duration of the paper rider removal animation

    public GameObject popupPanel;
    public TextMeshProUGUI popupText;
    public Button addValuesButton;
    public Button closeButton;

    public Image statusCircle; // Reference to the Status Circle Image
    public TextMeshProUGUI statusText; // Reference to the Status Text

    public TextMeshProUGUI[] lengthTexts; // References to the Length Text fields in the table
    public TextMeshProUGUI[] weightTexts; // References to the Weight Text fields in the table

    private int currentSetIndex = 0; // Track which set of values is being filled

    private void Start()
    {
        // Hide the popup panel at the start
        popupPanel.SetActive(false);

        // Add a listener to the Add Values button
        addValuesButton.onClick.AddListener(AddValuesToTable);

        // Add a listener to the Close Button
        closeButton.onClick.AddListener(ClosePopup);

        // Initialize the status UI
        UpdateStatusUI();
    }


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
                StartCoroutine(MovePaperRiderToTable(currentLength, currentWeight));
                break;
            }
        }
    }

    private System.Collections.IEnumerator MovePaperRiderToTable(float length, float weight)
    {
        // Get the starting position of the paper rider
        Vector3 startPosition = paperRider.transform.position;

        // Get the target position (table's position)
        Vector3 endPosition = new Vector3(startPosition.x, table.position.y, startPosition.z);

        // Animate the paper rider moving downward
        float elapsedTime = 0f;
        while (elapsedTime < animationDuration)
        {
            paperRider.transform.position = Vector3.Lerp(startPosition, endPosition, elapsedTime / animationDuration);
            elapsedTime += Time.deltaTime;
            yield return null;
        }

        // Ensure the paper rider is exactly at the table's position
        paperRider.transform.position = endPosition;

        // Show the popup panel with the length and weight values
        ShowPopup(length, weight);
    }

    private void ShowPopup(float length, float weight)
    {
        // Update the popup text
        popupText.text = $"Paper rider removes at length: {length:F2} cm and weight: {weight:F2} kg";

        // Show the popup panel
        popupPanel.SetActive(true);
    }

    private void AddValuesToTable()
    {
        // Check if there are available slots in the table
        if (currentSetIndex >= lengthTexts.Length || currentSetIndex >= weightTexts.Length)
        {
            Debug.Log("All value sets have been filled.");
            return;
        }

        // Get the current length and weight
        float currentLength = movableBridge.GetCurrentLength();
        float currentWeight = weightHanger.totalWeight;

        // Update the table with the current values
        lengthTexts[currentSetIndex].text = $"{currentLength:F2} cm";
        weightTexts[currentSetIndex].text = $"{currentWeight:F2} kg";

        // Move to the next set
        currentSetIndex++;

        // Update the status UI
        UpdateStatusUI();

        // Hide the popup panel
        popupPanel.SetActive(false);
    }

    private void UpdateStatusUI()
    {
        if (currentSetIndex >= lengthTexts.Length || currentSetIndex >= weightTexts.Length)
        {
            // All value sets are filled
            statusCircle.color = Color.green; // Change circle color to green
            statusText.text = "Done"; // Change text to "Done"
        }
        else
        {
            // Practical is still in progress
            statusCircle.color = Color.yellow; // Set circle color to yellow
            statusText.text = "In Progress"; // Set text to "In Progress"
        }
    }

    private void ClosePopup()
    {
        // Hide the popup panel
        popupPanel.SetActive(false);
    }


}