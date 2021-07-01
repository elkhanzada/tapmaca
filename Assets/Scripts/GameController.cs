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
    public GameObject bgDisplay;
    public Text timeRemainingDisplayText;
    public Text highScoreDisplay;
    public GameObject END;
    public Button musicButton;
    public Sprite forMusicButtonOn;
    public Sprite forMusicButtonOff;
    public GameObject settingsDisplay;
    public GameObject answerButton;
    
    private DataController dataController;
    private QuestionData[] questionPool;
    private RoundData currentRoundData;
    private List<AnswerData> currentWrongData;
    private bool isRoundActive;
    private float timeRemaining =40;
    private int questionIndex;
    private int playerScore;
    private List<GameObject> answerButtonGameObjects = new List<GameObject>();
    private List<QuestionData> questionsList;
    private int forStories;
    private AudioManager audio;
    private string[] abcd = { "A", "B", "C", "D" };
  
    
    void Start() {
        Time.timeScale = 1;
        if (!audio)
            audio = GameObject.FindObjectOfType<AudioManager>();
        dataController = FindObjectOfType<DataController>();
        currentRoundData = dataController.GetCurrentRoundData(MenuSceneController.questionSelected);
        currentWrongData = dataController.GetCurrentWrongData(MenuSceneController.questionSelected);
        questionPool = currentRoundData.questions;
      /*  switch (MenuSceneController.questionSelected)
        {
            case 0:
                bgDisplay.GetComponent<Image>().color = Color.green;
                answerButton.transform.GetChild(0).GetComponent<Image>().color = Color.green;
                break;
            case 1:
                bgDisplay.GetComponent<Image>().color = Color.yellow;
                answerButton.transform.GetChild(0).GetComponent<Image>().color = Color.yellow;
                break;
            case 2:
                bgDisplay.GetComponent<Image>().color = Color.cyan;
                answerButton.transform.GetChild(0).GetComponent<Image>().color = Color.cyan;
                break;
            case 3:
                bgDisplay.GetComponent<Image>().color = Color.red;
                answerButton.transform.GetChild(0).GetComponent<Image>().color = Color.cyan;
                break;
            case 4:
                bgDisplay.GetComponent<Image>().color = Color.black;
                answerButton.transform.GetChild(0).GetComponent<Image>().color = Color.black;
                break;
        }*/
        forStories = currentRoundData.questions.Length;
        if (PlayerPrefs.GetInt("Music") == 0)
        {
            musicButton.image.sprite = forMusicButtonOff;
        }
        else
        {
            musicButton.image.sprite = forMusicButtonOn;
        }


        timeRemaining = 40f;
        UpdateTimeRemainingDisplay();
        questionIndex = Random.Range(0, forStories);
        // yesno = GameObject.Find("TF");
        questionsList = new List<QuestionData>(questionPool);
        questionsList = questionsList.GetRange(0, forStories);
        //answeredQuestions.Add(questionIndex);
        playerScore = 0;
        isRoundActive = true;
        ShowQuestion();
        
	}
    private void ShowQuestion()
    {
        print(questionsList.Count);

        timeRemaining = 40f;
        RemoveAnswerButtons();
        QuestionData questionData = questionsList[questionIndex];
       
       
        questionDisplayText.text = questionData.questionText.Replace("NEWLINE", "\n");

        List<AnswerData> wrongAnswersForSpecificQuestion = new List<AnswerData>();
         for (int i = 0; i < 4; i++)
        {
            GameObject answerButtonGameObject = answerButtonObjectPool.GetObject();
            GameObject child = answerButtonGameObject.transform.GetChild(0).transform.GetChild(0).gameObject;
            if(child.GetComponent<Text>().text.Equals("-"))
                child.GetComponent<Text>().text = abcd[i];
            answerButtonGameObject.transform.SetParent(answerButoonParent);
            answerButtonGameObject.transform.localScale = new Vector3(1f, 1f, 1f);
            answerButtonGameObjects.Add(answerButtonGameObject);
            AnswerButton answerButton = answerButtonGameObject.GetComponent<AnswerButton>();

            if (i==0)
            {
                answerButton.Setup(questionData.answers[0]);
                wrongAnswersForSpecificQuestion.Add(questionData.answers[0]);
            }
            else
            {
                int incorrectIndex = Random.Range(0, currentWrongData.Count);
                while (wrongAnswersForSpecificQuestion.Exists(x => x.answerText[0] == currentWrongData[incorrectIndex].answerText[0]))
                    incorrectIndex = Random.Range(0, currentWrongData.Count);
                answerButton.Setup(currentWrongData[incorrectIndex]);
                wrongAnswersForSpecificQuestion.Add(currentWrongData[incorrectIndex]);
            }
        }
    }
    private void RemoveAnswerButtons()
    {
        int cubic = answerButtonGameObjects.Count;
        int removeButton = Random.Range(0, cubic*cubic*cubic);
        while (answerButtonGameObjects.Count > 0)
        {
            answerButtonObjectPool.ReturnObject(answerButtonGameObjects[removeButton%cubic]);
            answerButtonGameObjects.RemoveAt(removeButton%cubic);
            cubic--;
            removeButton = Random.Range(0, cubic*cubic*cubic);
        }
    }
    public void AnswerButtonClicked(bool isCorrect)
    {
       
        if (isCorrect)
        {
            playerScore += currentRoundData.pointsAddedForCorrectAnswer;
            scoreDisplayText.text = playerScore.ToString();


            questionsList.RemoveAt(questionIndex);
            forAnswerButtonClicked();
        }
        else
        {
            EndRound();
        }
      
      
        
        
    } 
    private void forAnswerButtonClicked()
    {
        questionIndex = Random.Range(0, questionsList.Count);
        if (questionsList.Count==0)
            EndRound();
        else
            ShowQuestion();
        /*  else
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
          */



    }
    public void EndRound()
    {
        isRoundActive = false;
        questionDisplay.SetActive(false);
        roundOverDisplay.SetActive(true);
        dataController.SubmitNewPlayerScore(playerScore);
        highScoreDisplay.text = playerScore.ToString();//dataController.GetHighestPlayerScore().ToString();
        if (questionsList.Count==0) 
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
        scoreDisplayText.text = playerScore.ToString();
        roundOverDisplay.SetActive(false);
        pauseDisplay.SetActive(false);
        questionDisplay.SetActive(true);

		AdManager.Instance.RequestInterstitial();
	
    }
    public void Pause()
    {
        Time.timeScale = 0;
        pauseDisplay.SetActive(true);
		
        AdManager.Instance.ShowVideo();

    }

    public void goToSettings(bool go)
    {
        if (go)
        {
            Time.timeScale = 0;
            settingsDisplay.SetActive(true);
            AdManager.Instance.ShowVideo();
        }else
        {
            Time.timeScale = 1;
            settingsDisplay.SetActive(false);
            AdManager.Instance.RequestInterstitial();
        }
    }
    public void Continue()
    {
		
        Time.timeScale = 1;
        questionDisplay.SetActive(true);
        pauseDisplay.SetActive(false);

		AdManager.Instance.RequestInterstitial();
    }
    public void ReturnToMenu(bool isMenu)
    {
        Time.timeScale = 1;
        if (isMenu)
        {
            SceneManager.LoadScene("MenuScene");
        }
        else
        {
            SceneManager.LoadScene("QuestionSelection");
        }
		AdManager.Instance.RequestInterstitial();
    }
    private void UpdateTimeRemainingDisplay()
    {
        timeRemainingDisplayText.text = ""+ Mathf.Floor(timeRemaining);
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
    public void LeaderBoard()
    {
        MenuSceneController mn = new MenuSceneController();
        mn.Leaderboard();
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
