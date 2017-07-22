using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMoveJoystick : MonoBehaviour {

    public VirtualJoystick joystick;
    public float moveSpeed = 40f;
    public float lookSpeed;

    private Rigidbody2D rb;
    private Vector2 dir;
    private bool allowMovement = true;

    public CircleCollider2D map;
    private CameraBehaviour cameraBehaviour;

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

            if(cameraBehaviour.getInitialZoomDone())
            {
                //We want the movement speed to stay the same (relatively) as the player increases in size
                moveSpeed = (map.bounds.extents.x - -map.bounds.extents.x) * 2;
            }

            //Set velocity based on input vector and move speed
            rb.velocity = dir * Time.deltaTime * moveSpeed;

            //Look forward
            if (dir != Vector2.zero)
            {
                var angle = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg;
                transform.rotation = Quaternion.Lerp(transform.rotation, Quaternion.AngleAxis(angle - 90, Vector3.forward), Time.deltaTime * lookSpeed);
            }
        }
    }

    public void SetAllowMovement(bool value)
    {
        allowMovement = value;
    }

    public float GetMoveSpeed()
    {
        return moveSpeed;
    }

    public void SetMoveSpeed(float value)
    {
        moveSpeed = value;
    }
}
