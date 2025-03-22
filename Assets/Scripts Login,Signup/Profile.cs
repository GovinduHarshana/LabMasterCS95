using System.Collections;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;
using UnityEngine.SceneManagement; 

public class ProfileManager : MonoBehaviour
{
    public InputField nameField, emailField, dobField, roleField, mobileField, nicField;
    public Image profilePicture;
    public string userId = "USER_ID_HERE";  
    private string apiUrl = "https://lab-master-backend.vercel.app/api/profile/";

    void Start()
    {
        StartCoroutine(FetchProfileData());
    }

    IEnumerator FetchProfileData()
    {
        UnityWebRequest request = UnityWebRequest.Get(apiUrl + userId);
        yield return request.SendWebRequest();

        if (request.result == UnityWebRequest.Result.Success)
        {
            string jsonResponse = request.downloadHandler.text;
            UserProfile user = JsonUtility.FromJson<UserProfile>(jsonResponse);

            nameField.text = user.name;
            emailField.text = user.email;
            dobField.text = user.dob;
            roleField.text = user.userRole;
            mobileField.text = user.mobileNumber;
            nicField.text = user.nic;
        }
        else
        {
            Debug.LogError("Error fetching profile: " + request.error);
        }
    }

    public void UpdateProfile()
    {
        StartCoroutine(UpdateProfileData());
    }

    IEnumerator UpdateProfileData()
    {
        UserProfile updatedUser = new UserProfile()
        {
            name = nameField.text,
            email = emailField.text,
            dob = dobField.text,
            userRole = roleField.text,
            mobileNumber = mobileField.text,
            nic = nicField.text
        };

        string jsonData = JsonUtility.ToJson(updatedUser);
        UnityWebRequest request = UnityWebRequest.Put(apiUrl + "update/" + userId, jsonData);
        request.SetRequestHeader("Content-Type", "application/json");

        yield return request.SendWebRequest();

        if (request.result == UnityWebRequest.Result.Success)
        {
            Debug.Log("Profile updated successfully!");
            NavigateToAccountSettings();
        }
        else
        {
            Debug.LogError("Error updating profile: " + request.error);
        }
    }

    public void NavigateToAccountSettings()
    {
        
        SceneManager.LoadScene("AccountSettingsScene");
    }
}

[System.Serializable]
public class UserProfile
{
    public string name;
    public string email;
    public string dob;
    public string userRole;
    public string mobileNumber;
    public string nic;
}
