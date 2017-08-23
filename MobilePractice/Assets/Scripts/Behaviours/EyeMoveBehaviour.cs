using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EyeMoveBehaviour : MonoBehaviour {


    private Transform target;

    //Local position limits and base pos of the eye
    public Vector2 xLimits, yLimits;
    public Vector2 basePos;
	
	// Update is called once per frame
	void Update () {

        //If we have targets
        if (target != null)
        {
            //Lerp global pos to closest target global pos
            transform.position = Vector3.Lerp(transform.position, target.position, Time.deltaTime * 0.5f);

            //Clamp local position to limits
            float x = Mathf.Clamp(transform.localPosition.x, xLimits[0], xLimits[1]);
            float y = Mathf.Clamp(transform.localPosition.y, yLimits[0], yLimits[1]);
            transform.localPosition = new Vector3(x, y, 0f);
        }
        else
            transform.localPosition = Vector3.Lerp(transform.localPosition, new Vector3(basePos[0], basePos[1], 0), Time.deltaTime * 1f); //Set back to base pos
    }   

    public void SetTarget(GameObject target)
    {
        this.target = target.transform;
    }
}
