using UnityEngine;
using TMPro; // Add this namespace for TextMeshPro
using UnityEngine.UI;

public class StepByStepGuidance : MonoBehaviour
{
    
    public GameObject stepByStepPanel;

    public GameObject fullGuidancePanel;

    public TextMeshProUGUI stepNumberText;
    public TextMeshProUGUI stepText;

    // Arrays to hold step numbers and instructions
    public string[] stepNumbers;
    public string[] stepInstructions;

    // Index to track the current step
    private int currentStepIndex = 0;

    public Button TeacherButton;

    void Start()
    {
        stepByStepPanel.SetActive(false);
        UpdateStep();

        TeacherButton.onClick.AddListener(OnTeacherButtonClicked);
    }

    // Method to update the step display
    private void UpdateStep()
    {
        if (currentStepIndex >= 0 && currentStepIndex < stepNumbers.Length)
        {
            stepNumberText.text = stepNumbers[currentStepIndex];
            stepText.text = stepInstructions[currentStepIndex];
        }
    }

    // Method to go to the next step
    public void NextStep()
    {
        if (currentStepIndex < stepNumbers.Length - 1)
        {
            currentStepIndex++;
            UpdateStep();
        }
    }

    // Method to go to the previous step
    public void PreviousStep()
    {
        if (currentStepIndex > 0)
        {
            currentStepIndex--;
            UpdateStep();
        }
    }

    // Method to open the full guidance panel
    public void OpenFullGuidance()
    {
        if (fullGuidancePanel != null)
        {
            fullGuidancePanel.SetActive(true);
        }
    }

    // Method to close the step-by-step panel
    public void CloseStepByStep()
    {
        if (stepByStepPanel != null)
        {
            stepByStepPanel.SetActive(false);
        }
    }

    public void OnTeacherButtonClicked()
    {
        Debug.Log("Teacher button clicked!");
        gameObject.SetActive(!gameObject.activeSelf); // Toggle the active state of the GameObject the script is on
        Debug.Log("StepByStepPanel active: " + gameObject.activeSelf);
    }
}