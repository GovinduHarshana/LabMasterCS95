using UnityEngine;
using TMPro;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class SessionManager : MonoBehaviour
{
    // Top Panel References
    public TextMeshProUGUI timerText;
    public GameObject endSessionPopup;

    // Practical Status References
    public Image statusCircle; // Reference to the status circle
    public TextMeshProUGUI statusText; // Reference to the status text

    // Practical Type (Chemistry or Physics)
    public string practicalType; // Set this in the Inspector

    // Timer Variables
    private float sessionTime = 0f;
    private bool isSessionActive = true;

    void Update()
    {
        if (isSessionActive)
        {
            // Update the session timer
            sessionTime += Time.deltaTime;
            UpdateTimerDisplay();
        }
    }

    // Method to update the timer display
    private void UpdateTimerDisplay()
    {
        int minutes = (int)(sessionTime / 60);
        int seconds = (int)(sessionTime % 60);
        timerText.text = string.Format("{0:00}:{1:00}", minutes, seconds);
    }

    // Method to open the End Session Popup
    public void OnEndSessionButtonClicked()
    {
        endSessionPopup.SetActive(true);
    }

    // Method to handle the "Yes" button in the popup
    public void OnYesButtonClicked()
    {
        Debug.Log("Status Text: " + statusText.text); // Debug status text
        Debug.Log("Practical Type: " + practicalType); // Debug practical type

        if (statusText.text == "In Progress")
        {
            if (practicalType == "Chemistry")
            {
                Debug.Log("Redirecting to Chemistry List"); // Debug redirection
                SceneManager.LoadScene("ChemistryList");
            }
            else if (practicalType == "Physics")
            {
                Debug.Log("Redirecting to Physics List"); // Debug redirection
                SceneManager.LoadScene("PhysicsList");
            }
            else
            {
                Debug.LogWarning("Unknown practical type: " + practicalType); // Debug unknown type
            }
        }
        else if (statusText.text == "Done")
        {
            // Save the session time
            PlayerPrefs.SetFloat("SessionTime", sessionTime);

            // Update progress (increase by 10%)
            float currentProgress = PlayerPrefs.GetFloat("Progress", 0f);
            currentProgress += 10f; // Increase progress by 10%
            PlayerPrefs.SetFloat("Progress", currentProgress);
            PlayerPrefs.Save();

            // Load the End Screen scene
            SceneManager.LoadScene("EndOfThePractical");
        }
        else
        {
            Debug.LogWarning("Unknown status: " + statusText.text); // Debug unknown status
        }
    }

    // Method to handle the "No" button in the popup
    public void OnNoButtonClicked()
    {
        endSessionPopup.SetActive(false);
    }
}