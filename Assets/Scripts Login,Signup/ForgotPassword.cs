using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Collections;
using System.Text;
using UnityEngine.Networking;

public class ForgotPassword : MonoBehaviour
{
    public TMP_InputField emailInput;
    public TMP_Text errorMessageText;
    public TMP_Text successMessageText;
    public Button sendButton;
    public string forgotPasswordServerUrl = "https://lab-master-backend.vercel.app/api/auth/forgot-password";

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
        string jsonData = JsonUtility.ToJson(new { email });
        byte[] jsonBytes = Encoding.UTF8.GetBytes(jsonData);

        UnityWebRequest request = new UnityWebRequest(forgotPasswordServerUrl, "POST");
        request.uploadHandler = new UploadHandlerRaw(jsonBytes);
        request.downloadHandler = new DownloadHandlerBuffer();
        request.SetRequestHeader("Content-Type", "application/json");

        yield return request.SendWebRequest();

        if (request.result == UnityWebRequest.Result.Success)
        {
            successMessageText.text = "Check your email for the reset token!";
            errorMessageText.text = "";
        }
        else
        {
            successMessageText.text = "";
            errorMessageText.text = "User not found or server error.";
        }
    }
}
