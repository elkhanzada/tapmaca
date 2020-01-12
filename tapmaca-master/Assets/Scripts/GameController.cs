using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using UnityEngine;

public class GameController : MonoBehaviour {
    public Text questionDisplayText;
    public Text scoreDisplayText;
    public SimpleObjectPool answerButtonObjectPool;
    public Transform answerButoonParent;
    public GameObject questionDisplay;
    public GameObject roundOverDisplay;
    public GameObject pauseDisplay;
    public GameObject fadeImage;
    public Text timeRemainingDisplayText;
    public Text highScoreDisplay;
    public GameObject END;
    public Button musicButton;
    public Sprite forMusicButtonOn;
    public Sprite forMusicButtonOff;


    private DataController dataController;
    private QuestionData[] questionPool;
    private RoundData currentRoundData;
    private bool isRoundActive;
    private float timeRemaining =40;
    private int questionIndex;
    private int playerScore;
    private GameObject yesno;
    private List<GameObject> answerButtonGameObjects = new List<GameObject>();
    private List<int> answeredQuestions;
    private int forStories;
    private AudioManager audio;
    

	void Start () {
        if(!audio)
        audio = GameObject.FindObjectOfType<AudioManager>();
        dataController = FindObjectOfType<DataController>();
        currentRoundData = dataController.GetCurrentRoundData(MenuSceneController.questionSelected);
        if (MenuSceneController.questionSelected == 0)
        {
            forStories = currentRoundData.questions.Length;
        }
        else
        {
            forStories = 50;
        }
        if (PlayerPrefs.GetInt("Music") == 0)
        {
            musicButton.image.sprite = forMusicButtonOff;
        }
        else
        {
            musicButton.image.sprite = forMusicButtonOn;
        }

        questionPool = currentRoundData.questions;
        timeRemaining = 40f;
        UpdateTimeRemainingDisplay();
        questionIndex = Random.Range(0,forStories);
       // yesno = GameObject.Find("TF");
        answeredQuestions = new List<int>();
        answeredQuestions.Add(questionIndex);
        playerScore = 0;
        isRoundActive = true;
        ShowQuestion();
        
	}
    private void ShowQuestion()
    {


        timeRemaining = 40f;
        RemoveAnswerButtons();
        QuestionData questionData = questionPool[questionIndex];
       
       
        questionDisplayText.text = questionData.questionText.Replace("NEWLINE", "\n"); 
        
        
        for(int i = 0; i < questionData.answers.Length; i++)
        {
            GameObject answerButtonGameObject = answerButtonObjectPool.GetObject();
            answerButtonGameObject.transform.SetParent(answerButoonParent);
            answerButtonGameObject.transform.localScale = new Vector3(1f, 1f, 1f);
            answerButtonGameObjects.Add(answerButtonGameObject);
            AnswerButton answerButton = answerButtonGameObject.GetComponent<AnswerButton>();
            answerButton.Setup(questionData.answers[i]);
        }

    }
    private void RemoveAnswerButtons()
    {
        int removeButton = Random.Range(0, answerButtonGameObjects.Count - 1);
        while (answerButtonGameObjects.Count > 0)
        {
            answerButtonObjectPool.ReturnObject(answerButtonGameObjects[removeButton]);
            answerButtonGameObjects.RemoveAt(removeButton);
            removeButton = Random.Range(0, answerButtonGameObjects.Count - 1);
        }
    }
    public void AnswerButtonClicked(bool isCorrect)
    {
       
        if (isCorrect)
        {
            playerScore += currentRoundData.pointsAddedForCorrectAnswer;
            scoreDisplayText.text = "Xal: " + playerScore.ToString();       
          
          
            
            forAnswerButtonClicked();
        }
        else
        {
            EndRound();
        }
      
      
        
        
    } 
    private void forAnswerButtonClicked()
    {
        questionIndex = Random.Range(0, forStories);
        if (answeredQuestions.Count >= forStories)
        {
            EndRound();
        }
        else
        {
            while (true)
            {
                if (!answeredQuestions.Contains(questionIndex))
                {
                    answeredQuestions.Add(questionIndex);
                    break;
                }
                else
                {
                    questionIndex = Random.Range(0, forStories);

                }
            }
           
            ShowQuestion();
        }
    }
    public void EndRound()
    {
        isRoundActive = false;
        questionDisplay.SetActive(false);
        roundOverDisplay.SetActive(true);
        dataController.SubmitNewPlayerScore(playerScore);
       highScoreDisplay.text = dataController.GetHighestPlayerScore().ToString();
        if (answeredQuestions.Count >= forStories) 
        {
            END.SetActive(true);
            print("yes");
        }
        else
        {
            END.SetActive(false);
        }
      
        AdManager.Instance.ShowVideo();
    	
    }
    public void Restart()
    {
        Start();
        scoreDisplayText.text = "Xal: " + playerScore.ToString();
        roundOverDisplay.SetActive(false);
        questionDisplay.SetActive(true);

		AdManager.Instance.RequestInterstitial();
	
    }
    public void Pause()
    {
        Time.timeScale = 0;
        questionDisplay.SetActive(false);
        pauseDisplay.SetActive(true);
		
        AdManager.Instance.ShowVideo();

    }
    public void Continue()
    {
		
        Time.timeScale = 1;
        questionDisplay.SetActive(true);
        pauseDisplay.SetActive(false);

		AdManager.Instance.RequestInterstitial();
    }
    public void ReturnToMenu()
    {
        Time.timeScale = 1;
        SceneManager.LoadScene("MenuScene");

		AdManager.Instance.RequestInterstitial();
    }
    private void UpdateTimeRemainingDisplay()
    {
        timeRemainingDisplayText.text = "Vaxt: " + Mathf.Floor(timeRemaining);
    }
    public void TurnOffOnMusic()
    {
        if (PlayerPrefs.HasKey("Music"))
        {
            if (PlayerPrefs.GetInt("Music") == 0)
            {
                if (!AudioManager.playingSound)
                {
                    audio.GetComponent<AudioSource>().Play();
                    AudioManager.playingSound = false;
                }
                else
                {
                    audio.GetComponent<AudioSource>().UnPause();
                }
                PlayerPrefs.SetInt("Music", 1);
                musicButton.image.sprite = forMusicButtonOn;
            }
            else
            {
                audio.GetComponent<AudioSource>().Pause();
                PlayerPrefs.SetInt("Music", 0);
                musicButton.image.sprite = forMusicButtonOff;
            }
        }
        else
        {


            musicButton.image.sprite = forMusicButtonOff;
            PlayerPrefs.SetInt("Music", 0);
            audio.GetComponent<AudioSource>().Pause();


        }
    }


    // Update is called once per frame
    void Update () {

        if (isRoundActive)
        {
            if (timeRemaining <= 0f)
            {
                EndRound();
            }
            timeRemaining -= Time.deltaTime;
            UpdateTimeRemainingDisplay();
           
        }
        if (!fadeImage.activeInHierarchy)
        {
            if (Input.GetKeyDown(KeyCode.Escape))
            {
                if (Time.timeScale == 0)
                {
                    Time.timeScale = 1;
                }
                SceneManager.LoadScene("QuestionSelection");
            }
        }
       /* if (Input.GetButtonDown("Fire2"))
        {
            if (Time.timeScale == 0)
            {
                Time.timeScale = 1;
            }
            SceneManager.LoadScene("QuestionSelection");
        }*/
	}
}
