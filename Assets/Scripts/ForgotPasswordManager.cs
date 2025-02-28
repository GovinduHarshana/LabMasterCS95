using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Networking;

public class ForgotPasswordManager : MonoBehaviour
{
    public InputField emailInputField;  // UI input field for email
    public Text responseText;           // UI text to display response message
    private string apiUrl = "http://localhost:5000/forgot-password";  // Update if using a remote server

    public void SendPasswordResetRequest()
    {
        string email = emailInputField.text.Trim();

        if (string.IsNullOrEmpty(email))
        {
            responseText.text = "⚠️ Please enter your email!";
            return;
        }

        StartCoroutine(ForgotPasswordRequest(email));
    }

    private IEnumerator ForgotPasswordRequest(string email)
    {
        // Create JSON data
        string jsonData = "{\"email\":\"" + email + "\"}";
        byte[] postData = System.Text.Encoding.UTF8.GetBytes(jsonData);

        // Setup request
        using (UnityWebRequest request = new UnityWebRequest(apiUrl, "POST"))
        {
            request.uploadHandler = new UploadHandlerRaw(postData);
            request.downloadHandler = new DownloadHandlerBuffer();
            request.SetRequestHeader("Content-Type", "application/json");

            // Send request
            yield return request.SendWebRequest();

            if (request.result == UnityWebRequest.Result.Success)
            {
                responseText.text = "✅ Reset link sent! Check your email.";
            }
            else
            {
                responseText.text = "❌ Error: " + request.downloadHandler.text;
            }
        }
    }
}
