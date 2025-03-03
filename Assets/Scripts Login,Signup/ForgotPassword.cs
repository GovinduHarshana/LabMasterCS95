using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Collections;
using System.Text;
using UnityEngine.Networking;

public class ForgotPassword : MonoBehaviour
{
    public TMP_InputField emailInput;   // Email input field
    public TMP_Text errorMessageText;   // Text to show errors
    public TMP_Text successMessageText; // Text to show success message
    public Button sendButton;           // Send button

    private string forgotPasswordURL = "http://localhost:5000/forgot-password";

    void Start()
    {
        errorMessageText.text = "";
        successMessageText.text = "";
        sendButton.onClick.AddListener(OnSendButtonClicked);
    }

    void OnSendButtonClicked()
    {
        string email = emailInput.text.Trim();

        if (string.IsNullOrEmpty(email))
        {
            errorMessageText.text = "Email is required.";
            successMessageText.text = "";
            return;
        }

        if (!IsValidEmail(email))
        {
            errorMessageText.text = "Invalid email format.";
            successMessageText.text = "";
            return;
        }

        errorMessageText.text = "";
        StartCoroutine(SendForgotPasswordRequest(email));
    }

    bool IsValidEmail(string email)
    {
        return System.Text.RegularExpressions.Regex.IsMatch(email, @"^[^@\s]+@[^@\s]+\.[^@\s]+$");
    }

    IEnumerator SendForgotPasswordRequest(string email)
    {
        // Create JSON payload
        string jsonData = "{\"email\": \"" + email + "\"}";
        byte[] jsonBytes = Encoding.UTF8.GetBytes(jsonData);

        // Create request
        UnityWebRequest request = new UnityWebRequest(forgotPasswordURL, "POST");
        request.uploadHandler = new UploadHandlerRaw(jsonBytes);
        request.downloadHandler = new DownloadHandlerBuffer();
        request.SetRequestHeader("Content-Type", "application/json");

        // Send request
        yield return request.SendWebRequest();

        if (request.result == UnityWebRequest.Result.Success)
        {
            successMessageText.text = "Check your email for the reset link!";
            errorMessageText.text = "";
        }
        else
        {
            successMessageText.text = "";
            errorMessageText.text = "User not found.";
        }
    }
}
