using UnityEngine;
using TMPro;
using UnityEngine.UI;
using System.Collections;
using UnityEngine.Networking;

public class UserDataManager : MonoBehaviour
{
    public static UserDataManager Instance; // Singleton pattern

    public string userName;
    public string userRole;
    

    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject); // Persist across scenes
        }
        else
        {
            Destroy(gameObject); // Ensure only one instance exists
        }
    }

    // Method to load user data from the backend
    public IEnumerator LoadUserData(string userId)
    {
        string baseUrl = "https://lab-master-backend.vercel.app/api/auth/profile";
        string url = $"{baseUrl}/{userId}";
        UnityWebRequest request = UnityWebRequest.Get(url);
        request.SetRequestHeader("Content-Type", "application/json");

        yield return request.SendWebRequest();

        if (request.result == UnityWebRequest.Result.Success)
        {
            string jsonResponse = request.downloadHandler.text;
            UserProfile profile = JsonUtility.FromJson<UserProfile>(jsonResponse);

            userName = profile.name;
            userRole = profile.userRole;

            // Notify other scripts that data is loaded (optional)
            OnUserDataLoaded();
        }
        else
        {
            Debug.LogError("Error loading user data: " + request.error);
        }
    }

    // Event or method to notify other scripts that data is loaded (optional)
    public delegate void UserDataLoadedAction();
    public static event UserDataLoadedAction OnUserDataLoaded;

    [System.Serializable]
    public class UserProfile
    {
        public string name;
        public string email;
        public string dob;
        public string userRole;
        public string mobileNumber;
        public string nic;
        public string password;
        public string profilePicture;
    }
}