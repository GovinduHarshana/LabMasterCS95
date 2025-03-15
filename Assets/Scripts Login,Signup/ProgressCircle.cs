using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using UnityEngine.Networking;

public class ProgressUpdater : MonoBehaviour
{
    public Image practicalProgressCircle;
    public Image quizProgressCircle;
    public Text practicalProgressText;
    public Text quizProgressText;

    private string userId = "USER_ID_HERE"; 
    public string serverUrl = "https://lab-master-backend.vercel.app"; // Vercel URL

    void Start()
    {
        StartCoroutine(FetchUserProgress());
    }

    IEnumerator FetchUserProgress()
    {
        using (UnityWebRequest request = UnityWebRequest.Get(serverUrl + userId))
        {
            yield return request.SendWebRequest();

            if (request.result == UnityWebRequest.Result.ConnectionError || request.result == UnityWebRequest.Result.ProtocolError)
            {
                Debug.LogError("Error fetching progress: " + request.error);
            }
            else
            {
                string jsonResponse = request.downloadHandler.text;
                ProgressData progress = JsonUtility.FromJson<ProgressData>(jsonResponse);

                float practicalFill = (float)progress.practicalsCompleted / progress.totalPracticals;
                float quizFill = (float)progress.quizzesCompleted / progress.totalQuizzes;

                practicalProgressCircle.fillAmount = practicalFill;
                quizProgressCircle.fillAmount = quizFill;

                practicalProgressText.text = (practicalFill * 100).ToString("F0") + "%";
                quizProgressText.text = (quizFill * 100).ToString("F0") + "%";
            }
        }
    }

    [System.Serializable]
    public class ProgressData
    {
        public int practicalsCompleted;
        public int totalPracticals;
        public int quizzesCompleted;
        public int totalQuizzes;
    }
}
