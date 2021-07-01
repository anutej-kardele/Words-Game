using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class WordData : MonoBehaviour
{
    [SerializeField]
    private Text charText;

    [HideInInspector]
    public char charValue;

    private Button buttonobj;


    private void Awake()
    {
        buttonobj = GetComponent<Button>();

        if (buttonobj)
        {
            buttonobj.onClick.AddListener(() => CharSelected());
        }
    }

    public void SetChar(char value)
    {
        charText.text = value + "";
        charValue = value;
    }

    private void CharSelected()
    {
        QuizManager.instance.SelectedQption(this);
    }
 
}
