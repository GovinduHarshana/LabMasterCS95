using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneSwitcher2 : MonoBehaviour
{
    // Function to load a new scene

    public void LoadWelcomePage()
    {
        SceneManager.LoadScene("WelcomePage");
    }

    public void LoadAboutUs()
    {
        SceneManager.LoadScene("AboutUs");
    }

    public void LoadHelpAndSupportWelcome()
    {
        SceneManager.LoadScene("HelpAndSupportWelcome");
    }




}
