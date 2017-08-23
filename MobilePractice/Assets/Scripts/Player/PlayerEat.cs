using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerEat : MonoBehaviour {

    //State
    private bool eating = false;

    //Global reference to our prey obj
    private GameObject obj;

    //Other player behaviours
    private PlayerMoveJoystick playerMove;

    //Sorting
    private int origSortingOrder;

    //Components
    private ScoreManager scoreManager;

    public AudioClip eatSound;
    private AudioSource audioSource;

    //A tool to allow debugging, by manually upping the score
    public bool upScore = false;

    // Use this for initialization
    void Start()
    {
        //Get original renderer sorting order ( we need this to push the eater above other sprites)
        origSortingOrder = GetComponent<Renderer>().sortingOrder;

        //Get score manager (we need this to up the score when the player eats)
        scoreManager = GameObject.FindGameObjectWithTag("ScoreManager").GetComponent<ScoreManager>();

        //Get audio source (for eat sound)
        audioSource = GetComponent<AudioSource>();

        //Get other player behaviours
        playerMove = GetComponent<PlayerMoveJoystick>();
    }

    // Update is called once per frame
    void Update()
    {
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
                //We are close, so to finish, MoveTowards       
                transform.position = Vector3.MoveTowards(transform.position, obj.transform.position, Time.deltaTime * 3f);

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
                        StartCoroutine("BoostTimed");
                        Destroy(obj);
                    }
                    else
                    {
                        //Else, we destroy our prey obj
                        DestroyObj(obj);

                        //Increment score
                        scoreManager.IncrementScore(10);

                        //Play pop audio clip (randomise pitch)
                        audioSource.pitch = Random.Range(0.5f, 1.3f);
                        audioSource.clip = eatSound;
                        audioSource.Play();

                        //Finish eating
                        SetFinishedEating();
                    }
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
        //Reduce speed to 0, wait for 3s, then restore original speed
        float origMoveSpeed = playerMove.GetMoveSpeed();
        playerMove.SetAllowMovement(false);
        yield return new WaitForSeconds(3f);
        playerMove.SetAllowMovement(true);
    }

    private IEnumerator BoostTimed()
    {
        //Boost to x2 speed for x seconds, then return to orig speed
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

        //If we don't current have prey, and we have collided with prey, and we are not eating
        if (col.tag.Contains("Enemy") && obj == null && !eating)
        {
            //If the prey is valid
            if(IsValidPrey(col.gameObject))
            {
                //Set our global prey variable
                obj = col.gameObject;

                //Handle sorting order to ensure this sprite is above the prey sprite
                PushThisSpriteAboveOther(col);

                //Finally, set eating to true
                eating = true;
                GetComponent<PlayerController>().setState("eating");
            }           
        }
    }

    private void OnTriggerExit2D(Collider2D col)
    {
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


    //private void PushThisSpriteAboveOther(Collider2D col)
    //{
    //    //Set sorting layer to one above the max 
    //    Collider2D[] hitColliders = Physics2D.OverlapCircleAll(transform.position, GetComponent<CircleCollider2D>().radius);
    //    foreach (Collider2D collider in hitColliders)
    //    {
    //        if (collider.tag.Contains("Enemy"))
    //        {
    //            GetComponent<Renderer>().sortingOrder = col.gameObject.GetComponent<Renderer>().sortingOrder + 1;
    //        }
    //    }
    //}

    private void PushThisSpriteAboveOther(Collider2D col)
    {
        //Set sorting layer to one above the max 
        Collider2D[] hitColliders = Physics2D.OverlapCircleAll(transform.position, GetComponent<CircleCollider2D>().radius);
        foreach (Collider2D collider in hitColliders)
        {
            if (collider.tag.Contains("Enemy") && !collider.CompareTag("Enemy_Food"))
            {
                if (collider.transform.Find("RightEye").GetComponent<Renderer>() != null)
                {

                    if (transform.Find("RightEye").GetComponent<Renderer>() != null)
                    {
                        //Get prey sorting order
                        int colSortingOrder = collider.gameObject.transform.Find("RightEye").GetComponent<Renderer>().sortingOrder;

                        //Set our body sprite sorting order to one above prey 
                        GetComponent<Renderer>().sortingOrder = colSortingOrder + 1;

                        //Get right eye and left eye renderer
                        EyeMoveBehaviour[] eyes = GetComponentInChildren<EyeController>().GetEyes();
                        foreach (EyeMoveBehaviour e in eyes)
                        {
                            e.GetComponent<Renderer>().sortingOrder = colSortingOrder + 2;
                        }
                        //Renderer rend1 = transform.Find("RightEye").GetComponent<Renderer>();
                        //Renderer rend2 = transform.Find("LeftEye").GetComponent<Renderer>();
                        //
                        ////Set eye sorting orders to 2 above the prey body
                        //rend1.sortingOrder = colSortingOrder + 2;
                        //rend2.sortingOrder = colSortingOrder + 2;
                    }
                }
            }
        }
    }

    public bool getEating()
    {
        return eating;
    }
}
