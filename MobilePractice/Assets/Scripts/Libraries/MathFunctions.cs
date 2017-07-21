using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MathFunctions : MonoBehaviour
{

    // Use this for initialization
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    public static float WrapAngle(float angle)
    {
        //Wrap around
        if (angle > 360)
        {
            angle = 0f;
        }
        else if (angle < 0)
        {
            angle = 360f;
        }
        return angle;
    }

    public static bool IsOverlapping(CircleCollider2D circleOverlapping, CircleCollider2D circle, float buffer)
    {      
        if (circleOverlapping.bounds.max.x + buffer > circle.bounds.max.x &&
            circleOverlapping.bounds.min.x - buffer < circle.bounds.min.x &&
            circleOverlapping.bounds.max.y + buffer > circle.bounds.max.y &&
            circleOverlapping.bounds.min.y - buffer < circle.bounds.min.y)
        {
            return true;
        }
        else
            return false;
    }

   
}
