using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMoveJoystick : MonoBehaviour {

    public VirtualJoystick joystick;
    public float moveSpeed = 40f;
    public float lookSpeed;
    private float speedFactor = 5f;

    private float moveFactor = 1f;
    private bool initialZoomDone = false;

    private Rigidbody2D rb;
    private Vector2 dir;
    private bool allowMovement = true;

    public BoxCollider2D map;
    private CameraBehaviour cameraBehaviour;
    private float boostMultiplier = 1.5f;

    private bool boosting = false;

	// Use this for initialization
	void Start () {
        rb = GetComponent<Rigidbody2D>();
        cameraBehaviour = GameObject.FindGameObjectWithTag("MainCamera").GetComponent<CameraBehaviour>();
	}

    // Update is called once per frame
    void FixedUpdate()
    {
        if (allowMovement)
        {
            //Get joystick input
            dir.x = joystick.Horizontal();
            dir.y = joystick.Vertical();

            //As long as we are touching the joystick
            if (dir != Vector2.zero)
            {
                //If we are not boosting
                if (!boosting)
                {
                    //We want the movement speed to stay the same (relatively) as the player increases in size
                    moveSpeed = (map.bounds.size.x * speedFactor);
                    //Set velocity based on input vector and move speed
                    rb.velocity = dir * Time.deltaTime * moveSpeed;
                }
                else
                {
                    //Enemies spawn and scale their movement speed to the player's.
                    //When the player is boosting, we don't want the enemies to spawn with that boost speed
                    //So we keep moveSpeed the same, and just change the rigidbody velocity
                    rb.velocity = dir * Time.deltaTime * (moveSpeed * boostMultiplier);
                }
            }

            //Look forward
            if (dir != Vector2.zero)
            {
                var angle = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg;
                transform.rotation = Quaternion.Lerp(transform.rotation, Quaternion.AngleAxis(angle - 90, Vector3.forward), Time.deltaTime * lookSpeed);
            }
        }
    }

    //Gets and sets
    public void SetAllowMovement(bool value)
    {
        allowMovement = value;
        rb.velocity = Vector2.zero;
    }

    public float GetMoveSpeed()
    {
        return moveSpeed;
    }

    public void SetMoveSpeed(float value)
    {
        moveSpeed = value;
    }

    public void SetBoosting(bool value)
    {
        boosting = value;
    }
}
