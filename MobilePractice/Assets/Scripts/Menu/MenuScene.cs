using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuScene : MonoBehaviour {

    private CanvasGroup fadeGroup;
    private float fadeInSpeed = 0.33f;

    public RectTransform menuContainer;
    public Vector3 desiredMenuPosition;
    private float currentMenuPos = 0;

	// Use this for initialization
	void Start () {

        //Grab the only canvasgroup in the scene
        fadeGroup = FindObjectOfType<CanvasGroup>();

        //Start with white screen
        fadeGroup.alpha = 1;

        //Subscripe to swipe events
        EventManager.swipeDetectedMethods += OnNavigationSwipeDetected;
    }
	
	// Update is called once per frame
	void Update () {
        //Fade-in
        fadeGroup.alpha = 1 - Time.timeSinceLevelLoad * fadeInSpeed;

        //Menu navigation
        menuContainer.anchoredPosition = Vector2.Lerp(menuContainer.anchoredPosition, desiredMenuPosition, 0.1f);
	}

    public void OnPlayPressed()
    {
        Debug.Log("Play Button Pressed");
        SceneManager.LoadScene("Play");
        GetComponent<AudioSource>().Play();
    }

    private void NavigateTo(int menuIndex)
    {
        switch(menuIndex)
        {
            default:
            case 0:
                desiredMenuPosition = Vector2.zero;
                break;                      
            case 1:                         
                desiredMenuPosition = Vector2.right * 640;
                break;                      
            case 2:                         
                desiredMenuPosition = Vector2.left * 640;
                break;
        }
    }

    private void OnNavigationSwipeDetected(int menuIndex)
    {
        //Store current menu index

        Debug.Log("Swipe Detected");
        switch (menuIndex)
        {
            default:
            case 0:
                desiredMenuPosition = Vector2.zero;
                break;
            case 1:
                desiredMenuPosition = new Vector2(currentMenuPos + 640, 0f);
                break;
            case 2:
                desiredMenuPosition = new Vector2(currentMenuPos - 640, 0f);
                break;
        }

        desiredMenuPosition = new Vector2(Mathf.Clamp(desiredMenuPosition.x, -640, 640), 0f);

        currentMenuPos = desiredMenuPosition.x;
    }
}
