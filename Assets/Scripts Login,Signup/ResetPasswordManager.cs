using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Networking;

public class ResetPasswordManager : MonoBehaviour
{
    public InputField tokenInputField;  // UI field for reset token
    public InputField newPasswordField; // UI field for new password
    public InputField confirmPasswordField; // UI field to confirm password
    public Text responseText;           // UI text to display response message
    private string apiUrl = "http://localhost:5000/reset-password";  // Update if using a remote server

    public void SendResetPasswordRequest()
    {
        string token = tokenInputField.text.Trim();
        string newPassword = newPasswordField.text.Trim();
        string confirmPassword = confirmPasswordField.text.Trim();

        if (string.IsNullOrEmpty(token) || string.IsNullOrEmpty(newPassword) || string.IsNullOrEmpty(confirmPassword))
        {
            responseText.text = "⚠️ All fields are required!";
            return;
        }

        if (newPassword != confirmPassword)
        {
            responseText.text = "❌ Passwords do not match!";
            return;
        }

        StartCoroutine(ResetPasswordRequest(token, newPassword));
    }

    private IEnumerator ResetPasswordRequest(string token, string newPassword)
    {
        // Create JSON data
        string jsonData = "{\"token\":\"" + token + "\", \"newPassword\":\"" + newPassword + "\"}";
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
                responseText.text = "✅ Password reset successfully! Redirecting to login...";
                yield return new WaitForSeconds(2); // Delay before redirect
                UnityEngine.SceneManagement.SceneManager.LoadScene("LoginScene"); // Change to your login scene name
            }
            else
            {
                responseText.text = "❌ Error: " + request.downloadHandler.text;
            }
        }
    }
}
