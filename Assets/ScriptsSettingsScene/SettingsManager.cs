using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class SettingsManager : MonoBehaviour
{
    // Side Panel Buttons
    public Button notificationsButton;
    public Button progressButton;

    // Content Panels
    public GameObject notificationsContent;
    public GameObject progressContent;

    // Reset Buttons
    public Button resetPracticalProgressButton;
    public Button resetQuizProgressButton;

    void Start()
    {
        // Set initial state
        ShowNotificationsContent();

        // Add button listeners
        notificationsButton.onClick.AddListener(ShowNotificationsContent);
        progressButton.onClick.AddListener(ShowProgressContent);
        resetPracticalProgressButton.onClick.AddListener(ResetPracticalProgress);
        resetQuizProgressButton.onClick.AddListener(ResetQuizProgress);
    }

    // Show Notifications Content
    private void ShowNotificationsContent()
    {
        notificationsContent.SetActive(true);
        progressContent.SetActive(false);
    }

    // Show Progress Content
    private void ShowProgressContent()
    {
        notificationsContent.SetActive(false);
        progressContent.SetActive(true);
    }

    // Reset Practical Progress
    private void ResetPracticalProgress()
    {
        PlayerPrefs.SetFloat("Progress", 0f); // Reset to 0%
        PlayerPrefs.Save();
        Debug.Log("Practical Progress Reset!");
    }

    // Reset Quiz Progress
    private void ResetQuizProgress()
    {
        PlayerPrefs.SetFloat("QuizProgress", 0f); // Reset to 0%
        PlayerPrefs.Save();
        Debug.Log("Quiz Progress Reset!");
    }
}