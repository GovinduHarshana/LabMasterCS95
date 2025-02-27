using UnityEngine;
using UnityEngine.Networking;
using System.Collections;
using System.Text;

public class QuizManager : MonoBehaviour
{
    string serverUrl = "http://your-server-ip:5000/save-answers";

    public void SubmitAnswers()
    {
        QuizSubmission submission = new QuizSubmission
        {
            studentId = "123456",
            quizId = "quiz1",
            answers = new Answer[]
            {
                new Answer { questionId = "q1", selectedAnswer = "A" },
                new Answer { questionId = "q2", selectedAnswer = "C" },
                new Answer { questionId = "q3", selectedAnswer = "B" },
                new Answer { questionId = "q4", selectedAnswer = "D" },
                new Answer { questionId = "q5", selectedAnswer = "A" }
            }
        };

        StartCoroutine(SendDataToServer(JsonUtility.ToJson(submission)));
    }

    IEnumerator SendDataToServer(string json)
    {
        using (UnityWebRequest request = new UnityWebRequest(serverUrl, "POST"))
        {
            byte[] bodyRaw = Encoding.UTF8.GetBytes(json);
            request.uploadHandler = new UploadHandlerRaw(bodyRaw);
            request.downloadHandler = new DownloadHandlerBuffer();
            request.SetRequestHeader("Content-Type", "application/json");

            yield return request.SendWebRequest();

            if (request.result == UnityWebRequest.Result.Success)
            {
                Debug.Log("Answers saved successfully!");
            }
            else
            {
                Debug.LogError("Error: " + request.error);
            }
        }
    }
}
