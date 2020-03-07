using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using System.IO;
using GooglePlayGames;
using UnityEngine.SocialPlatforms;


public class DataController : MonoBehaviour {

    public RoundData[] allRoundData;
    private PlayerProgress playerProgress;
    private AudioManager audio;
    
   
    // Use this for initialization
    private void Awake()
    {
        PlayGamesPlatform.Activate();
       
    }
    void Start () {
       
        DontDestroyOnLoad(gameObject);
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
}
