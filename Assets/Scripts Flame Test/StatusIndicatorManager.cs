using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class StatusIndicatorManager : MonoBehaviour
{
    public Image statusCircle; // Reference to the circle Image
    public TextMeshProUGUI statusText; // Reference to the TextMeshPro text
    public ChemicalTableManager tableManager; // Reference to the ChemicalTableManager

    private void Start()
    {
        // Initialize the status to "In Progress"
        SetStatusInProgress();
    }

    private void Update()
    {
        // Check if all chemicals have been identified
        if (AreAllChemicalsIdentified())
        {
            SetStatusDone();
        }
    }

    // Set the status to "In Progress"
    private void SetStatusInProgress()
    {
        statusCircle.color = Color.yellow;
        statusText.text = "In Progress";
    }

    // Set the status to "Done"
    private void SetStatusDone()
    {
        statusCircle.color = Color.green;
        statusText.text = "Done";
    }

    // Check if all chemicals have been identified
    private bool AreAllChemicalsIdentified()
    {
        foreach (TextMeshProUGUI colorText in tableManager.chemicalColorTexts)
        {
            if (colorText.text == "?")
            {
                return false; // At least one chemical is not identified
            }
        }
        return true; // All chemicals are identified
    }
}