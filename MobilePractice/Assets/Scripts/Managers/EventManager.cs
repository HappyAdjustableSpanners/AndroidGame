using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EventManager : MonoBehaviour {

    //Delegates (format of the message to be sent
    public delegate void SwipeDetected(int direction );
    public delegate void PlayerKilled();

    //Events based on those delegates
    public static event SwipeDetected swipeDetectedMethods;
    public static event PlayerKilled playerKilledMethods;

    public static void OnSwipeDetected(int direction)
    {
        swipeDetectedMethods(direction);
    }

    public static void OnPlayerKilled()
    {
        playerKilledMethods();
    }
}
