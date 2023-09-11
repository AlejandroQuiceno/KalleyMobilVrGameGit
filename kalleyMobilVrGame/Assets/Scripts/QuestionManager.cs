using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class QuestionManager : Singleton<QuestionManager>
{
    [SerializeField] public List<Question> questions = new List<Question>();
    public int currentQuestionIndex = 0;
    private void Start()
    {
        questions = ListRandomizer.RandomizeList(questions);
    }
    public Question GetQuestion()
    {
        return questions[currentQuestionIndex];
    }
    public void NextQuestion()
    {
        currentQuestionIndex++;
    }
}
