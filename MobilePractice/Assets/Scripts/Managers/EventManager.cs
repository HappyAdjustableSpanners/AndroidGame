using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EventManager : MonoBehaviour {

    //Delegates (format of the message to be sent
    public delegate void SwipeDetected(int direction );
    public delegate void PlayerKilled();
    public delegate void StageUp(int currentStage);
    public delegate void orthoSizeChanged(float newOrthoSize);

    //Events based on those delegates
    public static event SwipeDetected swipeDetectedMethods;
    public static event PlayerKilled playerKilledMethods;
    public static event StageUp stageUpMethods;
    public static event orthoSizeChanged orthoSizeChangedMethods;

    public static void OnSwipeDetected(int direction)
    {
        swipeDetectedMethods(direction); 
    }

    public static void OnPlayerKilled()
    {
        playerKilledMethods();
    }

    public static void OnStageUp(int currentStage)
    {
        stageUpMethods(currentStage);
    }

    public static void OnOrthoSizeChanged(float newOrthoSize)
    {
        orthoSizeChangedMethods(newOrthoSize);
    }

    public static void ClearAll()
    {
        swipeDetectedMethods = null;
        stageUpMethods = null;
        playerKilledMethods = null;
        orthoSizeChangedMethods = null;
    }
}
