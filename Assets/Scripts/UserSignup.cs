using UnityEngine;
using UnityEngine.Networking;
using System.Collections;
using TMPro;  // For UI input fields

public class UserSignup : MonoBehaviour
{
    // Input fields for user registration
    public TMP_InputField usernameField;
    public TMP_InputField passwordField;
    public TMP_InputField reEnterPasswordField;
    public TMP_InputField nameField;
    public TMP_InputField emailField;
    public TMP_InputField dobField;  // Date of Birth (format: YYYY-MM-DD)
    public TMP_Dropdown roleDropdown; // Dropdown for selecting Student/Teacher

    private string registerUrl = "http://localhost/labmasterdatabase/register.php";

    public void Register()
    {
        // Get values from input fields
        string username = usernameField.text;
        string password = passwordField.text;
        string reEnterPassword = reEnterPasswordField.text;
        string name = nameField.text;
        string email = emailField.text;
        string dob = dobField.text;  // Date of Birth input
        string userRole = roleDropdown.options[roleDropdown.value].text; // Dropdown selection

        // Call coroutine to send data to server
        StartCoroutine(RegisterCoroutine(username, password, reEnterPassword, name, email, dob, userRole));
    }

    IEnumerator RegisterCoroutine(string username, string password, string reEnterPassword, string name, string email, string dob, string userRole)
    {
        // Create a form to send data via POST
        WWWForm form = new WWWForm();
        form.AddField("username", username);
        form.AddField("password", password);
        form.AddField("re_enterPassword", reEnterPassword);
        form.AddField("name", name);
        form.AddField("email", email);
        form.AddField("dob", dob);  // Send DOB
        form.AddField("userRole", userRole);

        // Send request to server
        using (UnityWebRequest www = UnityWebRequest.Post(registerUrl, form))
        {
            yield return www.SendWebRequest();

            if (www.result == UnityWebRequest.Result.Success)
            {
                Debug.Log("Register Response: " + www.downloadHandler.text);
            }
            else
            {
                Debug.LogError("Error: " + www.error);
            }
        }
    }
}
