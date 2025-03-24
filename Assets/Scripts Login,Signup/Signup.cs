// Signup.cs
using System.Collections;
using UnityEngine;
using UnityEngine.Networking;
using TMPro;
using UnityEngine.SceneManagement;
using System.Text;
using System.Text.RegularExpressions;

public class Signup : MonoBehaviour
{
    public TMP_InputField emailField, nameField, dobField, passwordField, rePasswordField;
    public TMP_Dropdown userRoleDropdown;
    public TMP_Text errorMessageText;
    public string signupServerUrl = "https://lab-master-backend.vercel.app/api/auth/signup"; // Vercel URL

    public void SignupUser()
    {
        StartCoroutine(SignupCoroutine());
    }

    IEnumerator SignupCoroutine()
    {
        string email = emailField.text.Trim();
        string name = nameField.text.Trim();
        string dob = dobField.text.Trim();
        string password = passwordField.text.Trim();
        string rePassword = rePasswordField.text.Trim();
        string userRole = userRoleDropdown.options[userRoleDropdown.value].text.Trim();

        if (string.IsNullOrEmpty(email) ||
            string.IsNullOrEmpty(name) ||
            string.IsNullOrEmpty(dob) ||
            string.IsNullOrEmpty(userRole) ||
            string.IsNullOrEmpty(password) ||
            string.IsNullOrEmpty(rePassword))
        {
            DisplayError("All fields are required. Please fill in all the fields.");
            yield break;
        }

        if (!IsValidEmail(email))
        {
            DisplayError("Invalid email format. Please enter a valid email.");
            yield break;
        }

        if (!IsValidPassword(password))
        {
            DisplayError("Password must be at least 8 characters long and include at least one uppercase letter, one number, and one special character.");
            yield break;
        }

        if (password != rePassword)
        {
            DisplayError("Passwords do not match. Please try again.");
            yield break;
        }

        string jsonData = JsonUtility.ToJson(new SignupData
        {
            email = email,
            name = name,
            dob = dob,
            userRole = userRole,
            password = password,
            rePassword = rePassword
        });

        using (UnityWebRequest www = new UnityWebRequest(signupServerUrl, "POST"))
        {
            byte[] bodyRaw = Encoding.UTF8.GetBytes(jsonData);
            www.uploadHandler = new UploadHandlerRaw(bodyRaw);
            www.downloadHandler = new DownloadHandlerBuffer();
            www.SetRequestHeader("Content-Type", "application/json");

            yield return www.SendWebRequest();

            if (www.result == UnityWebRequest.Result.Success)
            {
                string responseText = www.downloadHandler.text;
                Debug.Log("Signup Success: " + responseText);

                if (responseText.Contains("User registered successfully"))
                {
                    Debug.Log("Signup Successful!");
                    SceneManager.LoadScene("LoginPageNew");
                }
                else
                {
                    DisplayError("Unexpected error. Please try again.");
                }
            }
            else if (www.responseCode == 400)
            {
                string responseText = www.downloadHandler.text;
                Debug.Log("Server Response: " + responseText);

                if (responseText.Contains("User already exists"))
                {
                    DisplayError("User already exists. Please try logging in.");
                }
                else if (responseText.Contains("All fields are required"))
                {
                    DisplayError("All fields are required. Please fill in all the fields.");
                }
                else if (responseText.Contains("Passwords do not match"))
                {
                    DisplayError("Passwords do not match. Please try again.");
                }
                else
                {
                    DisplayError("Signup failed. Please try again.");
                }
            }
            else
            {
                Debug.LogError("Signup Error: " + www.error);
                DisplayError("Unable to connect to the server. Please try again.");
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

    bool IsValidEmail(string email)
    {
        string pattern = @"^[a-zA-Z0-9._%+-]+@[a-zA-Z0-9.-]+\.[a-zA-Z]{2,}$";
        Regex regex = new Regex(pattern);
        return regex.IsMatch(email);
    }

    bool IsValidPassword(string password)
    {
        string pattern = @"^(?=.*[A-Z])(?=.*\d)(?=.*[\W_]).{8,}$";
        Regex regex = new Regex(pattern);
        return regex.IsMatch(password);
    }
}

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