using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement;
using System.Collections;

public class QuizManagerNew : MonoBehaviour
{
    public QuizSO quizSO;
    public TextMeshProUGUI questionText;
    public TextMeshProUGUI[] answerTexts;
    public Toggle[] answerToggles;
    public ToggleGroup answerToggleGroup;
    public TextMeshProUGUI resultText;
    public TextMeshProUGUI explanationText;
    public Button checkAnswerButton;
    public Button nextQuestionButton;
    public Slider progressBar;
    public TextMeshProUGUI questionNumberText;

    private int currentQuestionIndex = 0;
    private int correctAnswers = 0;

    public TextMeshProUGUI timerText; // timer text
    private float timer = 30f; // 30-second timer

    void Start()
    {
        LoadQuestion(currentQuestionIndex);
        UpdateProgressBar();

        checkAnswerButton.onClick.AddListener(CheckAnswer);
        nextQuestionButton.onClick.AddListener(NextQuestion);

        StartCoroutine(QuizTimer()); // Start the timer
    }

    IEnumerator QuizTimer()
    {
        while (timer > 0)
        {
            timerText.text = "Timer: " + Mathf.FloorToInt(timer).ToString() + "s";
            yield return new WaitForSeconds(1f);
            timer--;
        }

        timerText.text = "Time: 0"; // Ensure timer shows 0 when finished
        CompleteQuiz(); // Quiz finished when timer reaches 0
    }

    void LoadQuestion(int index)
    {
        QuizData currentQuestion = quizSO.quizData[index];

        questionText.text = currentQuestion.question;

        for (int i = 0; i < answerTexts.Length; i++)
        {
            answerTexts[i].text = currentQuestion.answers[i];
        }

        answerToggleGroup.SetAllTogglesOff();

        resultText.text = "";
        explanationText.text = "";

        questionNumberText.text = $"Question {index + 1} of {quizSO.quizData.Length}";
    }

    void CheckAnswer()
    {
        Toggle selectedToggle = answerToggleGroup.GetFirstActiveToggle();

        if (selectedToggle != null)
        {
            int selectedAnswerIndex = System.Array.IndexOf(answerToggles, selectedToggle);

            if (selectedAnswerIndex == quizSO.quizData[currentQuestionIndex].correctAnswerIndex)
            {
                resultText.text = "Correct!";
                correctAnswers++;
            }
            else
            {
                resultText.text = "Wrong!";
            }

            explanationText.text = quizSO.quizData[currentQuestionIndex].explanation;
        }
        else
        {
            Debug.Log("No answer selected!");
        }
    }

    void NextQuestion()
    {
        currentQuestionIndex++;

        if (currentQuestionIndex < quizSO.quizData.Length)
        {
            LoadQuestion(currentQuestionIndex);
            UpdateProgressBar();
        }
        else
        {
            // Last question reached
            questionText.text = $"Quiz Complete! You got {correctAnswers} out of {quizSO.quizData.Length} correct.";
            nextQuestionButton.GetComponentInChildren<TextMeshProUGUI>().text = "Finish"; // Change button text to "Finish"
            nextQuestionButton.onClick.RemoveAllListeners(); // Remove previous listener
            nextQuestionButton.onClick.AddListener(CompleteQuiz); // Add CompleteQuiz listener
        }
    }

    void UpdateProgressBar()
    {
        progressBar.value = (float)(currentQuestionIndex + 1) / quizSO.quizData.Length;
    }

    public void CompleteQuiz()
    {
        SceneManager.LoadScene("EndOfTheQuiz");
    }
}