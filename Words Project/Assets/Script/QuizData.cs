using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName ="QuestionData",menuName ="QuestionData",order =1)]
public class QuizData : ScriptableObject
{
    public List<QuestionData> questions;    
    
}
