// Login.cs
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
    public string serverUrl = "https://lab-master-backend.vercel.app/api/auth/login"; // Vercel URL

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

        string jsonData = JsonUtility.ToJson(new LoginData
        {
            email = email,
            password = password
        });

        using (UnityWebRequest www = new UnityWebRequest(serverUrl, "POST"))
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

                if (responseText.Contains("Login successful"))
                {
                    Debug.Log("Login Successful!");
                    SceneManager.LoadScene("HomePageNew");
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

                if (responseText.Contains("User not found"))
                {
                    DisplayError("Login Failed: User not found.");
                }
                else if (responseText.Contains("Incorrect password"))
                {
                    DisplayError("Login Failed: Incorrect password.");
                }
                else
                {
                    DisplayError("Login Failed: Unknown error.");
                }
            }
            else
            {
                Debug.LogError("Login Request Failed: " + www.error);
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
}

[System.Serializable]
public class LoginData
{
    public string email;
    public string password;
}