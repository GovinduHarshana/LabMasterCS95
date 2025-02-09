using UnityEngine;
using TMPro;

public class PracticalSteps : MonoBehaviour
{
    public TextMeshProUGUI guideText; // Guide message UI
    public GameObject stick;  // Stick GameObject
    public GameObject powder; // Powder GameObject
    public GameObject flame;  // Flame GameObject

    private bool isPowderApplied = false;
    private bool isStickBurning = false;
    private int currentStep = 0;

    void Start()
    {
        UpdateStep(); // Initialize the first guide message
    }

    public void NextStep()
    {
        currentStep++;
        Debug.Log("Step Updated: " + currentStep);

        if (guideText == null)
        {
            Debug.LogError("GuideText is not assigned in the Inspector!");
            return;
        }

        UpdateStep(); // Call UpdateStep() to ensure everything updates correctly
    }

    void UpdateStep()
    {
        Debug.Log("Updating Step UI: " + currentStep);

        switch (currentStep)
        {
            case 0:
                guideText.text = "Step 1: Get the stick.";
                stick.SetActive(true);
                powder.SetActive(false);
                flame.SetActive(false);
                break;

            case 1:
                guideText.text = "Step 2: Apply the powder to the stick.";
                powder.SetActive(true);
                break;

            case 2:
                guideText.text = "Step 3: Hold the stick to the flame.";
                powder.SetActive(false);
                flame.SetActive(true);
                break;

            case 3:
                if (isStickBurning)
                {
                    guideText.text = "Practical Completed! ?";
                }
                break;

            default:
                guideText.text = "Experiment Completed! ?";
                break;
        }

        // Force UI refresh
        guideText.enabled = false;
        guideText.enabled = true;
    }

    public void ApplyPowder()
    {
        if (currentStep == 1)
        {
            isPowderApplied = true;
            NextStep(); // Move to the next step
        }
    }

    public void HoldToFlame()
    {
        if (currentStep == 2 && isPowderApplied)
        {
            isStickBurning = true;
            NextStep(); // Move to the final step
        }
    }
}
