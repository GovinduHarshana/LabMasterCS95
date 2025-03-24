using System.Collections;
using UnityEngine;
using UnityEngine.Networking;
using System.Text;

public class QuizManager : MonoBehaviour
{
    private string apiUrl = "http://localhost:3000/save-answers"; // Change to online URL when deployed

    public void SubmitQuizAnswers(string studentId, string quizId, string[] answers)
    {
        StartCoroutine(SendAnswers(studentId, quizId, answers));
    }

    IEnumerator SendAnswers(string studentId, string quizId, string[] answers)
    {
        // Create JSON data
        QuizDetails data = new QuizDetails(studentId, quizId, answers);
        string jsonData = JsonUtility.ToJson(data);
        byte[] bodyRaw = Encoding.UTF8.GetBytes(jsonData);

        // Create a POST request
        UnityWebRequest request = new UnityWebRequest(apiUrl, "POST");
        request.uploadHandler = new UploadHandlerRaw(bodyRaw);
        request.downloadHandler = new DownloadHandlerBuffer();
        request.SetRequestHeader("Content-Type", "application/json");

        yield return request.SendWebRequest();

        // Handle the response
        if (request.result == UnityWebRequest.Result.Success)
        {
            Debug.Log("Quiz answers submitted successfully: " + request.downloadHandler.text);
        }
        else
        {
            Debug.LogError("Error submitting quiz answers: " + request.error);
        }
    }
}

// Data model for sending quiz data
[System.Serializable]
public class QuizDetails
{
    public string studentId;
    public string quizId;
    public string[] answers;

    public QuizDetails(string studentId, string quizId, string[] answers)
    {
        this.studentId = studentId;
        this.quizId = quizId;
        this.answers = answers;
    }
}
