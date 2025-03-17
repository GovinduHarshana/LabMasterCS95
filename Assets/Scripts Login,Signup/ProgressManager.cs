using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class ProgressManager : MonoBehaviour
{
    public Image progressBar;
    public TextMeshProUGUI progressText;

    void Start()
    {
        // Load progress from PlayerPrefs
        float progress = PlayerPrefs.GetFloat("Progress", 0f);

        // Update the progress bar and text
        UpdateProgress(progress);
    }

    // Method to update the progress bar and text
    private void UpdateProgress(float progress)
    {
        // Ensure progress does not exceed 100%
        progress = Mathf.Clamp(progress, 0f, 100f);

        // Update the progress bar fill amount
        progressBar.fillAmount = progress / 100f;

        // Update the progress text
        progressText.text = $"{progress:F0}%";
    }

    public void OnResetButtonClicked()
    {
        // Reset progress to 0%
        PlayerPrefs.SetFloat("Progress", 0f);
        PlayerPrefs.Save();

        // Update the progress bar and text
        UpdateProgress(0f);
    }
}