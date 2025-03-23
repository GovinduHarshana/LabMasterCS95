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

        // Determine if it's a practical or quiz end screen
        isPracticalEnd = SceneManager.GetActiveScene().name == "EndOfPractical";
    }

    // Store the last practical type (Physics or Chemistry)
    private string lastPracticalType;
    // Store the last quiz scene name
    private string lastQuizSceneName;
    // Store if it's the practical or quiz end screen
    private bool isPracticalEnd;

    // Method to redo the activity
    public void OnRedoButtonClicked()
    {
        if (isPracticalEnd)
        {
            // Redo the practical
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
                SceneManager.LoadScene("FlameTestNew");
                Debug.LogWarning("Unknown practical type. Defaulting to Chemistry Practical.");
            }
        }
        else
        {
            // Redo the quiz
            SceneManager.LoadScene(lastQuizSceneName);
        }
    }

    // Method to go to the corresponding activity
    public void OnTryActivityButtonClicked()
    {
        if (isPracticalEnd)
        {
            // Go to the quiz
            SceneManager.LoadScene(lastQuizSceneName);
        }
        else
        {
            // Go to the practical list
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
                SceneManager.LoadScene("ChemistryList");
                Debug.LogWarning("Unknown practical type. Defaulting to Chemistry List.");
            }
        }
    }

    // Method to go to the Home scene
    public void OnHomeButtonClicked()
    {
        SceneManager.LoadScene("HomePageNew");
    }

    // Method to go to the Activity List scene
    public void OnActivityListButtonClicked()
    {
        if (isPracticalEnd)
        {
            // Go to the quiz list
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
                SceneManager.LoadScene("ChemistryQuizList");
                Debug.LogWarning("Unknown practical type. Defaulting to Chemistry Quiz List.");
            }
        }
        else
        {
            // Go to the practical list
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
                SceneManager.LoadScene("ChemistryList");
                Debug.LogWarning("Unknown practical type. Defaulting to Chemistry List.");
            }
        }
    }
}