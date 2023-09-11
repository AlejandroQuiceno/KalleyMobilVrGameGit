using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

[Serializable]
public class Question 
{
    [SerializeField] public String question;
    [SerializeField] public List<string> answerList;
    [SerializeField] public List<BoxColor> correctColors;
}
