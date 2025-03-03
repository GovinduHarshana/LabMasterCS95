using UnityEngine;
using TMPro;  
using UnityEngine.UI;
using System.Collections;
using System.Text;
using UnityEngine.Networking;
using System.Text.RegularExpressions;

public class ResetPassword : MonoBehaviour
{
    public TMP_InputField newPasswordInput;
    public TMP_InputField confirmPasswordInput;
    public TextMeshProUGUI errorMessage;
    public GameObject successPopup;
    public TextMeshProUGUI successMessage;
    public Button resetPasswordButton; 

    private string apiUrl = "http://localhost:5000/reset-password";
    private string resetToken = "";

    void Start()
    {
        resetToken = PlayerPrefs.GetString("ResetToken", "");

        if (string.IsNullOrEmpty(resetToken))
        {
            Debug.LogError("❌ Reset Token is missing! Make sure it is saved correctly in PlayerPrefs.");
            errorMessage.text = "Invalid reset request. Please try again.";
            return;  // Stop execution if no token is available
        }

        // Ensure UI elements are correctly assigned
        if (newPasswordInput == null || confirmPasswordInput == null || errorMessage == null ||
            successPopup == null || successMessage == null || resetPasswordButton == null)
        {
            Debug.LogError("❌ One or more UI elements are missing! Please assign them in the Inspector.");
            return;
        }

        successPopup.SetActive(false);  // Hide popup at the start

        // Attach button click event programmatically
        resetPasswordButton.onClick.AddListener(ResetUserPassword);
    }


    public void ResetUserPassword()
    {
        string newPassword = newPasswordInput.text.Trim();
        string confirmPassword = confirmPasswordInput.text.Trim();

        errorMessage.text = ""; // Clear previous error messages

        if (string.IsNullOrEmpty(newPassword) || string.IsNullOrEmpty(confirmPassword))
        {
            errorMessage.text = "⚠ All fields must be filled!";
            return;
        }

        if (!IsValidPassword(newPassword))
        {
            errorMessage.text = "⚠ Password must be at least 8 characters, include 1 uppercase, 1 number, and 1 special character.";
            return;
        }

        if (newPassword != confirmPassword)
        {
            errorMessage.text = "⚠ Passwords do not match!";
            return;
        }

        StartCoroutine(ResetPasswordRequest(resetToken, newPassword));
    }

    bool IsValidPassword(string password)
    {
        string pattern = @"^(?=.*[A-Z])(?=.*\d)(?=.*[@$!%*?&])[A-Za-z\d@$!%*?&]{8,}$";
        return Regex.IsMatch(password, pattern);
    }

    IEnumerator ResetPasswordRequest(string token, string newPassword)
    {
        string jsonData = "{\"token\":\"" + token + "\", \"newPassword\":\"" + newPassword + "\"}";
        byte[] jsonBytes = Encoding.UTF8.GetBytes(jsonData);

        UnityWebRequest request = new UnityWebRequest(apiUrl, "POST");
        request.uploadHandler = new UploadHandlerRaw(jsonBytes);
        request.downloadHandler = new DownloadHandlerBuffer();
        request.SetRequestHeader("Content-Type", "application/json");

        yield return request.SendWebRequest();

        if (request.result == UnityWebRequest.Result.Success)
        {
            Debug.Log("Password reset successful: " + request.downloadHandler.text);
            successPopup.SetActive(true);
            successMessage.text = "Password reset successful! You can now log in with your new password.";
        }
        else
        {
            Debug.LogError("Reset password failed: " + request.downloadHandler.text);
            errorMessage.text = "Error: " + request.downloadHandler.text;
        }
    }

    
    public void GoBackToLogin()
    {
        UnityEngine.SceneManagement.SceneManager.LoadScene("LoginScene");
    }
}
