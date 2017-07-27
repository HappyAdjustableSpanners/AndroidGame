using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HidePlayerOnCollision : MonoBehaviour {

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    void OnTriggerStay2D(Collider2D col)
    {
        if(col.tag.Contains("Enemy"))
        {
            //If an enemy collides with this
            //Disable collider for 1 second
            //Tell it to finish eating
            
        }
    }
}
