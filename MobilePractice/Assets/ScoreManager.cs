using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;

public class ScoreManager : MonoBehaviour {

    public Text scoreText;
    private int score = 0;
    public Slider progressSlider;
    

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    public void IncrementScore(int value)
    {
        score += value;
        UpdateScoreText();
        progressSlider.value += 0.1f;

        if(progressSlider.value >= progressSlider.maxValue)
        {
            progressSlider.value = 0f;
            //progressSlider.maxValue += 1f;
            GameObject.FindGameObjectWithTag("GameManager").GetComponent<GameManager>().IncrementStage();
        }
    }

    private void UpdateScoreText()
    {
        scoreText.text = score.ToString();
    }
}
