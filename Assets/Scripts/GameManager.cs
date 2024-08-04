using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using System;

[Serializable]
public class QUESTION
{
    public Sprite qsprite;
    public string answer = "JAPAN";
}

public class GameManager : MonoBehaviour
{
    public int TimerTime = 0;
    int currentTimerTime = 0;

    [SerializeField] TMP_Text txtTImer;
    [SerializeField] Button btnGo;
    [SerializeField] Image qImage;
    [SerializeField] TMP_Text txtRighrWrong;
    [SerializeField] TMP_Text txtScore;

    public ObjectPreviewerControls objControl;

    public List<QUESTION> questions;

    int currentQuestion = 0;

    public Button btnJapan;
    public Button btnMalaysia;

    int currentScore = 0;
    [SerializeField] AudioSource AudioPlayer;
    [SerializeField] AudioClip sfxDing;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    public void StartGame()
    {
        objControl.objectPreviewerInputAction.Enable();

        btnGo.gameObject.SetActive(false);
        currentTimerTime = TimerTime;

        askQuestion();
    }

    void askQuestion()
    {
        InvokeRepeating("TimerCountDown", 0, 1f);
        ShowQuestion();

        btnJapan.gameObject.SetActive(true);
        btnMalaysia.gameObject.SetActive(true);
    }

    void TimerCountDown()
    {
        txtTImer.text = currentTimerTime.ToString();
        currentTimerTime -= 1;
        if(currentTimerTime <= -1)
        {
            TimesUp();
        }
    }

    void TimesUp()
    {
        CancelInvoke("TimerCountDown");
        Debug.Log("TImesUp");
        txtRighrWrong.gameObject.SetActive(true);
        txtRighrWrong.text = "Times UP!";

        btnJapan.gameObject.SetActive(false);
        btnMalaysia.gameObject.SetActive(false);

        StartCoroutine(nextQuestion());
    }

    IEnumerator nextQuestion()
    {

        yield return new WaitForSeconds(2);

        txtRighrWrong.gameObject.SetActive(false);

        currentQuestion++;
        currentTimerTime = TimerTime;

        askQuestion();
    }

    void ShowQuestion()
    {
        qImage.sprite = questions[currentQuestion%2].qsprite;
    }

    public void AnswerQuestion(string _answer)
    {
        Debug.Log(currentQuestion%2);
        txtRighrWrong.gameObject.SetActive(true);
        if (currentQuestion%2 == 0)
        {
            if(_answer == "JAPAN")
            {
                txtRighrWrong.text = "RIGHT!";
                currentScore += 100;
                AudioPlayer.PlayOneShot(sfxDing);
            }
            else
                txtRighrWrong.text = "WRONG!";
        }
        


        if (currentQuestion % 2 == 1)
        {
            if (_answer == "MALAYSIA")
            {
                txtRighrWrong.text = "RIGHT!";
                currentScore += 100;
                AudioPlayer.PlayOneShot(sfxDing);
            }
            else
                txtRighrWrong.text = "WRONG!";
        }

        txtScore.text = currentScore.ToString();


        CancelInvoke("TimerCountDown");
        btnJapan.gameObject.SetActive(false);
        btnMalaysia.gameObject.SetActive(false);

        StartCoroutine(nextQuestion());
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
