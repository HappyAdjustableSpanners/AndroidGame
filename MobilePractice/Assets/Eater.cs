using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Eater : MonoBehaviour {

    private EnemyController enemyController;

    private bool eating = false;
    private bool growing = false;
    private GameObject obj;
    private int origSortingOrder;
    private float eatSpeed;
    private float growSize;

    public float growSpeed = 1f;

    private AudioSource audioSource;
    private Rigidbody2D rb;
    private wander wander;
    private GameObject player;
    

	// Use this for initialization
	void Start () {
        enemyController = GetComponent<EnemyController>();
        origSortingOrder = GetComponent<Renderer>().sortingOrder;
        eatSpeed = GetComponent<wander>().GetMoveSpeed() * 3;
        audioSource = GetComponent<AudioSource>();
        rb = GetComponent<Rigidbody2D>();
        wander = GetComponent<wander>();
        player = GameObject.FindGameObjectWithTag("Player");
    }

    // Update is called once per frame
    void Update()
    {
        if (eating)
        {
            //IF the obj is null, or it is now bigger than we are, set the obj to null (we no longer chase it)
            if (obj != null)
            {
                if (obj.GetComponent<CircleCollider2D>().bounds.size.x > GetComponent<CircleCollider2D>().bounds.size.x || obj.GetComponent<CircleCollider2D>().bounds.size.x == GetComponent<CircleCollider2D>().bounds.size.x)
                {
                    SetFinishedEating();
                    return;
                }

                //Move towards until we lose track of it
                Vector3 normalizeddir = (obj.transform.position - transform.position).normalized;

                rb.velocity = normalizeddir * Time.deltaTime * wander.GetMoveSpeed();

                //Look at
                Vector3 dir = obj.transform.position - transform.position;
                float angle = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg;
                transform.rotation = Quaternion.Lerp(transform.rotation, Quaternion.AngleAxis(angle - 90, Vector3.forward), Time.deltaTime * wander.GetTurnSpeed() * 10);

                if (MathFunctions.IsOverlapping(gameObject.GetComponent<CircleCollider2D>(), obj.GetComponent<CircleCollider2D>(), 0.05f))
                {
                    if (obj.tag.Contains("Poison"))
                    {
                        //Destroy ourselves
                        StartCoroutine("FreezeTimed");
                        Destroy(obj);
                    }
                    else
                    {
                        //We have eaten
                        Grow();

                        //Play pop audio clip (randomise pitch)
                        audioSource.pitch = Random.Range(0.5f, 1.5f);
                        audioSource.Play();

                        //Destroy the obj
                        DestroyObj(obj);

                        //Finish eating
                        SetFinishedEating();                       
                    }
                }
            }
            else
                SetFinishedEating();
         
        }

        if (growing)
        {
            if (player != null)
            {
                if (transform.localScale.x < player.transform.localScale.x * 5f)
                {
                    transform.localScale = Vector3.Lerp(transform.localScale, new Vector3(growSize, growSize, 1f), Time.deltaTime * growSpeed);
                }
            }
        }
    }

    /*void Update()
    {
        if (eating)
        {
            //IF the obj is null, or it is now bigger than we are, set the obj to null (we no longer chase it)
            if (obj != null)
            {
                if (obj.GetComponent<CircleCollider2D>().bounds.size.x > GetComponent<CircleCollider2D>().bounds.size.x || obj.GetComponent<CircleCollider2D>().bounds.size.x == GetComponent<CircleCollider2D>().bounds.size.x)
                {
                    SetFinishedEating();
                    return;
                }

                //Lerp towards target center
                transform.position = Vector3.Lerp(transform.position, obj.transform.position, Time.deltaTime * eatSpeed);

                //Look at
                Vector3 dir = obj.transform.position - transform.position;
                float angle = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg;
                transform.rotation = Quaternion.AngleAxis(angle - 90, Vector3.forward);

                //Check if eaten
                if (MathFunctions.IsOverlapping(gameObject.GetComponent<CircleCollider2D>(), obj.GetComponent<CircleCollider2D>(), 0.2f))
                {
                    //Is overlapping
                    transform.position = Vector3.MoveTowards(transform.position, obj.transform.position, Time.deltaTime * eatSpeed);

                    if (MathFunctions.IsOverlapping(gameObject.GetComponent<CircleCollider2D>(), obj.GetComponent<CircleCollider2D>(), 0))
                    {
                        if (obj.tag.Contains("Poison"))
                        {
                            //Destroy ourselves
                            DestroyObj(gameObject);
                        }
                        else
                        {
                            //We have eaten
                            Grow();

                            //Play pop audio clip (randomise pitch)
                            audioSource.pitch = Random.Range(0.5f, 1.5f);
                            audioSource.Play();

                            //Destroy the obj
                            DestroyObj(obj);

                            //Finish eating
                            SetFinishedEating();

                            
                        }
                    }
                }
            }
            else
                SetFinishedEating();

            if(growing)
            {
                transform.localScale = Vector3.Lerp(transform.localScale, new Vector3(growSize, growSize, 1f), Time.deltaTime * growSpeed);
            }
        }
    } */

    private void Grow()
    {
        StartCoroutine("GrowTimed");
    }

    private IEnumerator GrowTimed()
    {
        growing = true;
        growSize = transform.localScale.x + obj.transform.localScale.x / 2;
        yield return new WaitForSeconds(1f);
        growing = false;
        enemyController.ScaleAudioSourceDistances();
        enemyController.ScaleParticleSystemTrailLength();
    }

    private IEnumerator FreezeTimed()
    {
        float origMoveSpeed = wander.GetMoveSpeed();
        wander.SetMoveSpeed(0f);
        yield return new WaitForSeconds(3f);
        wander.SetMoveSpeed(origMoveSpeed);
    }

    private void DestroyObj(GameObject objToDestroy)
    {
        //If the objToDestroy has a trail
        if (objToDestroy.transform.Find("Trail") != null)
        {
            //Unparent child trail so that it lingers after death
            GameObject trail = objToDestroy.transform.Find("Trail").gameObject;
            trail.transform.SetParent(null, false);
            //trail.transform.localScale = new Vector3(1f, 1f, 1f);

            Destroy(trail, 1f);
        }

        if(obj.tag == "Player")
        {
            //EventManager.OnPlayerKilled();
        }

        //Destroy parent (this will no longer destroy trail)
        Destroy(objToDestroy);
    }
	

    private void SetFinishedEating()
    {
        eating = false;
        enemyController.SetState("wandering");
        GetComponent<Renderer>().sortingOrder = origSortingOrder;
    }

    private void OnTriggerStay2D(Collider2D col)
    {
        if(col.tag.Contains("Enemy") || col.CompareTag("Player") && obj == null)
        {
            // If we have collided with poison but we are a purple (Dropper), then ignore
            if(col.tag.Contains("Poison") && transform.CompareTag("Enemy_Purple"))
            {
                return;
            }

            if (!eating)
            {
                if (col.gameObject.GetComponent<CircleCollider2D>().bounds.size.x > GetComponent<CircleCollider2D>().bounds.size.x)
                {
                    //Other obj is bigger
                    return;
                }

                obj = col.gameObject;
                eating = true;
                enemyController.SetState("eating");
                PushThisSpriteAboveOther(col);               
            }
        }
    }

    private void OnTriggerExit2D(Collider2D col)
    {
        //If we have prey, we want to check if it is our prey that has left our trigger
        if(obj != null)
        {
            if(col.gameObject.name.Equals(obj.name))
            {
                //We have lost the prey we were chasing
                SetFinishedEating();
            }
        }
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
