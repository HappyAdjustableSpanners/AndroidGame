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
    
    public static bool CheckColorEqual(Color c1, Color c2)
    {
        if (Mathf.Abs(c1.r - c2.r) < 0.1f
        && Mathf.Abs(c1.g - c2.g) < 0.1f
        && Mathf.Abs(c1.b - c2.b) < 0.1f)
        {
            return true;
        }
        else
            return false;
    }

    public static Vector2 FindRandomPointOnRectanglePerimeter(BoxCollider2D rect)
    {
        //Choose rand between 1 and 4
        float rand = Random.Range(1, 5);

        Vector2 point = Vector2.zero;
        if(rand == 1)
        {
            //Top edge
            float y = rect.bounds.min.y;
            point = new Vector2(Random.Range(rect.bounds.min.x, rect.bounds.max.x), y);     
        }
        else if(rand == 2)
        {
            //Bottom edge
            float y = rect.bounds.max.y;
            point = new Vector2(Random.Range(rect.bounds.min.x, rect.bounds.max.x), y);
        }
        else if(rand == 3)
        {
            //Left
            float x = rect.bounds.min.x;
            point = new Vector2(x, Random.Range(rect.bounds.min.y, rect.bounds.max.y));
        }
        else if(rand == 4)
        {
            //Right
            float x = rect.bounds.max.x;
            point = new Vector2(x, Random.Range(rect.bounds.min.y, rect.bounds.max.y));
        }

        return point;
    }

    public static Vector2 FindRandomPointInsideRectangle(BoxCollider2D rect)
    {
        Vector2 point = Vector2.zero;

        //Get random x and random y between size limits
        float x = Random.Range(rect.bounds.min.x, rect.bounds.max.x);
        float y = Random.Range(rect.bounds.min.y, rect.bounds.max.y);
        point.x = x;
        point.y = y;

        return point;
    }

   
}
