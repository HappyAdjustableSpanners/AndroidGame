using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Eater : MonoBehaviour {

    //State
    private bool eating = false;

    //Global reference to our prey obj
    private GameObject obj;

    //Sorting
    private int origSortingOrder;

    //Components
    private AudioSource audioSource;
    private Rigidbody2D rb;

    //Other behaviours
    private EnemyController enemyController;
    private Growable growable;
    private wander wander;

    //Global vars
    private float growSize;
    
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

            //Move towards the prey at move speed using rb velocity
            Vector3 normalizeddir = (obj.transform.position - transform.position).normalized;
            rb.velocity = normalizeddir * Time.deltaTime * wander.GetMoveSpeed();

            //Look at prey while we move towards it
            Vector3 dir = obj.transform.position - transform.position;
            float angle = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg;
            transform.rotation = Quaternion.Lerp(transform.rotation, Quaternion.AngleAxis(angle - 90, Vector3.forward), Time.deltaTime * wander.GetTurnSpeed() * 10);

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

                    //Else, we destroy our prey obj
                    DestroyObj(obj);

                    //Play pop audio clip (randomise pitch)
                    audioSource.pitch = Random.Range(0.5f, 1.5f);
                    audioSource.Play();

                    //Finish eating
                    SetFinishedEating();
                }
            }
        }
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
        GetComponent<Renderer>().sortingOrder = origSortingOrder;
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
        //If we don't current have prey, and we have collided with prey (or player), and we are not eating
        if (col.tag.Contains("Enemy") || col.CompareTag("Player") && obj == null && !eating)
        {
            //If the prey is valid
            if (IsValidPrey(col.gameObject))
            {
                //Set our global prey variable
                obj = col.gameObject;

                //Handle sorting order to ensure this sprite is above the prey sprite
                PushThisSpriteAboveOther(col);

                //Finally, set eating to true
                eating = true;

                //Set controller state to eating
                enemyController.SetState("eating");
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
                //We have lost the prey we were chasing
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
        else if (prey.tag.Contains("Poison") && transform.CompareTag("Enemy_Purple"))
        {
            return false;
        }

        //If none of the rules above have been violated, return true
        return true;
    }

    private void PushThisSpriteAboveOther(Collider2D col)
    {
        //Set sorting layer to one above the max 
        Collider2D[] hitColliders = Physics2D.OverlapCircleAll(transform.position, GetComponent<CircleCollider2D>().radius);
        foreach (Collider2D collider in hitColliders)
        {
            if (collider.tag.Contains("Enemy"))
            {
                GetComponent<Renderer>().sortingOrder = col.gameObject.GetComponent<Renderer>().sortingOrder + 1;
            }
        }
    }
}
