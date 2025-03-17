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

    // Function to load the WelcomePage 
    public void LoadWelcomePage()
    {
        SceneManager.LoadScene("WelcomePage");
    }

    // Function to load the PhysicsQuizListPage 
    public void LoadPhysicsQuizList()
    {
        SceneManager.LoadScene("PhysicsQuizList");
    }

    // Function to load the ChemistryQuizListPage 
    public void LoadChemistryQuizList()
    {
        SceneManager.LoadScene("ChemistryQuizList");
    }

    // Function to load the PhysicsListPage 
    public void LoadPhysicsList()
    {
        SceneManager.LoadScene("PhysicsList");
    }

    // Function to load the ChemistryListPage 
    public void LoadChemistryList()
    {
        SceneManager.LoadScene("ChemistryList");
    }
}
