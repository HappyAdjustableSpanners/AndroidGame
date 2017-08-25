using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class Eater : MonoBehaviour {

    //State
    private bool eating = false;

    //Global reference to our prey obj
    private GameObject obj;

    //Sorting
    private int origSortingOrder;

    //Components
    public AudioClip eatSound;
    private AudioSource audioSource;
    private Rigidbody2D rb;

    //Other behaviours
    private EnemyController enemyController;
    private Growable growable;
    private wander wander;

    //Eyes
    private EyeMoveBehaviour[] eyes;

    //Global vars
    private List<GameObject> neighbours = new List<GameObject>();

    public bool ignoreSmallPrey = true;

	// Use this for initialization
	void Start () {

        //Get original renderer sorting order ( we need this to push the eater above other sprites)
        origSortingOrder = GetComponent<Renderer>().sortingOrder;

        //Get audio source (for eat sound)
        audioSource = GetComponent<AudioSource>();

        //Get rigidbody (for movement)
        rb = GetComponent<Rigidbody2D>();

        //Get other behaviours
        enemyController = GetComponent<EnemyController>();
        wander = GetComponent<wander>();
        growable = GetComponent<Growable>();

        //Get eyes
        eyes = GetComponentInChildren<EyeController>().GetEyes();
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (eating)
        {
            //If the obj is null, or it is now bigger than we are, set the obj to null (we no longer chase it)
            if (obj == null || obj.GetComponent<CircleCollider2D>().bounds.size.x > GetComponent<CircleCollider2D>().bounds.size.x || obj.GetComponent<CircleCollider2D>().bounds.size.x == GetComponent<CircleCollider2D>().bounds.size.x)
            {
                SetFinishedEating();
                return;
            }

            //Only move towards and look at prey if it is at least 1/4 of our size

            if( PreyIsBigEnough(obj) || !ignoreSmallPrey )
            {
                //Move towards the prey at move speed using rb velocity
                Vector3 normalizeddir = (obj.transform.position - transform.position).normalized;
                rb.velocity = normalizeddir * Time.deltaTime * wander.GetMoveSpeed();

                //Look at prey while we move towards it
                Vector3 dir = obj.transform.position - transform.position;
                float angle = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg;
                transform.rotation = Quaternion.Lerp(transform.rotation, Quaternion.AngleAxis(angle - 90, Vector3.forward), Time.deltaTime * wander.GetTurnSpeed() * 10);
            }
            
            //If we are fully overlapped, then eat
            if (MathFunctions.IsOverlapping(gameObject.GetComponent<CircleCollider2D>(), obj.GetComponent<CircleCollider2D>(), 0f))
            {
                //If we have eaten poison, we freeze as punishment, and destroy the poison
                if (obj.tag.Contains("Poison"))
                {                   
                    StartCoroutine("FreezeTimed");
                    Destroy(obj);
                }
                else
                {        
                    //Grow ourselves
                    float newSize = transform.localScale.x + obj.transform.localScale.x / 2;
                    growable.Grow(newSize);

                    //Set new turn speed 
                    wander.SetTurnSpeed(0.2f / newSize);

                    //Else, we destroy our prey obj
                    DestroyObj(obj);

                    //Play pop audio clip (randomise pitch)
                    audioSource.pitch = UnityEngine.Random.Range(0.5f, 1.5f);
                    audioSource.clip = eatSound;
                    audioSource.Play();

                    //Finish eating
                    SetFinishedEating();
                }
            }
        }
    }

    private bool PreyIsBigEnough(GameObject obj)
    {
        if (obj.GetComponent<CircleCollider2D>().bounds.size.x / GetComponent<CircleCollider2D>().bounds.size.x > 0.33f)
            return true;
        else
            return false;
    }

    private void SetFinishedEating()
    {
        //Set eating to false, set state back to wandering, and restore the original sorting order for this sprite
        eating = false;

        //If we still (for some reason), have prey, then set it to null
        if(obj)
        {
            obj = null;
        }

        enemyController.SetState("wandering");
    }

    private void DestroyObj(GameObject objToDestroy)
    {
        //Destroy() does not do exactly what we want. So we make our own function
        //We want to trail to remain after the enemy is destroyed, so we unparent it, and destroy it after a delay of 1s

        //Check if the objToDestroy has a trail
        if (objToDestroy.transform.Find("Trail") != null)
        {
            //Unparent child trail
            GameObject trail = objToDestroy.transform.Find("Trail").gameObject;
            trail.transform.SetParent(null, false);

            //Unparent squeeker
            GameObject squeeker = objToDestroy.transform.Find("Squeeker").gameObject;
            squeeker.transform.SetParent(null, false);

            //Destroy trail after 1s delay
            Destroy(trail, 1f);

            //Destroy squeeker after 2s delay
            Destroy(squeeker, 2f);
        }

        //If we have destroyed the player, trigger event
        if (obj.tag == "Player")
        {
            EventManager.OnPlayerKilled();
        }

        //Destroy parent (this will no longer destroy trail)
        Destroy(objToDestroy);
    }

    private IEnumerator FreezeTimed()
    {
        //Reduce speed to 0, wait for 3s, then restore original speed
        float origMoveSpeed = wander.GetMoveSpeed();
        wander.SetMoveSpeed(0f);
        yield return new WaitForSeconds(3f);
        wander.SetMoveSpeed(origMoveSpeed);
    }

    private void OnTriggerStay2D(Collider2D col)
    {
            //If we have collided with prey
            if ((col.tag.Contains("Enemy") || col.CompareTag("Player")))
            {
                //If the prey is valid
                if (IsValidPrey(col))
                {
                    //If we are not touching any other enemies, return to original sorting order
                    if (neighbours.Count <= 0)
                    {
                        ResetSortingOrder();
                    }

                    //If our list does not already contain this neighbour, add it
                    if (!neighbours.Contains(col.gameObject))
                    {
                        neighbours.Add(col.gameObject);
                    }

                    //Handle sorting order to ensure this sprite is above the prey sprite
                    CalculateSortingOrder();

                    //If we do not already have prey
                    if (obj == null && !eating)
                    {
                        //Set our global prey variable
                        obj = col.gameObject;

                        //Set eating to true
                        eating = true;

                        //Set controller state to eating
                        enemyController.SetState("eating");
                    }
                }
            }
    }

    private void ResetSortingOrder()
    {
        //Reset sorting order to our recorded original sorting order
        GetComponent<Renderer>().sortingOrder = origSortingOrder;

        foreach (EyeMoveBehaviour e in eyes)
        {
            e.GetComponent<Renderer>().sortingOrder = origSortingOrder + 1;
        }
    }

    private void OnTriggerExit2D(Collider2D col)
    {
        if (neighbours.Contains(col.gameObject))
        {
            neighbours.Remove(col.gameObject);
        }

        //If we have prey, we want to check if it is our prey that has left our trigger
        if (obj != null)
        {
            //Check if it is our current prey
            if (col.gameObject.name.Equals(obj.name))
            {
                //We have lost the prey we were chasing
                SetFinishedEating();
            }
        }
    }

    private bool IsValidPrey(Collider2D col)
    {
        //There are a number of rules which determine whether this is valid prey

        //Check that this is the correct collider (e.g. not the eyes collider that is a box collider
        if(col.GetType() != typeof(CircleCollider2D))
        {
            return false;
        }
        else if (col.bounds.size.x > GetComponent<CircleCollider2D>().bounds.size.x)
        {
            //If the other obj is bigger than us, return 
            return false;
        }
        else if (col.tag.Contains("Poison") && transform.CompareTag("Enemy_Purple"))
        {
            return false;
        }
        

        //If none of the rules above have been violated, return true
        return true;
    }

    private void CalculateSortingOrder()
    {
        //Keep track of max sorting order
        int maxSortingOrder = 0;

        //Remove any null elements
        for(int i = 0; i < neighbours.Count; i++)
        {
            if (neighbours[i] == null)
            {
                neighbours.RemoveAt(i);
            }
        }

        //Find max sorting order 
        for (int i = 0; i < neighbours.Count; i++)
        {
            if(neighbours[i].GetComponent<Renderer>().sortingOrder > maxSortingOrder)
            {
                maxSortingOrder = neighbours[i].GetComponent<Renderer>().sortingOrder;
            }
        }

        //Set our sorting order to one above the max
        GetComponent<Renderer>().sortingOrder = maxSortingOrder + 2;

        //Set sorting order of eyes
        foreach (EyeMoveBehaviour e in eyes)
        {
            e.GetComponent<Renderer>().sortingOrder = maxSortingOrder + 3;
        }
    }



    public CircleCollider2D GetCollider2D()
    {
        return GetComponent<CircleCollider2D>();
    }
}
