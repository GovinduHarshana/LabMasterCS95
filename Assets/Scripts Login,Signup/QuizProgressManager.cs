using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class QuizProgressManager : MonoBehaviour
{
    public Image quizProgressBar; // Reference to the quiz progress bar
    public TextMeshProUGUI quizProgressText; // Reference to the quiz progress text

    void Start()
    {
        // Load quiz progress from PlayerPrefs
        float quizProgress = PlayerPrefs.GetFloat("QuizProgress", 0f);

        // Update the quiz progress bar and text
        UpdateQuizProgress(quizProgress);
    }

    // Method to update the quiz progress bar and text
    private void UpdateQuizProgress(float progress)
    {
        // Ensure progress does not exceed 100%
        progress = Mathf.Clamp(progress, 0f, 100f);

        // Update the progress bar fill amount
        quizProgressBar.fillAmount = progress / 100f;

        // Update the progress text
        quizProgressText.text = $"{progress:F0}%";
    }

    public void OnResetQuizProgressButtonClicked()
    {
        // Reset quiz progress to 0%
        PlayerPrefs.SetFloat("QuizProgress", 0f);
        PlayerPrefs.Save();

        // Update the quiz progress bar and text
        UpdateQuizProgress(0f);
    }
}