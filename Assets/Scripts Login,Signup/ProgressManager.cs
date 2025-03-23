using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class ProgressManager : MonoBehaviour
{
    public Image progressBar;
    public TextMeshProUGUI progressText;

    void Start()
    {
        LoadAndUpdateProgress();

        // Load progress from PlayerPrefs
        float progress = PlayerPrefs.GetFloat("Progress", 0f);

        // Update the progress bar and text
        UpdateProgress(progress);
    }

    private void LoadAndUpdateProgress()
    {
        float progress = PlayerPrefs.GetFloat("Progress", 0f);
        UpdateProgress(progress);
    }

    private void UpdateProgress(float progress)
    {
        progress = Mathf.Clamp(progress, 0f, 100f);
        progressBar.fillAmount = progress / 100f;
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

    // Add methods for hiding and showing the progress bar
    public void HideProgressBar()
    {
        if (progressBar != null && progressText != null)
        {
            progressBar.gameObject.SetActive(false);
            progressText.gameObject.SetActive(false);
        }
    }

    public void ShowProgressBar()
    {
        if (progressBar != null && progressText != null)
        {
            progressBar.gameObject.SetActive(true);
            progressText.gameObject.SetActive(true);
            LoadAndUpdateProgress(); // Refresh progress when showing
        }
    }
}