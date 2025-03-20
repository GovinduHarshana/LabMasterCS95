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

    // Function to load the ProfilePage 
    public void LoadProfilePage()
    {
        SceneManager.LoadScene("ProfilePage");
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

    // Function to load the FlameTestNewPage 
    public void LoadFlameTestNew()
    {
        SceneManager.LoadScene("FlameTestNew");
    }

    // Function to load the PhysicsPracticalPage 
    public void LoadPhysicsPractical()
    {
        SceneManager.LoadScene("PhysicsPractical");
    }

    // Function to load the QuizFlameTestPage 
    public void LoadQuizFlameTest()
    {
        SceneManager.LoadScene("QuizFlameTest");
    }

    // Function to load the QuizSonometerPage 
    public void LoadQuizSonometer()
    {
        SceneManager.LoadScene("QuizSonometer");
    }

}
