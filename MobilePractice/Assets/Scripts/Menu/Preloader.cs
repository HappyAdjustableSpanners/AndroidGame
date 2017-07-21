using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Preloader : MonoBehaviour {

    private CanvasGroup fadeGroup;
    private float loadTime;
    private float minimumLogoTime = 3.0f; //Min time of that scene

    private void Start()
    {
        //Grab the only canvasgroup in the scene
        fadeGroup = FindObjectOfType<CanvasGroup>();

        //Start with white screen
        fadeGroup.alpha = 1f;

        //Pre-load the game
        // $$

        //Get timestamp of the completion time
        //If loadtime is super fast, give it a small buffer time so we can appreciate the logo
        if (Time.time < minimumLogoTime)
        {
            loadTime = minimumLogoTime;
        }
        else
            loadTime = Time.time; 
    }

    private void Update()
    {
        //Fade-in
        if( Time.time < minimumLogoTime )
        {
            fadeGroup.alpha = 1 - Time.time;
        }

        //Fade-out
        if( Time.time > minimumLogoTime && loadTime != 0 )
        {
            fadeGroup.alpha = Time.time - minimumLogoTime;
            if(fadeGroup.alpha >= 1)
            {
                SceneManager.LoadScene("Menu");
            }
        }
    }
}
