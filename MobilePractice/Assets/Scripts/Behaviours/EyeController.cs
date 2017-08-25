using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class EyeController : MonoBehaviour {

    //We want both eyes to have the same trigger, so we detect trigger here and send to both

    //Eyes
    public EyeMoveBehaviour[] eyes;
    private BoxCollider2D sightRadius;

    //Index of closest target
    private int closest;

    //List of targets
    public List<GameObject> targets;
    public GameObject prevTarget;

    void Start()
    {
        sightRadius = GetComponent<BoxCollider2D>();
        sightRadius.edgeRadius = transform.parent.localScale.x * 42f;
    }

    void Update()
    {
        //Send target to eyes, if it hasnt changed
        if (targets.Count > 0 && targets[closest] != null)
        {
            if (targets[closest] != prevTarget)
            {
                prevTarget = targets[closest];
                foreach(EyeMoveBehaviour e in eyes)
                {
                    e.SetTarget(targets[closest]);
                }
            }
        }
    }
    //Send trigger to both eyes
    void OnTriggerEnter2D(Collider2D col)
    {
        //If we see an enemy, add it to our list (if its not already there), then check if we have any closer targets
        if ((col.tag.Contains("Enemy") || col.tag == "Player") && col.name != this.transform.parent.name)
        {
            if (!targets.Contains(col.gameObject))
            {
                targets.Add(col.gameObject);
            }
            CheckClosestTarget(targets);

            //Update sight radius
            try
            {


                sightRadius.edgeRadius = transform.parent.localScale.x * 10f;
            }
            catch(NullReferenceException ex)
            {
                Debug.Log("Hello");
            }
        }
    }

    //Send triggers to both eyes
    void OnTriggerExit2D(Collider2D col)
    {
        //If we lose sight of an enemy, remove it from our list, then check if we have any closer targets
        if ((col.tag.Contains("Enemy") || col.tag == "Player") && col.name != this.transform.parent.name)
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
            for (int i = 0; i < targets.Count; ++i)
            {
                //Get dist between current target
                if (targets[i] != null && targets[closest] != null)
                {
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

    public EyeMoveBehaviour[] GetEyes()
    {
        return eyes;
    }
}
