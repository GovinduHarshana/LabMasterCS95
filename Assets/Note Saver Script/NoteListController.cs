using System;
using System.Collections;
using System.Text;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Networking;
using UnityEngine.SceneManagement;

public class NoteListController : MonoBehaviour
{
    [SerializeField] private Transform notesContainer;
    [SerializeField] private GameObject noteItemPrefab;
    [SerializeField] private Button createNewNoteButton;
    [SerializeField] private Button backToHomeButton;
    [SerializeField] private string apiBaseUrl = "https://lab-master-backend.vercel.app/api";

    private string userId;
    private string authToken;

    void Start()
    {
        // Get userId from PlayerPrefs (set during login)
        userId = PlayerPrefs.GetString("userId", "");
        
        if (string.IsNullOrEmpty(userId))
        {
            Debug.LogWarning("No user ID found in PlayerPrefs. User may not be logged in properly.");
        }
        
        // Set up button listeners
        if (createNewNoteButton != null)
        {
            createNewNoteButton.onClick.AddListener(OnCreateNewNote);
        }
        
        // Fetch user notes
        GetUserNotes();
    }

    public void OpenNoteSaverFromNoteList()
    {
        PlayerPrefs.Save();
        SceneManager.LoadScene("NoteSaver");
    }

    public void BackToHomeFromNoteList()
    {
        PlayerPrefs.Save();
        SceneManager.LoadScene("HomePageNew");
    }

    private void OnCreateNewNote()
    {
        if (NoteSceneManager.Instance != null)
        {
            NoteSceneManager.Instance.LoadCreateNewNote();
        }
    }
    
    // Method to retrieve user's notes
    public void GetUserNotes()
    {
        if (string.IsNullOrEmpty(userId))
        {
            Debug.LogError("Cannot retrieve notes: User ID not set");
            return;
        }

        StartCoroutine(FetchUserNotes());
    }

    private IEnumerator FetchUserNotes()
    {
        string fetchNotesUrl = $"{apiBaseUrl}/note/retrieveNote";

        // Create a JSON object to send in the request body
        NoteRequest noteRequest = new NoteRequest
        {
            userId = userId
        };
        string jsonData = JsonUtility.ToJson(noteRequest);
        Debug.Log($"Retrieving notes for userId: {userId}");

        UnityWebRequest request = new UnityWebRequest(fetchNotesUrl, "POST");
        byte[] bodyRaw = Encoding.UTF8.GetBytes(jsonData);
        request.uploadHandler = new UploadHandlerRaw(bodyRaw);
        request.downloadHandler = new DownloadHandlerBuffer();

        request.SetRequestHeader("Content-Type", "application/json");

        if (!string.IsNullOrEmpty(authToken))
        {
            request.SetRequestHeader("Authorization", "Bearer " + authToken);
        }

        yield return request.SendWebRequest();

        if (request.result == UnityWebRequest.Result.Success)
        {
            string jsonResponse = request.downloadHandler.text;
            Debug.Log("Notes retrieved: " + jsonResponse);

            try
            {
                // Parse the JSON response
                NotesResponse response = JsonUtility.FromJson<NotesResponse>(jsonResponse);

                // Handle and display the notes in your UI
                if (response != null && response.notes != null && response.notes.Length > 0)
                {
                    Debug.Log($"Retrieved {response.notes.Length} notes");
                    DisplayNotesInUI(response.notes);
                }
                else
                {
                    Debug.Log("No notes found for this user");
                    ClearNotesContainer();
                }
            }
            catch (Exception e)
            {
                Debug.LogError($"Error parsing JSON response: {e.Message}");
                Debug.LogError($"Raw response: {jsonResponse}");
            }
        }
        else
        {
            Debug.LogError($"Error retrieving notes: {request.error}");
            Debug.LogError($"Response: {request.downloadHandler.text}");
        }
    }

    // Method to clear the notes container
    private void ClearNotesContainer()
    {
        if (notesContainer != null)
        {
            foreach (Transform child in notesContainer)
            {
                Destroy(child.gameObject);
            }
        }
    }

    // Method to display notes in the UI
    private void DisplayNotesInUI(NoteData[] notes)
    {
        // Clear existing note items
        ClearNotesContainer();

        // Check if the container exists
        if (notesContainer == null || noteItemPrefab == null)
        {
            Debug.LogError("Notes container or note item prefab is not assigned!");
            return;
        }

        // Create new note items
        foreach (NoteData note in notes)
        {
            GameObject noteItem = Instantiate(noteItemPrefab, notesContainer);
            NoteItemController controller = noteItem.GetComponent<NoteItemController>();

            if (controller != null)
            {
                controller.Initialize(note.noteId, note.title, note.content);
            }
            else
            {
                Debug.LogError("NoteItemController component not found on the note item prefab!");
            }
        }
    }

    [Serializable]
    public class NoteRequest
    {
        public string userId;
    }

    [Serializable]
    public class NoteData
    {
        public string noteId;
        public string userId;
        public string title;
        public string content;
        public string createdAt;
    }

    [Serializable]
    public class NotesResponse
    {
        public NoteData[] notes;
    }
}