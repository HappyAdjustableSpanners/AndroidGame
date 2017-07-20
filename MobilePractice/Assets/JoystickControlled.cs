using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JoystickControlled : MonoBehaviour {

    public VirtualJoystick joystick;
    public float moveSpeed;
    public float lookSpeed;

    private Rigidbody2D rb;
    private Vector2 dir;

	// Use this for initialization
	void Start () {
        rb = GetComponent<Rigidbody2D>();
	}

    // Update is called once per frame
    void FixedUpdate()
    {

        //Get joystick input
        dir.x = joystick.Horizontal();
        dir.y = joystick.Vertical();

        //Set velocity based on input vector and move speed
        rb.velocity = dir * Time.deltaTime * moveSpeed;

        //Look forward
        if (dir != Vector2.zero)
        {
            var angle = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg;
            transform.rotation = Quaternion.Lerp(transform.rotation, Quaternion.AngleAxis(angle - 90, Vector3.forward), Time.deltaTime * lookSpeed);
        }

    }

    public float GetMoveSpeed()
    {
        return moveSpeed;
    }
}
