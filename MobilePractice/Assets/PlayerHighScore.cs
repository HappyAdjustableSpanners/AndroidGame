using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerHighScore : MonoBehaviour {

    public float startSize = 1f;
    public float highScoreSize = 1f;

    private float maxImageSize = 4f;

    public Image imgStartSize, imgHighScoreSize;

	// Use this for initialization
	void Start () {
        if (highScoreSize > maxImageSize)
        {
            //Set high score size to max
            imgHighScoreSize.transform.localScale = new Vector3( maxImageSize, maxImageSize, 1f);

            //Work out diff
            float diff = highScoreSize - maxImageSize;

            //% increase between current size(max) and actual size
            float percentIncrease = (diff / maxImageSize) + 1;

            imgStartSize.transform.localScale = new Vector3(imgStartSize.transform.localScale.x / percentIncrease, imgStartSize.transform.localScale.y / percentIncrease, 1f);
        }
        else
        {
            imgHighScoreSize.transform.localScale = new Vector3(highScoreSize, highScoreSize, 1f);
        }
    }
	
	// Update is called once per frame
	void Update () {
		
            
	}
}
