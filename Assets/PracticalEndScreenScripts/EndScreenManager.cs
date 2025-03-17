using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;

public class EndScreenManager : MonoBehaviour
{
    public TextMeshProUGUI sessionTimeText;

    void Start()
    {
        // Retrieve the session time from the previous scene
        float sessionTime = PlayerPrefs.GetFloat("SessionTime", 0f);
        int minutes = (int)(sessionTime / 60);
        int seconds = (int)(sessionTime % 60);
        sessionTimeText.text = string.Format("Session Time: {0:00}:{1:00}", minutes, seconds);

        // Load the last practical type (Physics or Chemistry)
        lastPracticalType = PlayerPrefs.GetString("LastPracticalType", "Unknown");
        // Load the last quiz scene name
        lastQuizSceneName = PlayerPrefs.GetString("LastQuizScene", "QuizFlameTest"); // Default to FlameTest if not set
    }

    // Store the last practical type (Physics or Chemistry)
    private string lastPracticalType;
    // Store the last quiz scene name
    private string lastQuizSceneName;

    // Method to redo the quiz
    public void OnRedoButtonClicked()
    {
        SceneManager.LoadScene(lastQuizSceneName);
    }

    // Method to go to the corresponding practical list
    public void OnTryPracticalListButtonClicked()
    {
        if (lastPracticalType == "Physics")
        {
            SceneManager.LoadScene("PhysicsList");
        }
        else if (lastPracticalType == "Chemistry")
        {
            SceneManager.LoadScene("ChemistryList");
        }
        else
        {
            // Handle unknown practical type (e.g., default to Chemistry)
            SceneManager.LoadScene("ChemistryList");
            Debug.LogWarning("Unknown practical type. Defaulting to Chemistry List.");
        }
    }

    // Method to go to the Home scene
    public void OnHomeButtonClicked()
    {
        SceneManager.LoadScene("HomePageNew");
    }

    // Method to go to the Practical List scene
    public void OnQuizListButtonClicked()
    {
        if (lastPracticalType == "Physics")
        {
            SceneManager.LoadScene("PhysicsQuizList");
        }
        else if (lastPracticalType == "Chemistry")
        {
            SceneManager.LoadScene("ChemistryQuizList"); 
        }
        else
        {
            // Handle unknown practical type (e.g., default to Chemistry)
            SceneManager.LoadScene("ChemistryQuizList");
            Debug.LogWarning("Unknown practical type. Defaulting to Chemistry Quiz List.");
        }
    }

    // Method to redo the practical
    public void OnTryPracticalButtonClicked()
    {
        // Load the last practical scene based on the stored type
        if (lastPracticalType == "Physics")
        {
            SceneManager.LoadScene("PhysicsPractical");  
        }
        else if (lastPracticalType == "Chemistry")
        {
            SceneManager.LoadScene("FlameTestNew");  
        }
        else
        {
            // Handle unknown practical type (e.g., default to Chemistry)
            SceneManager.LoadScene("FlameTestNew");
            Debug.LogWarning("Unknown practical type. Defaulting to Chemistry Practical.");
        }
    }
}