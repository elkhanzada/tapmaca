using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SocialPlatforms;
using GooglePlayGames;

public class MenuSceneController : MonoBehaviour {
   
    public static int questionSelected;
    private AudioManager audio;
    PlayerProgress playerProgress;
    public Sprite forMusicButtonOn;
    public Sprite forMusicButtonOff;
    public  GameObject musicButton;
    private static int isOn = 1;
    
    private void Start()
    {
		
		AdManager.Instance.ShowVideo();
  
        audio = FindObjectOfType<AudioManager>();
        if (PlayerPrefs.HasKey("Music"))
        {
            if (musicButton)
            {
                if (PlayerPrefs.GetInt("Music") == 0)
                {
                    musicButton.GetComponent<Unity.VectorGraphics.SVGImage>().sprite = forMusicButtonOff;
                    AudioManager.playingSound = false;

                }
                else
                {
                    if (AudioManager.playingSound)
                    {
                        audio.GetComponent<AudioSource>().Play();
                        AudioManager.playingSound = false;
                        musicButton.GetComponent<Unity.VectorGraphics.SVGImage>().sprite = forMusicButtonOn;
                    }
                }
            }
        }
        else
        {
            audio.GetComponent<AudioSource>().Play();
        }
      
    }
    public void Leaderboard()
    {
       
        if (Social.localUser.authenticated)
        {
            PlayGamesPlatform.Instance.ShowLeaderboardUI(ElkhanResources.leaderboard_rekordlar);
        }
        else
        {
                Social.localUser.Authenticate((bool success) =>
                {

                });

          
        }
       


    }
    public void StartGame(int index)
    {
		
    	AdManager.Instance.RequestInterstitial();
        
        questionSelected = index;
        SceneManager.LoadScene("Game");

    }
    public void LoadScene()
    {
        SceneManager.LoadScene("QuestionSelection");
    }
    public void Quit()
    {
        Application.Quit();
    }
    public  void TurnOffOnMusic()
    {
        if (PlayerPrefs.HasKey("Music"))
        {
            if (PlayerPrefs.GetInt("Music") == 0)
            {
                if (!AudioManager.playingSound)
                {
                    audio.GetComponent<AudioSource>().Play();
                }
                else {
                    audio.GetComponent<AudioSource>().UnPause();
                }
                musicButton.GetComponent<Unity.VectorGraphics.SVGImage>().sprite = forMusicButtonOn; 
                PlayerPrefs.SetInt("Music", 1);
            }
            else
            {
                musicButton.GetComponent<Unity.VectorGraphics.SVGImage>().sprite = forMusicButtonOff;
                audio.GetComponent<AudioSource>().Pause();
                PlayerPrefs.SetInt("Music", 0);
            }
        }
        else
        {
           
            
                isOn = 0;
                PlayerPrefs.SetInt("Music", isOn);
                musicButton.GetComponent<Unity.VectorGraphics.SVGImage>().sprite = forMusicButtonOff;
                audio.GetComponent<AudioSource>().Pause();
            
          
        }
    }
    public void turnOnDisplay(GameObject g)
    {
        g.SetActive(true);
    }
    public void turnOffDisplay(GameObject g)
    {
        g.SetActive(false);
    }
    public void changeScene()
    {
        SceneManager.LoadScene("MenuScene");
    }
    private void Update(){
    	if(Input.GetKeyDown(KeyCode.Escape)){
    		if(SceneManager.GetActiveScene().name == "QuestionSelection"){
    			SceneManager.LoadScene("MenuScene");
    		}else{
    			Quit();
    		}

    	}

    }

}
