using UnityEngine;
using UnityEngine.SceneManagement;

public class NoteSceneManager : MonoBehaviour
{
    // Singleton pattern to access the manager from anywhere
    public static NoteSceneManager Instance { get; private set; }

    // Data to transfer between scenes
    private string currentNoteId = "";
    private string currentNoteTitle = "";
    private string currentNoteContent = "";

    private void Awake()
    {
        // Implement singleton pattern
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    // Load the note creation scene with a new note
    public void LoadCreateNewNote()
    {
        currentNoteId = "";
        currentNoteTitle = "";
        currentNoteContent = "";
        SceneManager.LoadScene("NoteSaver");
    }

    // Load the note creation scene with an existing note
    public void LoadEditNote(string noteId, string title, string content)
    {
        currentNoteId = noteId;
        currentNoteTitle = title;
        currentNoteContent = content;
        SceneManager.LoadScene("NoteSaver");
    }

    // Load the notes list scene
    public void LoadNotesListScene()
    {
        SceneManager.LoadScene("SaveCreateNotes");
    }

    // Getters to access the current note data
    public string GetCurrentNoteId() { return currentNoteId; }
    public string GetCurrentNoteTitle() { return currentNoteTitle; }
    public string GetCurrentNoteContent() { return currentNoteContent; }
}