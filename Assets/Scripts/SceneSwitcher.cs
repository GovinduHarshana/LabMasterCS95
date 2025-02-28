using UnityEngine;
using UnityEngine.SceneManagement; // Required to switch scenes

public class SceneSwitcher : MonoBehaviour
{
    // Function to load the Login page
    public void LoadLoginPage()
    {
        SceneManager.LoadScene("LoginPage");
    }

    // Function to load the Signup page
    public void LoadSignupPage()
    {
        SceneManager.LoadScene("SignupPage");
    }

    // Function to load the HomePage (Dashboard)
    public void LoadHomePage()
    {
        SceneManager.LoadScene("HomePage"); // Change "HomePage" to your actual home scene name
    }
}
