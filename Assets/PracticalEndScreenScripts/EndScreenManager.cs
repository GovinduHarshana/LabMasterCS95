using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;

public class EndScreenManager : MonoBehaviour
{
    public TextMeshProUGUI sessionTimeText;

    void Start()
    {
       
    }

    // Method to go to the Home scene
    public void OnHomeButtonClicked()
    {
        SceneManager.LoadScene("HomePageNew");
    }

    // Method to go to the Practical List scene
    public void OnQuizListButtonClicked()
    {
        SceneManager.LoadScene("PhysicsQuizList");
    }

    // Method to redo the practical
    public void OnRedoButtonClicked()
    {
        SceneManager.LoadScene("QuizSonometer"); // Replace with your practical scene name
    }

    // Method to go to the Quiz section
    public void OnPracticalButtonClicked()
    {
        SceneManager.LoadScene("PhysicsPractical"); // Replace with your quiz scene name
    }
}