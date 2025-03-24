using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;

public class NewEndScreenManager : MonoBehaviour
{
    public TextMeshProUGUI sessionTimeText;

    void Start()
    {
        // Retrieve the session time from PlayerPrefs
        float sessionTime = PlayerPrefs.GetFloat("SessionTime", 0f);
        int minutes = (int)(sessionTime / 60);
        int seconds = (int)(sessionTime % 60);
        sessionTimeText.text = string.Format("Session Time: {0:00}:{1:00}", minutes, seconds);
    }

    // Method to go to the Home scene
    public void OnHomeButtonClicked()
    {
        SceneManager.LoadScene("HomePageNew");
    }

    // Method to go to the Practical List scene
    public void OnPracticalListButtonClicked()
    {
        // Retrieve the practical type from PlayerPrefs
        string practicalType = PlayerPrefs.GetString("PracticalType", "");

        if (practicalType == "Chemistry")
        {
            SceneManager.LoadScene("ChemistryList");
        }
        else if (practicalType == "Physics")
        {
            SceneManager.LoadScene("PhysicsList");
        }
        else
        {
            Debug.LogWarning("Unknown practical type: " + practicalType);
        }
    }

    // Method to redo the practical
    public void OnRedoButtonClicked()
    {
        // Retrieve the practical type from PlayerPrefs
        string practicalType = PlayerPrefs.GetString("PracticalType", "");

        if (practicalType == "Chemistry")
        {
            SceneManager.LoadScene("FlameTestNew"); // Replace with your Chemistry Practical scene name
        }
        else if (practicalType == "Physics")
        {
            SceneManager.LoadScene("PhysicsPractical"); // Replace with your Physics Practical scene name
        }
        else
        {
            Debug.LogWarning("Unknown practical type: " + practicalType);
        }
    }

    // Method to go to the Quiz section
    public void OnQuizButtonClicked()
    {
        // Retrieve the practical type from PlayerPrefs
        string practicalType = PlayerPrefs.GetString("PracticalType", "");

        if (practicalType == "Chemistry")
        {
            SceneManager.LoadScene("QuizFlameTest"); // Replace with your Chemistry Quiz scene name
        }
        else if (practicalType == "Physics")
        {
            SceneManager.LoadScene("QuizSonometer"); // Replace with your Physics Quiz scene name
        }
        else
        {
            Debug.LogWarning("Unknown practical type: " + practicalType);
        }
    }
}