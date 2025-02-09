using UnityEngine;
using UnityEngine.Networking;
using System.Collections;
using TMPro;

public class LoginUser : MonoBehaviour
{
    // UI elements for input fields
    public TMP_InputField usernameField;
    public TMP_InputField passwordField;
    public TextMeshProUGUI loginStatusText; // To show login success or failure

    private string loginUrl = "http://localhost/labmasterdatabase/login.php";

    public void Login()
    {
        string username = usernameField.text;
        string password = passwordField.text;

        StartCoroutine(LoginCoroutine(username, password));
    }

    IEnumerator LoginCoroutine(string username, string password)
    {
        WWWForm form = new WWWForm();
        form.AddField("username", username);
        form.AddField("password", password);

        using (UnityWebRequest www = UnityWebRequest.Post(loginUrl, form))
        {
            yield return www.SendWebRequest();

            if (www.result == UnityWebRequest.Result.Success)
            {
                string responseText = www.downloadHandler.text;
                Debug.Log("Login Response: " + responseText);

                if (responseText.Contains("Login successful"))
                {
                    loginStatusText.text = "Login Successful!";
                }
                else
                {
                    loginStatusText.text = "Invalid Username or Password!";
                }
            }
            else
            {
                Debug.LogError("Error: " + www.error);
                loginStatusText.text = "Network Error!";
            }
        }
    }
}
