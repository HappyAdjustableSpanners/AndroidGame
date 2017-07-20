using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class wander : MonoBehaviour {

    /* Move forward and turn on interval */

    //Turning and moving
    public float turnInterval = 5f;
    public float turnSpeed = 1f;
    public float moveSpeed = 0f;

    //Current target rotation
    private float targetRot;

    public bool wandering = false;

    private Rigidbody2D rb;

    // Use this for initialization
    void Start () {
        InvokeRepeating("ChooseNewRotation", turnInterval, turnInterval);
        rb = GetComponent<Rigidbody2D>();
    }

	// Update is called once per frame
	void FixedUpdate () {

        //If we are wandering, move and turn
        if (wandering)
        {
            Move();
            Turn();
        }
    }

    void Move()
    {
        //Move forward
        rb.velocity = (Vector2)transform.up * Time.deltaTime * moveSpeed;   
    }

    void Turn()
    {   
        //Lerp towards target rotation
        float step = Mathf.Lerp(rb.rotation, targetRot, Time.deltaTime * turnSpeed);
        rb.MoveRotation(step);
    }

    private void ChooseNewRotation()
    {
        //Pick a angle within 90 degrees of the current z axis 
        targetRot = Random.Range(transform.rotation.eulerAngles.z - 45f, transform.rotation.eulerAngles.z + 45f);
    }

    public void SetWandering(bool value)
    {
        wandering = value;
    }

    public void SetMoveSpeed(float value)
    {
        moveSpeed = value;
    }

    public float GetMoveSpeed()
    {
        return moveSpeed;
    }

    public float GetTurnSpeed()
    {
        return turnSpeed;
    }
}
