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

	// Use this for initialization
	void Start () {
        rb = GetComponent<Rigidbody2D>();
        cameraBehaviour = GameObject.FindGameObjectWithTag("MainCamera").GetComponent<CameraBehaviour>();

        EventManager.finishedInitialZoomMethods += OnInitialZoomFinished;
	}

    // Update is called once per frame
    void FixedUpdate()
    {
        if (allowMovement)
        {
            //Get joystick input
            dir.x = joystick.Horizontal();
            dir.y = joystick.Vertical();

            if(initialZoomDone)
            {
                //We want the movement speed to stay the same (relatively) as the player increases in size
                moveSpeed = (map.bounds.size.x * speedFactor) / moveFactor;
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
        rb.velocity = Vector2.zero;
    }

    public float GetMoveSpeed()
    {
        return moveSpeed;
    }

    private void OnInitialZoomFinished()
    {
        initialZoomDone = true;
        if (map.bounds.extents.x > 7f)
        {
            moveFactor = map.bounds.extents.x / 7f;
        }
    }
}
