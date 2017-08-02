using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Fade : MonoBehaviour {

    public bool fadeIn = true;
	private CanvasGroup fadeGroup;
    public float fadeInSpeed = 0.33f;

	// Use this for initialization
	void Start () {

        //Grab the only canvasgroup in the scene
        fadeGroup = FindObjectOfType<CanvasGroup>();

        //Start with white screen
        fadeGroup.alpha = 1;
    }

    // Update is called once per frame
    void Update() {
        if (fadeIn)
        { 
            //Fade-in
            fadeGroup.alpha = 0 + Time.timeSinceLevelLoad * fadeInSpeed;
        }
        else if(!fadeIn)
        {
            //Fade-out
            fadeGroup.alpha = 1 - Time.timeSinceLevelLoad * fadeInSpeed;
        }
	}

}
