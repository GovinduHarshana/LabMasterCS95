[System.Serializable]
public class Answer
{
    public string questionId;
    public string selectedAnswer;
}

[System.Serializable]
public class QuizSubmission
{
    public string studentId;
    public string quizId;
    public Answer[] answers;
}
