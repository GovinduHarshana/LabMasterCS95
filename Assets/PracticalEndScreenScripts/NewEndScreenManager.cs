using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;

public class NewEndScreenManager : MonoBehaviour
{
    public TextMeshProUGUI sessionTimeText;

    void Start()
    {
        // Retrieve the session time from the previous scene
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
        SceneManager.LoadScene("PracticalList");
    }

    // Method to redo the practical
    public void OnRedoButtonClicked()
    {
        SceneManager.LoadScene("FlameTestNew"); // Replace with your practical scene name
    }

    // Method to go to the Quiz section
    public void OnQuizButtonClicked()
    {
        SceneManager.LoadScene("QuizFlameTest"); // Replace with your quiz scene name
    }
}