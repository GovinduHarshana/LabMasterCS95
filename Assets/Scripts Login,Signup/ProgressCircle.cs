using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class ProgressUpdater : MonoBehaviour
{
    public Image practicalProgressCircle;
    public Image quizProgressCircle;
    public TextMeshPro practicalProgressText;
    public TextMeshPro quizProgressText;

    void Start()
    {
        UpdateProgressUI();
    }

    void UpdateProgressUI()
    {
        // Get progress from PlayerPrefs instead of server
        float practicalProgress = PlayerPrefs.GetFloat("Progress", 0f) / 100f;
        float quizProgress = PlayerPrefs.GetFloat("QuizProgress", 0f) / 100f;

        practicalProgressCircle.fillAmount = practicalProgress;
        quizProgressCircle.fillAmount = quizProgress;

        practicalProgressText.text = (practicalProgress * 100).ToString("F0") + "%";
        quizProgressText.text = (quizProgress * 100).ToString("F0") + "%";
    }

    public void HideProgressCircles()
    {
        if (practicalProgressCircle != null && quizProgressCircle != null && 
            practicalProgressText != null && quizProgressText != null)
        {
            practicalProgressCircle.gameObject.SetActive(false);
            quizProgressCircle.gameObject.SetActive(false);
            practicalProgressText.gameObject.SetActive(false);
            quizProgressText.gameObject.SetActive(false);
        }
    }

    public void ShowProgressCircles()
    {
        if (practicalProgressCircle != null && quizProgressCircle != null && 
            practicalProgressText != null && quizProgressText != null)
        {
            practicalProgressCircle.gameObject.SetActive(true);
            quizProgressCircle.gameObject.SetActive(true);
            practicalProgressText.gameObject.SetActive(true);
            quizProgressText.gameObject.SetActive(true);
            UpdateProgressUI(); // Refresh local data
        }
    }
}