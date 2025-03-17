using UnityEngine;

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
}
