using UnityEngine;
using UnityEngine.SceneManagement; // Required to switch scenes

public class SceneSwitcher : MonoBehaviour
{
    public void LoadLoginPage() // Function to load the sign-in page
    {
        SceneManager.LoadScene("LoginPage"); // Ensure this matches your scene name
    }
}
