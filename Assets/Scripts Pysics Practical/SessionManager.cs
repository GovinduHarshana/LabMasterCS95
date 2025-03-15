using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;

public class SessionManager : MonoBehaviour
{
    // Top Panel References
    public TextMeshProUGUI timerText;
    public GameObject endSessionPopup;

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
        // Load the End Screen scene
        SceneManager.LoadScene("EndScreen");
    }

    // Method to handle the "No" button in the popup
    public void OnNoButtonClicked()
    {
        endSessionPopup.SetActive(false);
    }
}