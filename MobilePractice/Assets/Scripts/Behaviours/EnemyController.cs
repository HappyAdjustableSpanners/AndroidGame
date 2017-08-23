using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyController : MonoBehaviour {

    public enum State {  wandering, eating }
    public State state = State.wandering;
    private wander wander;

	// Use this for initialization
	void Start () {
        wander = GetComponent<wander>();
    }
	
	// Update is called once per frame
	void Update () {
		
        //Simple state machine. 
        //Ensures that while eating, the enemy does not wander
        if(state == State.wandering)
        {
            wander.SetWandering(true);
        }
        else if(state == State.eating)
        {
            wander.SetWandering(false);
        }
    }

    public void SetState(string stateTemp)
    {
        if(stateTemp.Equals("wandering"))
        {
            state = State.wandering;
        }
        else if(stateTemp.Equals("eating"))
        {
            state = State.eating;
        }
    }
}
