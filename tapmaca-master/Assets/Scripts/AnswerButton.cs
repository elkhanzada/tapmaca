using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AnswerButton : MonoBehaviour {

    public Text answerText;
    public AudioClip yes;
    public AudioClip no;
    private AnswerData answerData;
    private static int checkIfOneClicked = 0;
    private GameController gameController;
    
	// Use this for initialization
	void Start () {
        gameController = FindObjectOfType<GameController>();
	}
	
	
    public void Setup(AnswerData answer)
    {
        answerData = answer;
        answerText.text = answerData.answerText;
    }
    public void HandleClick()
    {
        checkIfOneClicked++;
        print(checkIfOneClicked);
        if (checkIfOneClicked == 1)
        {
            if (answerData.isCorrect)
            {
                gameObject.GetComponent<Image>().color = Color.green;
                if (PlayerPrefs.HasKey("Music"))
                {
                    if (PlayerPrefs.GetInt("Music") == 1)
                    {
                        AudioSource.PlayClipAtPoint(yes, Vector3.zero);
                    }



                }
                else
                {
                    AudioSource.PlayClipAtPoint(yes, Vector3.zero);
                }
            }

            else
            {
                gameObject.GetComponent<Image>().color = Color.red;
                if (PlayerPrefs.HasKey("Music"))
                {
                    if (PlayerPrefs.GetInt("Music") == 1)
                    {
                        AudioSource.PlayClipAtPoint(no, Vector3.zero);
                    }
                }
                else
                {
                    AudioSource.PlayClipAtPoint(no, Vector3.zero);
                }

            }
            gameController.fadeImage.SetActive(true);
           
            Invoke("ToWhite", 1.3f);
            Invoke("WorkonButton", 1.4f);
        }
    }
    private void ToWhite()
    {
        gameObject.GetComponent<Image>().color = Color.white;
    }
    private void WorkonButton()
    {
        checkIfOneClicked = 0;
        gameController.AnswerButtonClicked(answerData.isCorrect);
        gameController.fadeImage.SetActive(false);
    }
}
