using System.Collections;
using UnityEngine;
using UnityEngine.Networking;
using TMPro;
using System.Text;
using UnityEngine.SceneManagement;
using System.Text.RegularExpressions;

public class MongoDBManager : MonoBehaviour
{
    public TMP_InputField emailField, nameField, dobField, passwordField, rePasswordField;
    public TMP_Dropdown userRoleDropdown;
    public TMP_Text errorMessageText;
    public string serverUrl = "http://localhost:5000";

    // Method for signup
    public void Signup()
    {
        StartCoroutine(SignupCoroutine());
    }

    IEnumerator SignupCoroutine()
    {
        // Trim input fields to remove accidental spaces
        string email = emailField.text.Trim();
        string name = nameField.text.Trim();
        string dob = dobField.text.Trim();
        string password = passwordField.text.Trim();
        string rePassword = rePasswordField.text.Trim();
        string userRole = userRoleDropdown.options[userRoleDropdown.value].text.Trim();

        // Validation for empty fields
        if (string.IsNullOrEmpty(email) ||
            string.IsNullOrEmpty(name) ||
            string.IsNullOrEmpty(dob) ||
            string.IsNullOrEmpty(userRole) ||
            string.IsNullOrEmpty(password) ||
            string.IsNullOrEmpty(rePassword))
        {
            DisplayError("❌ All fields are required. Please fill in all the fields.");
            yield break;
        }

        // Email format validation
        if (!IsValidEmail(email))
        {
            DisplayError("❌ Invalid email format. Please enter a valid email.");
            yield break;
        }

        // Check if passwords match
        if (password != rePassword)
        {
            DisplayError("❌ Passwords do not match. Please try again.");
            yield break;
        }

        // Create JSON object for signup request
        string jsonData = JsonUtility.ToJson(new SignupData
        {
            email = email,
            name = name,
            dob = dob,
            userRole = userRole,
            password = password,
            rePassword = rePassword
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
                string responseText = www.downloadHandler.text;
                Debug.Log("✅ Signup Success: " + responseText);

                if (responseText.Contains("User registered successfully"))
                {
                    Debug.Log("✅ Signup Successful!");
                    SceneManager.LoadScene("LoginPage");
                }
                else
                {
                    DisplayError("❌ Unexpected error. Please try again.");
                }
            }
            else
            {
                string errorMessage = www.downloadHandler.text;
                Debug.LogError("❌ Signup Error: " + errorMessage);
                DisplayError("❌ Signup failed. Please try again.");
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
        string email = emailField.text.Trim();
        string password = passwordField.text.Trim();

        // Validation for empty fields
        if (string.IsNullOrEmpty(email) || string.IsNullOrEmpty(password))
        {
            DisplayError("❌ Email and password are required.");
            yield break;
        }

        string jsonData = JsonUtility.ToJson(new LoginData
        {
            email = email,
            password = password
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
                Debug.Log("✅ Server Response: " + responseText);

                if (responseText.Contains("Login successful"))
                {
                    Debug.Log("✅ Login Successful!");
                    SceneManager.LoadScene("HomePage");
                }
                else
                {
                    DisplayError("❌ Unexpected error. Please try again.");
                }
            }
            else if (www.responseCode == 400)
            {
                string responseText = www.downloadHandler.text;
                Debug.Log("❌ Server Response: " + responseText);

                if (responseText.Contains("User not found"))
                {
                    DisplayError("❌ Login Failed: User not found.");
                }
                else if (responseText.Contains("Incorrect password"))
                {
                    DisplayError("❌ Login Failed: Incorrect password.");
                }
                else
                {
                    DisplayError("❌ Login Failed: Unknown error.");
                }
            }
            else
            {
                Debug.LogError("❌ Login Request Failed: " + www.error);
                DisplayError("❌ Unable to connect to the server. Please try again.");
            }
        }
    }

    void DisplayError(string message)
    {
        if (errorMessageText != null)
        {
            errorMessageText.text = message;
        }
        Debug.LogError(message);
    }

    // Email validation function using regex
    bool IsValidEmail(string email)
    {
        string pattern = @"^[a-zA-Z0-9._%+-]+@[a-zA-Z0-9.-]+\.[a-zA-Z]{2,}$";
        Regex regex = new Regex(pattern);
        return regex.IsMatch(email);
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
