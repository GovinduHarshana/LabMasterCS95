using UnityEngine;
using UnityEngine.SceneManagement; // Required to switch scenes

public class SceneSwitcher : MonoBehaviour
{
    // Function to load the Login page
    public void LoadLoginPageNew()
    {
        SceneManager.LoadScene("LoginPageNew");
    }

    // Function to load the Signup page
    public void LoadSignupPage()
    {
        SceneManager.LoadScene("SignupPage");
    }

    // Function to load the HomePage 
    public void LoadHomePageNew()
    {
        SceneManager.LoadScene("HomePageNew");
    }

    // Function to load the ForgetPasswordPage 
    public void LoadForgetPassword()
    {
        SceneManager.LoadScene("ForgetPassword");
    }
}
