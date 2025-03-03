using UnityEngine;
using UnityEngine.UI;
using System;
using System.Text;
using UnityEngine.Networking;
using System.Collections;

public class ChatbotManager : MonoBehaviour
{
    [SerializeField] private InputField userInputField;
    [SerializeField] private Text chatHistoryText;
    [SerializeField] private Button sendButton;

    private readonly string apiKey = "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";
    private readonly string apiUrl = "https://openrouter.ai/api/v1/chat/completions";

    void Start()
    {
        Debug.Log("ChatbotManager started");
        sendButton.onClick.AddListener(SendMessage);
    }

    public void SendMessage()
    {
        if (string.IsNullOrEmpty(userInputField.text)) return;
        
        string userMessage = userInputField.text;
        UpdateChatHistory($"You: {userMessage}");
        StartCoroutine(SendChatRequest(userMessage));
        userInputField.text = "";
    }

    private void UpdateChatHistory(string message)
    {
        chatHistoryText.text += $"\n{message}";
    }

    private IEnumerator SendChatRequest(string userMessage)
    {
        // Create the request body
        string jsonBody = JsonUtility.ToJson(new ChatRequest
        {
            model = "anthropic/claude-3-sonnet",
            messages = new Message[]
            {
                new Message { role = "user", content = userMessage }
            }
        });

        using (UnityWebRequest request = new UnityWebRequest(apiUrl, "POST"))
        {
            byte[] bodyRaw = Encoding.UTF8.GetBytes(jsonBody);
            request.uploadHandler = new UploadHandlerRaw(bodyRaw);
            request.downloadHandler = new DownloadHandlerBuffer();
            request.SetRequestHeader("Content-Type", "application/json");
            request.SetRequestHeader("Authorization", $"Bearer {apiKey}");
            request.SetRequestHeader("HTTP-Referer", "https://github.com/"); // Required by OpenRouter
            request.SetRequestHeader("X-Title", "Unity Chatbot"); // Optional but recommended

            yield return request.SendWebRequest();

            if (request.result == UnityWebRequest.Result.Success)
            {
                try
                {
                    string responseText = request.downloadHandler.text;
                    ChatResponse response = JsonUtility.FromJson<ChatResponse>(responseText);
                    if (response != null && response.choices != null && response.choices.Length > 0)
                    {
                        string botResponse = response.choices[0].message.content;
                        UpdateChatHistory($"Bot: {botResponse}");
                    }
                    else
                    {
                        Debug.LogError("Invalid response format");
                        UpdateChatHistory("Bot: Sorry, I received an invalid response.");
                    }
                }
                catch (Exception e)
                {
                    Debug.LogError($"Error parsing response: {e.Message}");
                    UpdateChatHistory("Bot: Sorry, I encountered an error processing the response.");
                }
            }
            else
            {
                Debug.LogError($"Request error: {request.error}");
                UpdateChatHistory("Bot: Sorry, I encountered a network error.");
            }
        }
    }
}

[Serializable]
public class ChatRequest
{
    public string model;
    public Message[] messages;
}

[Serializable]
public class ChatResponse
{
    public Choice[] choices;
}

[Serializable]
public class Choice
{
    public Message message;
}

[Serializable]
public class Message
{
    public string role;
    public string content;
}