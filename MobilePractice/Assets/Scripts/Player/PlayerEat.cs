using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerEat : MonoBehaviour {

    //State
    private bool eating = false;
    private bool poisoned = false;

    //Global reference to our prey obj
    private GameObject obj;

    //Other player behaviours
    private PlayerMoveJoystick playerMove;

    //Sorting
    private int origSortingOrder;

    //Components
    private ScoreManager scoreManager;

    public AudioClip eatSound;
    public AudioSource audioSource;

    //A tool to allow debugging, by manually upping the score
    public bool upScore = false;

    //Get eyes
    private EyeMoveBehaviour[] eyes;

    //Global vars
    private List<GameObject> neighbours = new List<GameObject>();

    //Sprite renderer
    private SpriteRenderer sr;

    // Use this for initialization
    void Start()
    {
        //Get sr
        sr = GetComponent<SpriteRenderer>();

        //Get original renderer sorting order ( we need this to push the eater above other sprites)
        origSortingOrder = GetComponent<Renderer>().sortingOrder;

        //Get score manager (we need this to up the score when the player eats)
        scoreManager = GameObject.FindGameObjectWithTag("ScoreManager").GetComponent<ScoreManager>();

        //Get other player behaviours
        playerMove = GetComponent<PlayerMoveJoystick>();

        //Get eyes
        eyes = GetComponentInChildren<EyeController>().GetEyes();
    }

    // Update is called once per frame
    void Update()
    {
        if(poisoned)
        {
            sr.color = Color.Lerp(sr.color, Color.magenta, Time.deltaTime);
        }
        else if( !Color.Equals(sr.color, Color.white) )
        {
            sr.color = Color.Lerp(sr.color, Color.white, Time.deltaTime);
        }

        if (eating)
        {
            //If the obj is null, or it is now bigger than we are, stop eating and return
            if (obj == null || obj.GetComponent<CircleCollider2D>().bounds.size.x > GetComponent<CircleCollider2D>().bounds.size.x || obj.GetComponent<CircleCollider2D>().bounds.size.x == GetComponent<CircleCollider2D>().bounds.size.x)
            {
                SetFinishedEating();
                return;
            }
           
            //Check if we have almost overlapped / engulfed prey
            //Lerp slows down as we near the target (meaning it looks super smooth and nice). However, this can result in the eater never fully reaching and engulfing the prey.
            //Vector3.MoveTowards would solve this, as it does not speed-up/slow-down. However, it does not look smooth. 
            //The solution I came to is that the predator will lerp smoothly to within a certain proximity, and then finish off by 'MoveTowards' meaning it will fully reach its goal whilst stil looking quite smooth
            if (MathFunctions.IsOverlapping(gameObject.GetComponent<CircleCollider2D>(), obj.GetComponent<CircleCollider2D>(), 0.2f))
            {
                //Only do this if we are facing target to within 90 degrees
                if (Vector3.Angle(transform.up, obj.transform.position - transform.position) < 45)
                {
                    //We are close, so to finish, MoveTowards       
                    transform.position = Vector3.MoveTowards(transform.position, obj.transform.position, Time.deltaTime * 3f);
                }

                //If we are fully overlapped, then eat
                if (MathFunctions.IsOverlapping(gameObject.GetComponent<CircleCollider2D>(), obj.GetComponent<CircleCollider2D>(), 0))
                {
                    //If we have eaten poison, we freeze as punishment, and destroy the poison
                    if (obj.tag.Contains("Poison"))
                    {
                        StartCoroutine("FreezeTimed");
                        Destroy(obj);
                    }
                    else if(obj.tag.Contains("Food"))
                    {
                        //if we are already boosting
                        if (playerMove.GetBoosting() == true)
                        {
                            StopCoroutine("BoostTimed");
                        }

                        StartCoroutine("BoostTimed");
                        scoreManager.IncrementScore(2);
                        Destroy(obj);
                    }
                    else
                    {
                        //Else, we destroy our prey obj
                        DestroyObj(obj);

                        //Increment score
                        scoreManager.IncrementScore(10);

                        //Finish eating
                        SetFinishedEating();
                    }

                    //Play pop audio clip (randomise pitch)
                    audioSource.pitch = UnityEngine.Random.Range(0.5f, 1.5f);
                    audioSource.clip = eatSound;
                    audioSource.Play();
                }
            }
        }

        //For debugging only - manually increase score
        if(upScore == true)
        {
            scoreManager.IncrementScore(10);
            upScore = false;
        }
    }

    private void SetFinishedEating()
    {
        //Set eating to false, and restore the original sorting order for this sprite
        eating = false;
        GetComponent<Renderer>().sortingOrder = origSortingOrder;

        foreach (EyeMoveBehaviour e in eyes)
        {
            e.GetComponent<Renderer>().sortingOrder = origSortingOrder + 1;
        }

        //Set state back to moving
        GetComponent<PlayerController>().setState("moving");
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

            //Destroy it after 1s delay
            Destroy(trail, 1f);
        }

        //Destroy parent (this will no longer destroy trail)
        Destroy(objToDestroy);
    }

    private IEnumerator FreezeTimed()
    {
        //Disable eyes
        transform.Find("RightEye").transform.gameObject.SetActive(false);
        transform.Find("LeftEye").transform.gameObject.SetActive(false);
        GetComponent<SpriteRenderer>().sprite = Resources.Load<Sprite>("Sprites/player_sprite_dead");
        poisoned = true;

        //Reduce speed to 0, wait for 3s, then restore original speed
        float origMoveSpeed = playerMove.GetMoveSpeed();
        playerMove.SetSpeedFactor(2f);
        yield return new WaitForSeconds(3f);
        playerMove.SetSpeedFactor(5f);

        transform.Find("RightEye").transform.gameObject.SetActive(true);
        transform.Find("LeftEye").transform.gameObject.SetActive(true);
        GetComponent<SpriteRenderer>().sprite = Resources.Load<Sprite>("Sprites/player_sprite_alive");
        transform.Find("poison_radial_countdown").transform.gameObject.SetActive(false);
        poisoned = false;
    }

    private IEnumerator BoostTimed()
    {
        float origMoveSpeed = playerMove.GetMoveSpeed();
        playerMove.SetBoosting(true);
        ParticleSystem.MainModule main = transform.Find("Trail").GetComponent<ParticleSystem>().main;
        main.startColor = Color.green;
        yield return new WaitForSeconds(3f);
        main.startColor = Color.black;
        playerMove.SetBoosting(false);
    }

    private void OnTriggerStay2D(Collider2D col)
    {
        if(!eating && obj != null)
        {
            //if we are not eating, but we have prey, this is always a bug, and we can put in a bodge fix here
            obj = null;
        }

        //If we have collided with prey
        if (col.tag.Contains("Enemy"))
        {
            //If the prey is valid
            if(IsValidPrey(col.gameObject))
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

                CalculateSortingOrder();

                if (obj == null && !eating)
                {
                    //Set our global prey variable
                    obj = col.gameObject;

                    //Finally, set eating to true
                    eating = true;
                    GetComponent<PlayerController>().setState("eating");
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
                //We have lost the prey we were chasing, so finish eating
                SetFinishedEating();
            }
        }
    }

    private bool IsValidPrey(GameObject prey)
    {
        //There are a number of rules which determine whether this is valid prey

        //If the other obj is bigger than us, return 
        if (prey.GetComponent<CircleCollider2D>().bounds.size.x > GetComponent<CircleCollider2D>().bounds.size.x)
        {
            //Other obj is bigger
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
        for (int i = 0; i < neighbours.Count; i++)
        {
            if (neighbours[i] == null)
            {
                neighbours.RemoveAt(i);
            }
        }

        //Find max sorting order 
        foreach (GameObject go in neighbours)
        {
            //If current sorting order is more than our max sorting order, set it to max
            if (go.GetComponent<Renderer>().sortingOrder > maxSortingOrder)
            {
                maxSortingOrder = go.GetComponent<Renderer>().sortingOrder;
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

    public bool getEating()
    {
        return eating;
    }
}
