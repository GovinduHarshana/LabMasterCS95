using System.Collections;
using UnityEngine;
using UnityEngine.Networking;
using TMPro;
using UnityEngine.SceneManagement;
using System.Text;

public class Login : MonoBehaviour
{
    public TMP_InputField emailField, passwordField;
    public TMP_Text errorMessageText;
    private string loginServerUrl = "https://lab-master-backend.vercel.app/api/auth/login";

    public void LoginUser()
    {
        StartCoroutine(LoginCoroutine());
    }

    IEnumerator LoginCoroutine()
    {
        string email = emailField.text.Trim();
        string password = passwordField.text.Trim();

        if (string.IsNullOrEmpty(email) || string.IsNullOrEmpty(password))
        {
            DisplayError("Email and password are required.");
            yield break;
        }

        string jsonData = JsonUtility.ToJson(new LoginData { email = email, password = password });

        using (UnityWebRequest www = new UnityWebRequest(loginServerUrl, "POST"))
        {
            byte[] bodyRaw = Encoding.UTF8.GetBytes(jsonData);
            www.uploadHandler = new UploadHandlerRaw(bodyRaw);
            www.downloadHandler = new DownloadHandlerBuffer();
            www.SetRequestHeader("Content-Type", "application/json");

            yield return www.SendWebRequest();

            if (www.result == UnityWebRequest.Result.Success)
            {
                string responseText = www.downloadHandler.text;
                Debug.Log("Server Response: " + responseText);

                LoginResponse response = JsonUtility.FromJson<LoginResponse>(responseText);

                if (response.message == "Login successful")
                {
                    Debug.Log("Login Successful!");

                    // Save user details in PlayerPrefs
                    PlayerPrefs.SetString("userId", response.userId);
                    PlayerPrefs.SetString("userName", response.name);
                    PlayerPrefs.SetString("userRole", response.role);
                    PlayerPrefs.Save();

                    Debug.Log($"Saved User Data - ID: {response.userId}, Name: {response.name}, Role: {response.role}");

                    SceneManager.LoadScene("HomePageNew");
                }
                else
                {
                    DisplayError("Unexpected error. Please try again.");
                }
            }
            else if (www.responseCode == 401) // Handle login failure
            {
                string responseText = www.downloadHandler.text;
                Debug.LogError("Server Response: " + responseText);

                if (responseText.Contains("Invalid email"))
                {
                    DisplayError("User not found. Please check your email.");
                }
                else if (responseText.Contains("Invalid password"))
                {
                    DisplayError("Incorrect password. Please try again.");
                }
                else
                {
                    DisplayError("Login failed. Please check your details.");
                }
            }
            else
            {
                Debug.LogError("Login Request Failed: " + www.error);
                DisplayError("Unable to connect to the server. Please try again.");
            }
        }
    }

    public void LoginAsGuest()
    {
        // Clear all previously saved user data
        PlayerPrefs.DeleteKey("email");
        PlayerPrefs.DeleteKey("userName");
        PlayerPrefs.DeleteKey("dob");
        PlayerPrefs.DeleteKey("token");
        PlayerPrefs.DeleteKey("userId");
        PlayerPrefs.DeleteKey("useRole");
        PlayerPrefs.DeleteKey("Progress");
        PlayerPrefs.DeleteKey("QuizProgress");
        PlayerPrefs.DeleteKey("notes");

        // Set guest user values
        PlayerPrefs.SetString("userRole", "Guest");
        PlayerPrefs.SetString("userName", "Guest User");
        PlayerPrefs.Save();

        // Navigate to the home scene
        UnityEngine.SceneManagement.SceneManager.LoadScene("HomePageNew");
    }

    void DisplayError(string message)
    {
        if (errorMessageText != null)
        {
            errorMessageText.text = message;
        }
        Debug.LogError(message);
    }
}

[System.Serializable]
public class LoginData
{
    public string email;
    public string password;
}

[System.Serializable]
public class LoginResponse
{
    public string message;
    public string userId;
    public string name;
    public string role;
}
