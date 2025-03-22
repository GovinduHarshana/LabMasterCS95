using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Text;
using UnityEngine.Networking;
using System;
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

    [Header("MongoDB Settings")]
    [SerializeField] private string mongoDBAPIUrl = "https://yourapi.com/savenote";

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
        // Initialize state
        SaveCurrentStateForUndo();

        // Add listeners for input changes to track undo/redo state
        textAreaField.onValueChanged.AddListener(OnTextChanged);
        titleField.onValueChanged.AddListener(OnTitleChanged);

        colorPanel.SetActive(false);

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

    // Method to handle red color button click
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
            // You might want to show a UI message to the user here
            return;
        }

        StartCoroutine(UploadNoteToMongoDB(title, content));
    }

    private IEnumerator UploadNoteToMongoDB(string title, string content)
    {
        // Create the data object to send
        NoteData noteData = new NoteData
        {
            title = title,
            content = content,
            timestamp = DateTime.UtcNow.ToString("o")
        };

        // Convert to JSON
        string jsonData = JsonUtility.ToJson(noteData);

        // Create request
        UnityWebRequest request = new UnityWebRequest(mongoDBAPIUrl, "POST");
        byte[] bodyRaw = System.Text.Encoding.UTF8.GetBytes(jsonData);
        request.uploadHandler = new UploadHandlerRaw(bodyRaw);
        request.downloadHandler = new DownloadHandlerBuffer();
        request.SetRequestHeader("Content-Type", "application/json");

        // Send request
        yield return request.SendWebRequest();

        if (request.result == UnityWebRequest.Result.Success)
        {
            Debug.Log("Note saved successfully!");
            // You might want to show a success message to the user
            // Or clear the fields for a new note
            // titleField.text = "";
            // textAreaField.text = "";
            SaveCurrentStateForUndo();
        }
        else
        {
            Debug.LogError($"Error saving note: {request.error}");
            // You might want to show an error message to the user
        }
    }
}

// Data structure for sending to MongoDB
[Serializable]
public class NoteData
{
    public string title;
    public string content;
    public string timestamp;
}