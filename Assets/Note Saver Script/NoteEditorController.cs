using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Text;
using UnityEngine.Networking;
using UnityEngine.EventSystems;

public class NoteEditorController : MonoBehaviour
{
    [Header("Text Fields")]
    [SerializeField] private TMP_InputField titleField;
    [SerializeField] private TMP_InputField textAreaField;
    [SerializeField] private TMP_Text textAreaNameDisplay;

    [Header("Basic Formatting Buttons")]
    [SerializeField] private Button boldButton;
    [SerializeField] private Button italicButton;
    [SerializeField] private Button underlineButton;
    [SerializeField] private Button strikethroughButton;
    [SerializeField] private Button subscriptButton;
    [SerializeField] private Button superscriptButton;
    [SerializeField] private Button bulletListButton;
    [SerializeField] private Button numberListButton;
    [SerializeField] private Button undoButton;
    [SerializeField] private Button redoButton;
    [SerializeField] private Button saveButton;

    [Header("New Buttons")]
    [SerializeField] private Button highlightButton;
    [SerializeField] private Button fontColorButton;

    [Header("Color Buttons")]
    [SerializeField] private GameObject colorPanel;
    [SerializeField] private Button redColorButton;
    [SerializeField] private Button greenColorButton;
    [SerializeField] private Button blueColorButton;
    [SerializeField] private Button blackColorButton;

    [Header("API Settings")]
    [SerializeField] private string apiBaseUrl = "https://lab-master-backend.vercel.app/api";
    private string userId = ""; // Retrieved from PlayerPrefs
    private string authToken = ""; // Set after login
    private string currentNoteId = ""; // Track the currently loaded note

    // For undo/redo functionality
    private List<string> undoStack = new List<string>();
    private List<string> redoStack = new List<string>();
    private bool isUndoRedoAction = false;

    private bool isbulletListButtton = false;
    private bool isnumberListButtton = false;

    private int currentLineNumber = 1;

    // Constants
    private const int MAX_UNDO_STEPS = 30;

    void Start()
    {
        // Get userId from PlayerPrefs (set during login)
        userId = PlayerPrefs.GetString("userId", "");
        string userName = PlayerPrefs.GetString("userName", "Unknown User");

        // Check if user is logged in
        if (string.IsNullOrEmpty(userId))
        {
            Debug.LogWarning("No user ID found in PlayerPrefs. User may not be logged in properly.");
        }
        else
        {
            Debug.Log($"User logged in: {userName} (ID: {userId})");
        }

        // Initialize state
        SaveCurrentStateForUndo();

        // Add listeners for input changes to track undo/redo state
        textAreaField.onValueChanged.AddListener(OnTextChanged);
        titleField.onValueChanged.AddListener(OnTitleChanged);

        colorPanel.SetActive(false);

        // Set up button listeners
        if (saveButton != null && saveButton.onClick.GetPersistentEventCount() == 0)
        {
            saveButton.onClick.AddListener(OnSaveButtonClick);
        }

        // Connect color button listeners
        if (redColorButton != null)
        {
            redColorButton.onClick.AddListener(OnRedColorButtonClick);
        }

        if (greenColorButton != null)
        {
            greenColorButton.onClick.AddListener(OnGreenColorButtonClick);
        }

        if (blueColorButton != null)
        {
            blueColorButton.onClick.AddListener(OnBlueColorButtonClick);
        }

        if (blackColorButton != null)
        {
            blackColorButton.onClick.AddListener(OnBlackColorButtonClick);
        }

        if (NoteSceneManager.Instance != null)
        {
            string loadedNoteId = NoteSceneManager.Instance.GetCurrentNoteId();
            if (!string.IsNullOrEmpty(loadedNoteId))
            {
                // Load the existing note data
                currentNoteId = loadedNoteId;
                titleField.text = NoteSceneManager.Instance.GetCurrentNoteTitle();
                textAreaField.text = NoteSceneManager.Instance.GetCurrentNoteContent();

                // Reset undo/redo state
                undoStack.Clear();
                redoStack.Clear();
                SaveCurrentStateForUndo();
            }
        }

        // Log configured endpoints for debugging
        Debug.Log($"API Base URL: {apiBaseUrl}");
        Debug.Log($"Create Note Endpoint: {apiBaseUrl}/note/createNote");
        Debug.Log($"Retrieve Notes Endpoint: {apiBaseUrl}/note/retrieveNote");
    }

    void Update()
    {
        // Handle keyboard shortcuts
        HandleKeyboardShortcuts();
    }

    private void HandleKeyboardShortcuts()
    {
        // Only process shortcuts if the text area has focus
        if (!textAreaField.isFocused)
            return;

        // Undo: Ctrl+Z
        if (Input.GetKey(KeyCode.LeftControl) && Input.GetKeyDown(KeyCode.Z))
        {
            OnUndoButtonClick();
        }

        // Redo: Ctrl+Y
        if (Input.GetKey(KeyCode.LeftControl) && Input.GetKeyDown(KeyCode.Y))
        {
            OnRedoButtonClick();
        }

        // Bold: Ctrl+B
        if (Input.GetKey(KeyCode.LeftControl) && Input.GetKeyDown(KeyCode.B))
        {
            OnBoldButtonClick();
        }

        // Italic: Ctrl+I
        if (Input.GetKey(KeyCode.LeftControl) && Input.GetKeyDown(KeyCode.I))
        {
            OnItalicButtonClick();
        }

        // Underline: Ctrl+U
        if (Input.GetKey(KeyCode.LeftControl) && Input.GetKeyDown(KeyCode.U))
        {
            OnUnderlineButtonClick();
        }

        // Save: Ctrl+S
        if (Input.GetKey(KeyCode.LeftControl) && Input.GetKeyDown(KeyCode.S))
        {
            OnSaveButtonClick();
        }
    }

    private void OnTextChanged(string newText)
    {
        if (!isUndoRedoAction)
        {
            SaveCurrentStateForUndo();
            redoStack.Clear();
        }
        isUndoRedoAction = false;
    }

    private void OnTitleChanged(string newTitle)
    {
        // Update the title display if available
        if (textAreaNameDisplay != null)
        {
            textAreaNameDisplay.text = string.IsNullOrEmpty(newTitle) ? "Untitled" : newTitle;
        }
    }

    private void SaveCurrentStateForUndo()
    {
        undoStack.Add(textAreaField.text);
        if (undoStack.Count > MAX_UNDO_STEPS)
        {
            undoStack.RemoveAt(0);
        }
    }

    // PUBLIC BUTTON HANDLERS - Connect these in the Inspector

    public void OnUndoButtonClick()
    {
        if (undoStack.Count > 1) // Keep at least one state in undo stack
        {
            // Move current state to redo stack
            redoStack.Add(undoStack[undoStack.Count - 1]);
            undoStack.RemoveAt(undoStack.Count - 1);

            // Apply previous state
            isUndoRedoAction = true;
            textAreaField.text = undoStack[undoStack.Count - 1];
        }
    }

    public void OnRedoButtonClick()
    {
        if (redoStack.Count > 0)
        {
            // Get last redo state
            string redoState = redoStack[redoStack.Count - 1];
            redoStack.RemoveAt(redoStack.Count - 1);

            // Apply redo state
            isUndoRedoAction = true;
            textAreaField.text = redoState;

            // Add to undo stack
            undoStack.Add(redoState);
        }
    }

    public void OnBoldButtonClick()
    {
        ApplyFormatting("b");
    }

    public void OnItalicButtonClick()
    {
        ApplyFormatting("i");
    }

    public void OnUnderlineButtonClick()
    {
        ApplyFormatting("u");
    }

    public void OnStrikethroughButtonClick()
    {
        ApplyFormatting("s");
    }

    public void OnSubscriptButtonClick()
    {
        ApplyFormatting("sub");
    }

    public void OnSuperscriptButtonClick()
    {
        ApplyFormatting("sup");
    }

    public void OnBulletListButtonClick()
    {
        ApplyListFormatting(true);
    }

    public void OnNumberListButtonClick()
    {
        ApplyListFormatting(false);
    }

    public void OnSaveButtonClick()
    {
        SaveNote();
    }

    // Method to handle font color button click
    public void OnFontColorButtonClick()
    {
        colorPanel.SetActive(true); // Show the color panel
    }

    // Method to handle red color button click
    public void OnRedColorButtonClick()
    {
        ApplyColorFormatting("color", "#FF0000"); // Red
        colorPanel.SetActive(false); // Hide the color panel
    }

    // Method to handle black color button click
    public void OnBlackColorButtonClick()
    {
        ApplyColorFormatting("color", "#000000"); // Black
        colorPanel.SetActive(false); // Hide the color panel
    }

    // Method to handle green color button click
    public void OnGreenColorButtonClick()
    {
        ApplyColorFormatting("color", "#00FF00"); // Green
        colorPanel.SetActive(false); // Hide the color panel
    }

    // Method to handle blue color button click
    public void OnBlueColorButtonClick()
    {
        ApplyColorFormatting("color", "#0000FF"); // Blue
        colorPanel.SetActive(false); // Hide the color panel
    }

    private void ApplyColorFormatting(string tag, string colorCode)
    {
        int selectionStart = textAreaField.selectionStringAnchorPosition;
        int selectionEnd = textAreaField.selectionStringFocusPosition;

        if (selectionStart > selectionEnd)
        {
            int temp = selectionStart;
            selectionStart = selectionEnd;
            selectionEnd = temp;
        }

        if (selectionStart == selectionEnd) return; // No text selected

        string text = textAreaField.text;
        string selectedText = text.Substring(selectionStart, selectionEnd - selectionStart);

        textAreaField.richText = true;
        string formattedText = $"<{tag}={colorCode}>{selectedText}</{tag}>";

        textAreaField.text = text.Substring(0, selectionStart) + formattedText + text.Substring(selectionEnd);
        SaveCurrentStateForUndo();

        int newCursorPos = selectionStart + formattedText.Length;
        textAreaField.caretPosition = newCursorPos;
        textAreaField.selectionStringAnchorPosition = newCursorPos; //Reset selection so the next typed text will not be formatted
        textAreaField.selectionStringFocusPosition = newCursorPos; //Reset selection so the next typed text will not be formatted
    }

    private void ApplyFormatting(string tag)
    {
        // Get selection
        int selectionStart = textAreaField.selectionStringAnchorPosition;
        int selectionEnd = textAreaField.selectionStringFocusPosition;

        // Make sure we have valid selection (if not, cursor position)
        if (selectionStart > selectionEnd)
        {
            int temp = selectionStart;
            selectionStart = selectionEnd;
            selectionEnd = temp;
        }

        string text = textAreaField.text;

        // Check if there is an actual selection
        if (selectionStart == selectionEnd)
        {
            Debug.Log("No text selected for formatting.");
            return;
        }

        string selectedText = text.Substring(selectionStart, selectionEnd - selectionStart);

        // Configure the input field to support rich text
        textAreaField.richText = true;

        // Format the text with rich text tags
        string formattedText = $"<{tag}>{selectedText}</{tag}>";

        // Replace the selected text with the formatted text
        textAreaField.text = text.Substring(0, selectionStart) + formattedText + text.Substring(selectionEnd);

        // Update undo stack
        SaveCurrentStateForUndo();

        // Reset the selection/cursor position after applying formatting
        int newCursorPosition = selectionStart + formattedText.Length;
        textAreaField.caretPosition = newCursorPosition;
        textAreaField.selectionStringAnchorPosition = newCursorPosition;
        textAreaField.selectionStringFocusPosition = newCursorPosition;
    }

    private void ApplyListFormatting(bool isBullet)
    {
        string text = textAreaField.text;
        int selectionStart = textAreaField.selectionStringAnchorPosition;
        int selectionEnd = textAreaField.selectionStringFocusPosition;

        if (selectionStart > selectionEnd)
        {
            int temp = selectionStart;
            selectionStart = selectionEnd;
            selectionEnd = temp;
        }

        if (selectionStart == selectionEnd)
        {
            selectionStart = 0;
            selectionEnd = text.Length;
        }

        string selectedText = text.Substring(selectionStart, selectionEnd - selectionStart);
        string[] lines = selectedText.Split('\n');
        StringBuilder newText = new StringBuilder();

        if (isBullet)
        {
            // If bullet list is requested, turn off numbered list
            if (isnumberListButtton)
            {
                isnumberListButtton = false;
                // Remove numbered list formatting from selected text
                selectedText = RemoveListFormatting(selectedText, false);
                lines = selectedText.Split('\n'); // Re-split after removing
            }

            isbulletListButtton = !isbulletListButtton; // Toggle bullet list

            if (isbulletListButtton)
            {
                for (int i = 0; i < lines.Length; i++)
                {
                    newText.AppendLine($"• {lines[i]}");
                }
            }
            else
            {
                // Remove bullet points
                selectedText = RemoveListFormatting(selectedText, true);
                lines = selectedText.Split('\n');
                newText.Append(selectedText);
            }
        }
        else
        {
            // If numbered list is requested, turn off bullet list
            if (isbulletListButtton)
            {
                isbulletListButtton = false;
                // Remove bullet list formatting from selected text
                selectedText = RemoveListFormatting(selectedText, true);
                lines = selectedText.Split('\n'); // Re-split after removing
            }

            isnumberListButtton = !isnumberListButtton; // Toggle numbered list

            if (isnumberListButtton)
            {
                for (int i = 0; i < lines.Length; i++)
                {
                    newText.AppendLine($"{i + 1}. {lines[i]}");
                }
            }
            else
            {
                // Remove numbered list
                selectedText = RemoveListFormatting(selectedText, false);
                lines = selectedText.Split('\n');
                newText.Append(selectedText);
            }
        }

        textAreaField.text = text.Substring(0, selectionStart) + newText.ToString().TrimEnd('\n') + text.Substring(selectionEnd);
        SaveCurrentStateForUndo();
    }

    // Helper method to remove list formatting
    private string RemoveListFormatting(string text, bool isBullet)
    {
        string[] lines = text.Split('\n');
        StringBuilder cleanText = new StringBuilder();

        if (isBullet)
        {
            foreach (string line in lines)
            {
                if (line.StartsWith("• "))
                {
                    cleanText.AppendLine(line.Substring(2));
                }
                else
                {
                    cleanText.AppendLine(line);
                }
            }
        }
        else
        {
            foreach (string line in lines)
            {
                // Remove numbered list patterns (e.g., "1. ", "2. ", etc.)
                int dotIndex = line.IndexOf(". ");
                if (dotIndex > 0 && int.TryParse(line.Substring(0, dotIndex), out _))
                {
                    cleanText.AppendLine(line.Substring(dotIndex + 2));
                }
                else
                {
                    cleanText.AppendLine(line);
                }
            }
        }

        return cleanText.ToString().TrimEnd('\n');
    }

    private void SaveNote()
    {
        string title = titleField.text;
        string content = textAreaField.text;

        if (string.IsNullOrEmpty(title))
        {
            Debug.LogError("Title cannot be empty!");
            return;
        }

        // Check if user is logged in (userId available from PlayerPrefs)
        if (string.IsNullOrEmpty(userId))
        {
            Debug.LogError("User ID is empty. User may not be logged in.");
            return;
        }

        Debug.Log($"Saving note with title: {title}, content length: {content.Length} characters, for user: {userId}");

        if (string.IsNullOrEmpty(currentNoteId))
        {
            // Create new note
            StartCoroutine(CreateNoteInDatabase(title, content));
        }
        else
        {
            // Update existing note
            StartCoroutine(UpdateNoteInDatabase(currentNoteId, title, content));
        }
    }

    private IEnumerator CreateNoteInDatabase(string title, string content)
    {
        // Create the data object to send to the server
        NoteData noteData = new NoteData
        {
            userId = userId,
            title = title,
            content = content
        };

        // Convert to JSON
        string jsonData = JsonUtility.ToJson(noteData);
        Debug.Log("Sending note data: " + jsonData);

        // Create the web request with the correct endpoint
        string createNoteUrl = $"{apiBaseUrl}/note/createNote";
        UnityWebRequest request = new UnityWebRequest(createNoteUrl, "POST");
        byte[] bodyRaw = Encoding.UTF8.GetBytes(jsonData);
        request.uploadHandler = new UploadHandlerRaw(bodyRaw);
        request.downloadHandler = new DownloadHandlerBuffer();

        // Set headers
        request.SetRequestHeader("Content-Type", "application/json");
        if (!string.IsNullOrEmpty(authToken))
        {
            request.SetRequestHeader("Authorization", "Bearer " + authToken);
        }

        // Send the request
        yield return request.SendWebRequest();

        // Handle the response
        if (request.result == UnityWebRequest.Result.Success)
        {
            Debug.Log("Note saved successfully: " + request.downloadHandler.text);

            // Parse response to get the note ID
            try
            {
                NoteResponse response = JsonUtility.FromJson<NoteResponse>(request.downloadHandler.text);
                if (response != null && !string.IsNullOrEmpty(response.noteId))
                {
                    currentNoteId = response.noteId;
                    Debug.Log($"New note created with ID: {currentNoteId}");
                }
            }
            catch (Exception e)
            {
                Debug.LogError($"Error parsing response: {e.Message}");
            }

            // Show a success message to the user (you could add a UI element for this)
            Debug.Log("Note saved successfully!");
        }
        else
        {
            Debug.LogError($"Error saving note: {request.error}");
            Debug.LogError($"Response: {request.downloadHandler.text}");
        }
    }

    private IEnumerator UpdateNoteInDatabase(string noteId, string title, string content)
    {
        // Create the data object to send to the server
        UpdateNoteData updateData = new UpdateNoteData
        {
            noteId = noteId,
            userId = userId,
            title = title,
            content = content
        };

        // Convert to JSON
        string jsonData = JsonUtility.ToJson(updateData);
        Debug.Log($"Updating note {noteId}: " + jsonData);

        // Create the web request with the correct endpoint
        string updateNoteUrl = $"{apiBaseUrl}/note/updateNote";
        UnityWebRequest request = new UnityWebRequest(updateNoteUrl, "PUT");
        byte[] bodyRaw = Encoding.UTF8.GetBytes(jsonData);
        request.uploadHandler = new UploadHandlerRaw(bodyRaw);
        request.downloadHandler = new DownloadHandlerBuffer();

        // Set headers
        request.SetRequestHeader("Content-Type", "application/json");
        if (!string.IsNullOrEmpty(authToken))
        {
            request.SetRequestHeader("Authorization", "Bearer " + authToken);
        }

        // Send the request
        yield return request.SendWebRequest();

        // Handle the response
        if (request.result == UnityWebRequest.Result.Success)
        {
            Debug.Log("Note updated successfully: " + request.downloadHandler.text);
            // Show a success message to the user
        }
        else
        {
            Debug.LogError($"Error updating note: {request.error}");
            Debug.LogError($"Response: {request.downloadHandler.text}");
        }
    }

    // Method to show notes panel and retrieve user's notes
    public void ShowNotesPanel()
    {
        if (NoteSceneManager.Instance != null)
        {
            NoteSceneManager.Instance.LoadNotesListScene();
        }
        else
        {
            Debug.LogError("NoteSceneManager instance not found!");
        }
    }

    
    // Method to create a new note
    public void CreateNewNote()
    {
        // Clear the editor
        titleField.text = "";
        textAreaField.text = "";
        currentNoteId = ""; // Reset current note ID

        // Reset undo/redo state
        undoStack.Clear();
        redoStack.Clear();
        SaveCurrentStateForUndo();
    }

    // Method to load a note into the editor
    public void LoadNoteToEditor(string noteId, string title, string content)
    {
        currentNoteId = noteId;
        titleField.text = title;
        textAreaField.text = content;

        // Reset undo/redo state
        undoStack.Clear();
        redoStack.Clear();
        SaveCurrentStateForUndo();
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
        string fetchNotesUrl = $"{apiBaseUrl}/note/retrieveNote"; // Changed to match backend

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
                    
                }
                else
                {
                    Debug.Log("No notes found for this user");
                    
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

    

    // Define data structures for JSON serialization/deserialization
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
    public class UpdateNoteData
    {
        public string noteId;
        public string userId;
        public string title;
        public string content;
    }

    [Serializable]
    public class NoteResponse
    {
        public string noteId;
    }

    [Serializable]
    public class NotesResponse
    {
        public NoteData[] notes;
    }
}