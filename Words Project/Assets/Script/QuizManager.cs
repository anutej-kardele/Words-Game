using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System.IO;

public class QuizManager : MonoBehaviour
{
    public static QuizManager instance;

    [SerializeField]
    private QuizData questionData;
    [SerializeField]
    private Image questionImage;
    [SerializeField]
    public Image writeImage;
    [SerializeField]
    public Image wrongImage;
    [SerializeField]
    public Image[] stars;
    [SerializeField]
    private Text result;    
    [SerializeField]
    private WordData[] answerWordCharector;
    [SerializeField]
    private WordData[] optionWordCharector;
    private char[] charArray;
    private int currentanswerindex = 0;
    private bool correctAnswer = true;
    private int currentquestionindex = 0;
    private string answerWord;
    private gamestatus gamestatus = gamestatus.Playing;
    private int sco = 0;
    private List<int> selectedWordIndex;
    private int hintcount = 0;
    public string ans;
    //public Animator anim;

    private void Awake()
    {
        if (instance == null)
           instance = this;
        else
            Destroy(gameObject);

        selectedWordIndex = new List<int>();
    }
    void Start()
    {
        for (int i = 0; i < questionData.questions.Count; i++)
        {
            QuestionData fruitCurrentIndex = questionData.questions[i];
            int randomIndex = Random.Range(i, questionData.questions.Count);
            questionData.questions[i] = questionData.questions[randomIndex];
            questionData.questions[randomIndex] = fruitCurrentIndex;
        }
        SetQuestion();
    }

    private void SetQuestion()
    {
       
        currentanswerindex = 0;
        selectedWordIndex.Clear();
        writeImage.gameObject.SetActive(false);
        wrongImage.gameObject.SetActive(false);
        questionImage.sprite = questionData.questions[currentquestionindex]. questionImage;
        answerWord = questionData.questions[currentquestionindex].answer;
        result.text = "GET THE CORRECT SPELLING";
        numberofbox(); 
        charArray = new char[answerWord.Length];
        for (int i=0;i< answerWord.Length;i++)
        {
            charArray[i] = answerWord[i];
        }

        charArray = ShuffleList.ShuffleListItems<char>(charArray.ToList()).ToArray();
        
        int j = 0;
        for (; j < charArray.Length; j++)
            optionWordCharector[j].SetChar(charArray[j]);

        for (; j < 6; j++)
            optionWordCharector[j].gameObject.SetActive(false);

        currentquestionindex++;
        gamestatus = gamestatus.Playing;
    } 

    public void SelectedQption(WordData wordData)
    {
        //Debug.Log(json);
        if (gamestatus == gamestatus.Next ||currentanswerindex >= answerWordCharector.Length)
            return;
        else
            answerWordCharector[currentanswerindex].SetChar(wordData.charValue);
            wordData.gameObject.SetActive(false);
            currentanswerindex++;

        selectedWordIndex.Add(wordData.transform.GetSiblingIndex());
        if(currentanswerindex >= answerWord.Length)
        {
            correctAnswer = true;
            //Debug.Log(answerWord);

            for(int i = 0; i < answerWord.Length; i++)
            {
                //Debug.Log(answerWordCharector[i].charValue);
                if (answerWord[i]!=answerWordCharector[i].charValue)
                {
                    correctAnswer = false;
                    ans = ans + answerWordCharector[i].charValue;
                }
                else
                {
                    ans = ans + answerWordCharector[i].charValue;
                }
            }
            ans = ans + " ";
            Debug.Log(ans);
            

            if (correctAnswer)
            {
               //Debug.Log("Write");
                writeImage.gameObject.SetActive(true);
                result.text = "CORRECT ANSWER YOU GET STAR";
                sco += 1;
                starON();
                gamestatus = gamestatus.Next;
                if (currentquestionindex < 5)
                {
                    //Debug.Log("score : " + score);
                    Invoke("SetQuestion", 3.0f);
                }
                else
                {
                    //Debug.Log("END");
                    Invoke("ChangeScene", 3.0f);
                }
            }
            else if(!correctAnswer)
            {
                //Debug.Log("Wrong");
                wrongImage.gameObject.SetActive(true);
                result.text = "WRONG ANSWER CORRECT ANSWER IS "+ answerWord;
                gamestatus = gamestatus.Next;
                if (currentquestionindex < 5)
                {
                   // Debug.Log("score : " + score);
                    Invoke("SetQuestion", 3.0f);
                }
                else
                {
                    //Debug.Log("END")
                    Invoke("ChangeScene", 3.0f);
                }
            }
        }

        void starON()
        {
            for(int k = 0; k < sco; k++)
            {
                stars[k].gameObject.SetActive(true);
            }
        }

        
    }

    private void numberofbox()
    {
        int i = 0;
        for (; i < answerWord.Length; i++)
        {
            answerWordCharector[i].gameObject.SetActive(true);
            answerWordCharector[i].SetChar('_');
        }
        for (; i < answerWordCharector.Length; i++)
        {
            answerWordCharector[i].gameObject.SetActive(false);
        }
        for(i =0; i < optionWordCharector.Length; i++)
        {
            optionWordCharector[i].gameObject.SetActive(true);
        }
    }

    void ChangeScene()
    {
        storejson store = new storejson();
        store.number_of_hints = hintcount;
        store.answers = ans;
        string json = JsonUtility.ToJson(store);
        File.WriteAllText(Application.dataPath + "/saveddata.json", json);
        SceneManager.LoadScene(0);
    }

    public void ResetLastword()
    {
        if(selectedWordIndex.Count > 0)
        {
            int index = selectedWordIndex[selectedWordIndex.Count - 1];
            optionWordCharector[index].gameObject.SetActive(true);
            answerWordCharector[index].gameObject.SetActive(true);
            selectedWordIndex.RemoveAt(selectedWordIndex.Count - 1);
            currentanswerindex--;
            answerWordCharector[currentanswerindex].SetChar('_');
        }
    }

    public void HINT()
    {
        hintcount++;
        int number = Random.Range(1, 4);
        if (number == 1)
            result.text = "The first letter is : " + answerWord[0];
        else if (number == 2)
            result.text = "The second letter is : " + answerWord[1];
        else if (number == 3)
            result.text = "The third letter is : " + answerWord[2];
        else if (number == 4)
            result.text = "The second last letter is : " + answerWord[answerWord.Length - 2];
        else if (number == 5)
            result.text = "The last letter is : " + answerWord[answerWord.Length - 1];
        Debug.Log(number);
    }

    private class storejson
    {
        public string answers;
        public int number_of_hints;
    }

}
[System.Serializable]
public class QuestionData
{
    public Sprite questionImage;
    public string answer;

}


public enum gamestatus
{
    Playing,
    Next,
}
