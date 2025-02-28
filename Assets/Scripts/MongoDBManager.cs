using System.Collections;
using UnityEngine;
using UnityEngine.Networking;
using TMPro;

public class MongoDBManager : MonoBehaviour
{
    public TMP_InputField emailField, nameField, dobField, passwordField, rePasswordField;
    public TMP_Dropdown userRoleDropdown;
    public string serverUrl = "http://localhost:5000";  

    public void Signup()
    {
        StartCoroutine(SignupCoroutine());
    }

    IEnumerator SignupCoroutine()
    {
        // Create a JSON object to hold the data
        string jsonData = JsonUtility.ToJson(new SignupData
        {
            email = emailField.text,
            name = nameField.text,
            dob = dobField.text,
            userRole = userRoleDropdown.options[userRoleDropdown.value].text,
            password = passwordField.text,
            rePassword = rePasswordField.text
        });

        // Send POST request with JSON data
        using (UnityWebRequest www = new UnityWebRequest(serverUrl + "/signup", "POST"))
        {
            byte[] bodyRaw = System.Text.Encoding.UTF8.GetBytes(jsonData);
            www.uploadHandler = new UploadHandlerRaw(bodyRaw);
            www.downloadHandler = new DownloadHandlerBuffer();
            www.SetRequestHeader("Content-Type", "application/json"); // Set the content type to JSON

            yield return www.SendWebRequest();

            if (www.result == UnityWebRequest.Result.Success)
                Debug.Log("Signup Success: " + www.downloadHandler.text);
            else
                Debug.LogError("Signup Failed: " + www.error);
        }
    }

    public void Login()
    {
        StartCoroutine(LoginCoroutine());
    }

    IEnumerator LoginCoroutine()
    {
        // Create a JSON object to hold the data
        string jsonData = JsonUtility.ToJson(new LoginData
        {
            email = emailField.text,
            password = passwordField.text
        });

        // Send POST request with JSON data
        using (UnityWebRequest www = new UnityWebRequest(serverUrl + "/login", "POST"))
        {
            byte[] bodyRaw = System.Text.Encoding.UTF8.GetBytes(jsonData);
            www.uploadHandler = new UploadHandlerRaw(bodyRaw);
            www.downloadHandler = new DownloadHandlerBuffer();
            www.SetRequestHeader("Content-Type", "application/json"); // Set the content type to JSON

            yield return www.SendWebRequest();

            if (www.result == UnityWebRequest.Result.Success)
                Debug.Log("Login Success: " + www.downloadHandler.text);
            else
                Debug.LogError("Login Failed: " + www.error);
        }
    }
}

// Class for Signup data
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

// Class for Login data
[System.Serializable]
public class LoginData
{
    public string email;
    public string password;
}
