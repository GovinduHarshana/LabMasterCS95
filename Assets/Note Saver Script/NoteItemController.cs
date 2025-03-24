using System;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class NoteItemController : MonoBehaviour
{
    [SerializeField] private TMP_Text titleText;
    [SerializeField] private TMP_Text previewText;
    [SerializeField] private Button viewButton;

    public Action onViewButtonClicked;
    private string fullContent;
    private string noteId;

    public void Initialize(string id, string title, string content)
    {
        noteId = id;
        titleText.text = title;
        fullContent = content;

        // Create preview (first 50 characters)
        string preview = content.Length > 50 ? content.Substring(0, 50) + "..." : content;
        previewText.text = preview;

        viewButton.onClick.AddListener(OnViewButtonClick);
    }

    public void OnViewButtonClick()
    {
        if (NoteSceneManager.Instance != null)
        {
            NoteSceneManager.Instance.LoadEditNote(noteId, titleText.text, fullContent);
        }
        else
        {
            Debug.LogError("NoteSceneManager instance not found!");
        }
    }

    private void OnDestroy()
    {
        viewButton.onClick.RemoveListener(OnViewButtonClick);
    }

    internal void Initialize(string noteId, string title, string content, Action<string, string, string> loadNoteToEditor)
    {
        throw new NotImplementedException();
    }
}