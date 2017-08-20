using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EyeMoveBehaviour : MonoBehaviour {

    private int closest;
    public Vector2 xLimits, yLimits;
    public Vector2 basePos;

    public List<GameObject> targets;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
        if (targets.Count > 0)
        {
            //Look at closest object
            transform.position = Vector3.Lerp(transform.position, targets[closest].transform.position, Time.deltaTime * 0.5f);

            //Clamp position
            float x = Mathf.Clamp(transform.localPosition.x, xLimits[0], xLimits[1]);
            float y = Mathf.Clamp(transform.localPosition.y, yLimits[0], yLimits[1]);

            transform.localPosition = new Vector3(x, y, 0f);
        }
        else
            transform.localPosition = Vector3.Lerp(transform.localPosition, new Vector3(basePos[0], basePos[1], 0), Time.deltaTime * 0.5f);
    }

    public void OnTrigEnter2D(Collider2D col)
    {
        if (col.tag.Contains("Enemy"))
        {
            if (!targets.Contains(col.gameObject))
            {
                targets.Add(col.gameObject);
            }

            CheckClosestTarget(targets);
        }
    }

    public void OnTrigExit2D(Collider2D col)
    {
        if (col.tag.Contains("Enemy"))
        {
            if (targets.Contains(col.gameObject))
            {
                targets.Remove(col.gameObject);
            }

            CheckClosestTarget(targets);
        }
    }

    private void CheckClosestTarget(List<GameObject> targets)
    {
        //If we have items in the list
        if (targets.Count > 0)
        {
            //Set closest to first item in list
            closest = 0;

            //Check if any of remaining objects are closer
            for(int i = 0; i < targets.Count; ++i)
            {
                //Get dist between current target
                float dist = (targets[i].transform.position - transform.position).magnitude;

                //Get dist of current closest target
                float currentClosestDist = (targets[closest].transform.position - transform.position).magnitude;

                //Check if dist is closer than current closest
                if (dist < currentClosestDist)
                {
                    closest = i;
                }
            }
        }
    }
}
