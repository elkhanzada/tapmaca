﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using System.IO;
using GooglePlayGames;
using UnityEngine.SocialPlatforms;
using System;
using System.Diagnostics;

public class DataController : MonoBehaviour {

    public RoundData[] allRoundData;
    public List<List<AnswerData>> wrongAnswers = new List<List<AnswerData>>(); 
    public GameObject prefData;
    private PlayerProgress playerProgress;
    private AudioManager audio;
    
   
    // Use this for initialization
    private void Awake()
    {
        PlayGamesPlatform.Activate();
       
    }
    void Start () {
       
        DontDestroyOnLoad(gameObject);

        //extractData(allRoundData);
        

        for (int i = 0; i<allRoundData.Length; i++)
        {
            List<AnswerData> one = new List<AnswerData>();
            for (int j = 0; j<allRoundData[i].questions.Length; j++)
            {
                //if (allRoundData[i].questions[j].answers[0].answerText == "" || allRoundData[i].questions[j].answers[0].isCorrect == false)
                  //  print("Something at " + allRoundData[i].questions[j].questionText + "Index "+ i); 
                    if (allRoundData[i].questions[j].answers[0].answerText != "" &&
                        !one.Exists(x => x.answerText == allRoundData[i].questions[j].answers[0].answerText))
                    {
                        AnswerData temp = new AnswerData();
                        temp.answerText = allRoundData[i].questions[j].answers[0].answerText;
                        one.Add(temp);
                    }
                
            }
            if(one.Count>0)
                wrongAnswers.Add(one);
        }

        audio = GameObject.FindObjectOfType<AudioManager>();
        if (PlayerPrefs.HasKey("Music"))
        {
            if (PlayerPrefs.GetInt("Music") == 0)
            {
                audio.GetComponent<AudioSource>().Pause();
               
            }
            else
            {
                audio.GetComponent<AudioSource>().UnPause();
            }
        }
        SceneManager.LoadScene("MenuScene");
        LoadPlayerProgress();
        // StartService();
	}
    
    public RoundData GetCurrentRoundData(int index)
    {
        return allRoundData[index];
    }
    public List<AnswerData> GetCurrentWrongData(int index)
    {
        return wrongAnswers[index];
    }
    public void SubmitNewPlayerScore(int newScore)
    {

        if (newScore > playerProgress.highestScore)
        {
            
            
            playerProgress.highestScore = newScore;
            SavePlayerProgress();
            AddScore();
        }
    }
    public int GetHighestPlayerScore()
    {
        return playerProgress.highestScore;
    }
	private void LoadPlayerProgress()
    {
        playerProgress = new PlayerProgress();
        if (PlayerPrefs.HasKey("highestScore"))
        {
            playerProgress.highestScore = PlayerPrefs.GetInt("highestScore");
            

        }
    }
    private void SavePlayerProgress()
    {
        PlayerPrefs.SetInt("highestScore", playerProgress.highestScore);
    }

   
	// Update is called once per frame
	void Update () {
		
	}
    private void StartService()
    {
        Social.localUser.Authenticate((bool success) =>
        {

        });
    }
    private void AddScore()
    {
        Social.ReportScore(playerProgress.highestScore, ElkhanResources.leaderboard_rekordlar, (bool success) =>
        {

        });
    }
    private void extractData(RoundData[] data)
    {

        for (int i = 0; i < data.Length; i++)
        {
            if (File.Exists(data[i].name + ".json"))
            {
                File.Delete(data[i].name + ".json");
            }
            StreamWriter writer = new StreamWriter(data[i].name + ".json", true);
            string sjson = JsonUtility.ToJson(data[i]);
            writer.WriteLine(sjson);
            writer.Close();
        }
        /*  if (File.Exists(data[i].name + "_debug.json"))
          {
              File.Delete(data[i].name + "_debug.json");
          }
        StreamWriter writer = new StreamWriter(data[i].name + "_debug.json", true);
        QuestionData[] questions = data[i].questions;
            for (int j = 0; j < questions.Length; j++)
        {
        string answer = questions[j].answers[0].answerText;
        if (answer.Contains(" və ") || answer.Contains(","))
        {
          string sjson = JsonUtility.ToJson(questions[j]);
          writer.WriteLine(sjson);
        }
        }
        writer.Close();*/
    }



}
