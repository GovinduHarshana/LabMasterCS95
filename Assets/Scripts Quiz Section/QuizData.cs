using System;

[Serializable]
public class QuizData
{
    public string question;
    public string[] answers;
    public int correctAnswerIndex;
    public string explanation;
}