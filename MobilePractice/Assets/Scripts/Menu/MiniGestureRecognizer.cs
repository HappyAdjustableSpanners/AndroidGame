using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MiniGestureRecognizer : MonoBehaviour
{

    public enum SwipeDirection
    {
        Up,
        Down,
        Right,
        Left
    }

    //public static event Action<SwipeDirection> Swipe;
    private bool swiping = false;
    private bool eventSent = false;
    private Vector2 lastPosition;

    void Update()
    {
        if (Input.touchCount == 0)
            return;

        if (Input.GetTouch(0).deltaPosition.sqrMagnitude != 0)
        {
            if (swiping == false)
            {
                swiping = true;
                lastPosition = Input.GetTouch(0).position;
                return;
            }
            else
            {
                if (!eventSent)
                {
                    if (/*Swipe != null*/ true )
                    {
                        Vector2 direction = Input.GetTouch(0).position - lastPosition;

                        if (Mathf.Abs(direction.x) > Mathf.Abs(direction.y))
                        {
                            if (direction.x > 0)
                                EventManager.OnSwipeDetected(1); //Right
                            else
                                EventManager.OnSwipeDetected(2); //Left
                        }
                        else
                        {
                            //if (direction.y > 0)
                                //EventManager.OnSwipeDetected(2); //Up
                            //else
                                //EventManager.OnSwipeDetected(3); //Down
                        }

                        eventSent = true;
                    }
                }
            }
        }
        else
        {
            swiping = false;
            eventSent = false;
        }
    }
}