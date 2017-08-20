using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;

public class ScoreManager : MonoBehaviour {

    public Text scoreText;
    public int score = 0;
    private int scoreTarget = 100;
    public Slider progressSlider;
    private bool moveSlider = false;
    private float newSliderVal = 0f;
    

	// Use this for initialization
	void Start () {
        //newSliderVal = progressSlider.value;
	}
	
	// Update is called once per frame
	void Update () {
        //if (Mathf.Abs(progressSlider.value - newSliderVal) > 0.1f)
        //{
        //    progressSlider.value = Mathf.Lerp(progressSlider.value, newSliderVal, Time.deltaTime);
        //}
	}

    public void IncrementScore(int value)
    {
        score += value;
        UpdateScoreText();

        if(score >= scoreTarget)
        {
            scoreTarget += scoreTarget;
            GameObject.FindGameObjectWithTag("GameManager").GetComponent<GameManager>().IncrementStage();
        }
        //newSliderVal += 0.2f;
        //
        //
        //if(progressSlider.value >= progressSlider.maxValue)
        //{
        //    newSliderVal = 0f;
        //    //progressSlider.maxValue += 1f;
        //    GameObject.FindGameObjectWithTag("GameManager").GetComponent<GameManager>().IncrementStage();
        //}
    }

    private void UpdateScoreText()
    {
        scoreText.text = score.ToString();
    }
}
