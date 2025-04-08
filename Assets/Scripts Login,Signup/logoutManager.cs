using UnityEngine;
using UnityEngine.SceneManagement;

public class LogoutUIManager : MonoBehaviour
{
    public GameObject logoutPanel; // Assign this in the Inspector

    // Show the logout confirmation panel
    public void ShowLogoutPanel()
    {
        logoutPanel.SetActive(true);
    }

    // Hide the logout confirmation panel
    public void HideLogoutPanel()
    {
        logoutPanel.SetActive(false);
    }

    public void ConfirmLogout()
    {
        PlayerPrefs.DeleteKey("email");
        PlayerPrefs.DeleteKey("role");
        PlayerPrefs.DeleteKey("username");
        PlayerPrefs.DeleteKey("dob");
        PlayerPrefs.DeleteKey("token");
        PlayerPrefs.DeleteKey("userId");
        PlayerPrefs.Save();

        SceneManager.LoadScene("WelcomePage");
    }

}
