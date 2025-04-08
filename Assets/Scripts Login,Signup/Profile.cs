using System.Collections;
using UnityEngine;
using UnityEngine.Networking;
using TMPro;
using UnityEngine.UI;
using System.Text;

public class ProfileManager : MonoBehaviour
{
    public TMP_InputField nameField, emailField, dobField, mobileField, nicField, passwordField;
    public Button saveButton;
    public TextMeshProUGUI usernameDisplay;
    public TextMeshProUGUI roleDisplay;
    public TextMeshProUGUI menuUsername;
    public TextMeshProUGUI menuRole;

    private string baseUrl = "https://lab-master-backend.vercel.app/api/auth/profile";
    private string userId;

    void Start()
    {
        // Retrieve user role from PlayerPrefs (added to check for guest role)
        string userRole = PlayerPrefs.GetString("userRole", "");

        // If user is a guest, skip fetching data from backend and show guest data instead
        if (userRole == "Guest")
        {
            Debug.Log("Guest user detected. Skipping profile fetch.");
            // Display guest details
            nameField.text = "Guest User";
            emailField.text = "N/A";
            dobField.text = "N/A";
            mobileField.text = "N/A";
            nicField.text = "N/A";
            passwordField.text = "********"; // Hide password for guests

            usernameDisplay.text = "Guest User";
            roleDisplay.text = "Guest";
            menuUsername.text = "Guest User"; // Update menu panel
            menuRole.text = "Guest"; // Update menu panel

            return; // Exit early to prevent backend request
        }

        // Retrieve user ID from PlayerPrefs
        userId = PlayerPrefs.GetString("userId", "");
        if (string.IsNullOrEmpty(userId))
        {
            Debug.LogError("User ID not found in PlayerPrefs. Redirecting to login...");
            // Optionally, you can redirect to login screen if user ID is missing.
            // SceneManager.LoadScene("LoginScene");
            return;
        }

        Debug.Log("ProfileManager Start - userId: " + userId);
        saveButton.onClick.AddListener(UpdateProfile);
        StartCoroutine(GetProfileData());
    }

    IEnumerator GetProfileData()
    {
        string url = $"{baseUrl}/{userId}";
        Debug.Log("Fetching Profile Data: " + url);

        UnityWebRequest request = UnityWebRequest.Get(url);
        request.SetRequestHeader("Content-Type", "application/json");

        yield return request.SendWebRequest();

        if (request.result == UnityWebRequest.Result.Success)
        {
            Debug.Log("Profile Data Fetched: " + request.downloadHandler.text);
            UserProfile profile = JsonUtility.FromJson<UserProfile>(request.downloadHandler.text);

            // Populate UI fields with user data
            nameField.text = profile.name;
            emailField.text = profile.email;
            dobField.text = profile.dob;
            mobileField.text = profile.mobileNumber; // If empty, user can enter manually
            nicField.text = profile.nic; // If empty, user can enter manually
            passwordField.text = "********"; // Hide password

            // Update menu panel username and role
            usernameDisplay.text = profile.name;
            roleDisplay.text = profile.userRole;
            menuUsername.text = profile.name; // Update menu panel
            menuRole.text = profile.userRole; // Update menu panel

            // Save details for other UI elements
            PlayerPrefs.SetString("userName", profile.name);
            PlayerPrefs.SetString("userRole", profile.userRole);
            PlayerPrefs.Save();
        }
        else
        {
            Debug.LogError("Error Fetching Profile Data: " + request.error);
        }
    }

    void UpdateProfile()
    {
        StartCoroutine(UpdateProfileCoroutine());
    }

    IEnumerator UpdateProfileCoroutine()
    {
        UserProfile updatedProfile = new UserProfile
        {
            name = nameField.text,
            email = emailField.text,
            dob = dobField.text,
            mobileNumber = mobileField.text,
            nic = nicField.text,
            password = passwordField.text, // Ideally, don’t send if unchanged
            profilePicture = ""
        };

        string url = $"{baseUrl}/update/{userId}";
        Debug.Log("Updating Profile: " + url);
        string jsonData = JsonUtility.ToJson(updatedProfile);
        byte[] jsonToSend = Encoding.UTF8.GetBytes(jsonData);

        UnityWebRequest request = new UnityWebRequest(url, "PUT");
        request.uploadHandler = new UploadHandlerRaw(jsonToSend);
        request.downloadHandler = new DownloadHandlerBuffer();
        request.SetRequestHeader("Content-Type", "application/json");

        yield return request.SendWebRequest();

        if (request.result == UnityWebRequest.Result.Success)
        {
            Debug.Log("Profile Updated Successfully!");
            StartCoroutine(GetProfileData());

            // Update menu panel dynamically
            menuUsername.text = updatedProfile.name;
            menuRole.text = updatedProfile.userRole;

            // Save updated info
            PlayerPrefs.SetString("userName", updatedProfile.name);
            PlayerPrefs.SetString("userRole", updatedProfile.userRole);
            PlayerPrefs.Save();
        }
        else
        {
            Debug.LogError("Error Updating Profile: " + request.error);
        }
    }
}

[System.Serializable]
public class UserProfile
{
    public string name;
    public string email;
    public string dob;
    public string userRole;
    public string mobileNumber;
    public string nic;
    public string password;
    public string profilePicture;
}
