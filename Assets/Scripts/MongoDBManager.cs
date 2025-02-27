using System.Collections;
using UnityEngine;
using UnityEngine.Networking;
using TMPro;
using System.Text;
using UnityEngine.SceneManagement;

public class MongoDBManager : MonoBehaviour
{
    public TMP_InputField emailField, nameField, dobField, passwordField, rePasswordField;
    public TMP_Dropdown userRoleDropdown;
    public TMP_Text errorText; // Reference to the UI text element for displaying errors
    public string serverUrl = "http://localhost:5000";

    // Method for signup
    public void Signup()
    {
        StartCoroutine(SignupCoroutine());
    }

    IEnumerator SignupCoroutine()
    {
        string jsonData = JsonUtility.ToJson(new SignupData
        {
            email = emailField.text,
            name = nameField.text,
            dob = dobField.text,
            userRole = userRoleDropdown.options[userRoleDropdown.value].text,
            password = passwordField.text,
            rePassword = rePasswordField.text
        });

        using (UnityWebRequest www = new UnityWebRequest(serverUrl + "/signup", "POST"))
        {
            byte[] bodyRaw = Encoding.UTF8.GetBytes(jsonData);
            www.uploadHandler = new UploadHandlerRaw(bodyRaw);
            www.downloadHandler = new DownloadHandlerBuffer();
            www.SetRequestHeader("Content-Type", "application/json");

            yield return www.SendWebRequest();

            if (www.result == UnityWebRequest.Result.Success)
            {
                Debug.Log("✅ Signup Success: " + www.downloadHandler.text);
                SceneManager.LoadScene("LoginPage"); // Redirect to login page after successful signup
            }
            else
            {
                // Handle error messages based on backend response
                string errorMessage = www.downloadHandler.text;
                if (errorMessage.Contains("All fields are required"))
                {
                    DisplayError("❌ All fields are required. Please fill in all the fields.");
                }
                else if (errorMessage.Contains("Passwords do not match"))
                {
                    DisplayError("❌ Passwords do not match. Please try again.");
                }
                else if (errorMessage.Contains("User already exists"))
                {
                    DisplayError("❌ User already exists. Please try a different email.");
                }
                else
                {
                    DisplayError("❌ Signup failed. " + errorMessage);
                }
            }
        }
    }

    // Method for login
    public void Login()
    {
        StartCoroutine(LoginCoroutine());
    }

    IEnumerator LoginCoroutine()
    {
        string jsonData = JsonUtility.ToJson(new LoginData
        {
            email = emailField.text,
            password = passwordField.text
        });

        using (UnityWebRequest www = new UnityWebRequest(serverUrl + "/login", "POST"))
        {
            byte[] bodyRaw = Encoding.UTF8.GetBytes(jsonData);
            www.uploadHandler = new UploadHandlerRaw(bodyRaw);
            www.downloadHandler = new DownloadHandlerBuffer();
            www.SetRequestHeader("Content-Type", "application/json");

            yield return www.SendWebRequest();

            if (www.result == UnityWebRequest.Result.Success)
            {
                string responseText = www.downloadHandler.text;
                Debug.Log("Server Response: " + responseText);  // Log the full response text

                // Handle the server response
                if (responseText.Contains("Login successful"))
                {
                    Debug.Log("✅ Login Successful!");
                    SceneManager.LoadScene("HomePage");
                }
                else if (responseText.Contains("User not found"))
                {
                    DisplayError("❌ Login Failed: User not found.");
                }
                else if (responseText.Contains("Incorrect password"))
                {
                    DisplayError("❌ Login Failed: Incorrect password.");
                }
                else
                {
                    DisplayError("❌ Login Failed: Unexpected error.");
                }
            }
            else
            {
                // This is for unexpected errors (e.g., network issues)
                DisplayError("❌ Login Request Failed: " + www.error);
            }
        }
    }


    // Method to display error messages on the screen
    private void DisplayError(string message)
    {
        errorText.text = message; // Update the UI text to show the error message
        errorText.color = Color.red; // Change the text color to red for error messages
    }
}

// Data models for signup and login
[System.Serializable]
public class SignupData
{
    public string email;
    public string name;
    public string dob;
    public string userRole;
    public string password;
    public string rePassword;
}

[System.Serializable]
public class LoginData
{
    public string email;
    public string password;
}
