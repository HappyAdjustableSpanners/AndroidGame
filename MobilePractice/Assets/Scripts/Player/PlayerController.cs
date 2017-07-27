using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour {

    PlayerMoveJoystick playerMove;
    PlayerEat playerEat;

    public enum State {  moving, eating };
    public State state;

    // Use this for initialization
    void Start () {
        playerMove = GetComponent<PlayerMoveJoystick>();
        playerEat = GetComponent<PlayerEat>();
    }
	
	// Update is called once per frame
	void Update () {
        //if (state == State.eating)
        //{
        //    playerMove.SetAllowMovement(false);
        //}
        //else
        //    playerMove.SetAllowMovement(true);
	}

    public void setState(string value)
    {
        if(value.Equals("eating"))
        {
            state = State.eating;
        }
        else if(value.Equals("moving"))
        {
            state = State.moving;
        }
    }
}
