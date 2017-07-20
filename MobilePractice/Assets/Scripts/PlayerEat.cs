using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerEat : MonoBehaviour {

    private bool eating = false;
    private bool growing = false;
    private GameObject obj;
    private int origSortingOrder;
    private float eatSpeed;
    private float growSize;

    public float growSpeed = 3f;
    private float growAmount = 1.5f;

    private ScoreManager scoreManager;

    private AudioSource audioSource;

    private PlayerBehaviours playerController;

    // Use this for initialization
    void Start()
    {
        origSortingOrder = GetComponent<Renderer>().sortingOrder;
        eatSpeed = GetComponent<wander>().GetMoveSpeed() * 3;

        scoreManager = GameObject.FindGameObjectWithTag("ScoreManager").GetComponent<ScoreManager>();
        audioSource = GetComponent<AudioSource>();
        playerController = GetComponent<PlayerBehaviours>();
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
                            //Destroy the obj
                            DestroyObj(obj);

                            //Increment score
                            scoreManager.IncrementScore(10);

                            //Play pop sound
                            audioSource.pitch = Random.Range(0.5f, 1.3f);
                            audioSource.Play();

                            //Finish eating
                            SetFinishedEating();


                        }
                    }
                }
            }
            else
                SetFinishedEating();
        }
    }

    private void DestroyObj(GameObject objToDestroy)
    {
        //If the objToDestroy has a trail
        if (objToDestroy.transform.Find("Trail") != null)
        {
            //Unparent child trail so that it lingers after death
            GameObject trail = objToDestroy.transform.Find("Trail").gameObject;
            trail.transform.SetParent(null, false);

            Destroy(trail, 1f);
        }

        //Destroy parent (this will no longer destroy trail)
        Destroy(objToDestroy);
    }


    private void SetFinishedEating()
    {
        eating = false;
        GetComponent<Renderer>().sortingOrder = origSortingOrder;
    }

    private void OnTriggerStay2D(Collider2D col)
    {
        if (col.tag.Contains("Enemy") && obj == null)
        {
            // If we have collided with poison but we are a purple (Dropper), then ignore
            if (col.tag.Contains("Poison") && transform.CompareTag("Enemy_Purple"))
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
                PushThisSpriteAboveOther(col);
            }
        }
    }

    private void OnTriggerExit2D(Collider2D col)
    {
        //If we have prey, we want to check if it is our prey that has left our trigger
        if (obj != null)
        {
            if (col.gameObject.name.Equals(obj.name))
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
