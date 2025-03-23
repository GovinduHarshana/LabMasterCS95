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
    public Button BackButton;

    private int currentQuestionIndex = 0;
    private int correctAnswers = 0;

    public TextMeshProUGUI TimerText; // timer text
    public GameObject PopupBox;
    public Button OkButton;

    private bool timerRunning = false;
    private float timer = 180f; // 3-minute timer

    private Vector3 popupStartPosition;

    public GameObject TimeoutPopup;
    public Button ContinueButton; 
    public Button CancelButton;   

    void Start()
    {
        LoadQuestion(currentQuestionIndex);
        UpdateProgressBar();

        checkAnswerButton.onClick.AddListener(CheckAnswer);
        nextQuestionButton.onClick.AddListener(NextQuestion);
        BackButton.onClick.AddListener(PreviousQuestion);

        OkButton.onClick.AddListener(StartQuizTimer); // Add listener to OK button

        // Disable quiz interaction elements
        checkAnswerButton.interactable = false;
        nextQuestionButton.interactable = false;
        foreach (Toggle toggle in answerToggleGroup.GetComponentsInChildren<Toggle>())
        {
            toggle.interactable = false;
        }

        // Set initial position and start animation
        popupStartPosition = PopupBox.GetComponent<RectTransform>().anchoredPosition;
        PopupBox.GetComponent<RectTransform>().anchoredPosition = popupStartPosition + new Vector3(0, 500, 0); // Start off-screen

        StartCoroutine(AnimatePopup());

        ContinueButton.onClick.AddListener(OnContinueButtonClicked); 
        CancelButton.onClick.AddListener(OnCancelButtonClicked);

    }

    IEnumerator AnimatePopup()
    {
        float animationDuration = 1f; 
        float elapsedTime = 0f;
        RectTransform popupRect = PopupBox.GetComponent<RectTransform>();

        while (elapsedTime < animationDuration)
        {
            popupRect.anchoredPosition = Vector3.Lerp(popupRect.anchoredPosition, popupStartPosition, elapsedTime / animationDuration);
            elapsedTime += Time.deltaTime;
            yield return null;
        }

        popupRect.anchoredPosition = popupStartPosition; // Ensure final position is exact
    }

    void StartQuizTimer()
    {
        PopupBox.SetActive(false); // Hide the popup
        timerRunning = true;
        StartCoroutine(QuizTimer());

        // Enable quiz interaction elements
        checkAnswerButton.interactable = true;
        nextQuestionButton.interactable = true;
        foreach (Toggle toggle in answerToggleGroup.GetComponentsInChildren<Toggle>())
        {
            toggle.interactable = true;
        }
    }

    IEnumerator QuizTimer()
    {
        while (timer > 0)
        {
            int minutes = Mathf.FloorToInt(timer / 60);
            int seconds = Mathf.FloorToInt(timer % 60);
            TimerText.text = string.Format("{0:00}:{1:00}", minutes, seconds);
            yield return new WaitForSeconds(1f);
            timer--;
        }

        TimerText.text = "Time: 00:00"; // Ensure timer shows 0 when finished
        if (timerRunning)
        {
            TimeoutPopup.SetActive(true);
            timerRunning = false;
            //Optional Code to stop interaction with the quiz.
            checkAnswerButton.interactable = false;
            nextQuestionButton.interactable = false;
            BackButton.interactable = false;
            foreach (Toggle toggle in answerToggleGroup.GetComponentsInChildren<Toggle>())
            {
                toggle.interactable = false;
            }
        }
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

        // Disable Back button on the first question
        BackButton.interactable = (index > 0);
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

    void PreviousQuestion()
    {
        currentQuestionIndex--;
        LoadQuestion(currentQuestionIndex);
        UpdateProgressBar();
    }

    void UpdateProgressBar()
    {
        progressBar.value = (float)(currentQuestionIndex + 1) / quizSO.quizData.Length;
    }

    public void OnContinueButtonClicked()
    {
        TimeoutPopup.SetActive(false);
        RestartQuiz(); // Restart the quiz
    }

    public void OnCancelButtonClicked()
    {
        TimeoutPopup.SetActive(false);
        //SceneManager.LoadScene("QuizListPanel"); 
    }

    public void RestartQuiz()
    {
        currentQuestionIndex = 0;
        correctAnswers = 0;
        timer = 180f; // Reset the timer
        timerRunning = true;
        LoadQuestion(currentQuestionIndex);
        UpdateProgressBar();
        StartCoroutine(QuizTimer());

        // Enable quiz interaction elements
        checkAnswerButton.interactable = true;
        nextQuestionButton.interactable = true;
        foreach (Toggle toggle in answerToggleGroup.GetComponentsInChildren<Toggle>())
        {
            toggle.interactable = true;
        }
    }


    public void CompleteQuiz()
    {
        // Save quiz progress (increase by 10%)
        float currentQuizProgress = PlayerPrefs.GetFloat("QuizProgress", 0f);
        currentQuizProgress += 50f; // Increase progress by 50%
        PlayerPrefs.SetFloat("QuizProgress", currentQuizProgress);
        PlayerPrefs.Save();

        SceneManager.LoadScene("EndOfTheQuiz");
    }
}