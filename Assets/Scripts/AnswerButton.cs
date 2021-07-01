using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AnswerButton : MonoBehaviour {

    public Text answerText;
    public AudioClip yes;
    public AudioClip no;
    public GameObject img;
    public Sprite greenSprite;
    public Sprite redSprite;
    private AnswerData answerData;
    private static int checkIfOneClicked = 0;
    private GameController gameController;
    private static AnswerButton correct;
	// Use this for initialization
	void Start () {
        gameController = FindObjectOfType<GameController>();
	}
	
	
    public void Setup(AnswerData answer)
    {
        answerData = answer;
        if (answerData.isCorrect) correct = this;
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
                gameObject.GetComponent<Unity.VectorGraphics.SVGImage>().sprite = greenSprite;
                gameObject.GetComponent<Unity.VectorGraphics.SVGImage>().color = new Color(1,1,1,1);
                gameObject.transform.GetChild(1).GetComponent<Text>().color = Color.black;
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
                gameObject.GetComponent<Unity.VectorGraphics.SVGImage>().sprite = redSprite;
                gameObject.GetComponent<Unity.VectorGraphics.SVGImage>().color = new Color(1, 1, 1, 1);
                gameObject.transform.GetChild(1).GetComponent<Text>().color = Color.black;
                correct.gameObject.GetComponent<Unity.VectorGraphics.SVGImage>().color = new Color(1,1,1,1);
                correct.gameObject.GetComponent<Unity.VectorGraphics.SVGImage>().sprite = greenSprite;
                correct.gameObject.transform.GetChild(1).GetComponent<Text>().color = Color.black; 
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
        Color32 myColor = new Color32(55, 233, 187,255);
        gameObject.GetComponent<Unity.VectorGraphics.SVGImage>().color = new Color(0,0,0,0);
        gameObject.transform.GetChild(1).GetComponent<Text>().color = myColor;
        correct.gameObject.GetComponent<Unity.VectorGraphics.SVGImage>().color = new Color(0, 0, 0, 0);
        correct.gameObject.transform.GetChild(1).GetComponent<Text>().color = myColor;
    }
    private void WorkonButton()
    {
        checkIfOneClicked = 0;
        gameController.AnswerButtonClicked(answerData.isCorrect);
        gameController.fadeImage.SetActive(false);
    }
}
