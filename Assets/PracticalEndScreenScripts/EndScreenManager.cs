using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;

public class EndScreenManager : MonoBehaviour
{
    

    void Start()
    {
        
    }

    // Method to go to the Home scene
    public void OnHomeButtonClicked()
    {
        SceneManager.LoadScene("HomePageNew");
    }

    // Method to go to the Quiz List scene
    public void OnQuizListButtonClicked()
    {
        SceneManager.LoadScene("PhysicsQuizList");
    }

    // Method to redo the Quiz
    public void OnRedoButtonClicked()
    {
        SceneManager.LoadScene("QuizSonometer");
    }

    // Method to go to the Quiz section
    public void OnPracticalButtonClicked()
    {
        SceneManager.LoadScene("PhysicsPractical");
    }
}