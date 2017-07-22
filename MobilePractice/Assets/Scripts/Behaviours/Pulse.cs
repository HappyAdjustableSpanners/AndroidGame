using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pulse : MonoBehaviour {

    private bool pulseOut = false;
    public float pulseSpeed = 1f;

	// Use this for initialization
	void Start () {
        InvokeRepeating("SwitchPulseDirection", 1, 1);	
	}
	
	// Update is called once per frame
	void Update () {

        if (pulseOut)
        {
            transform.localScale = Vector3.Lerp(transform.localScale, new Vector3(transform.localScale.x * 1.1f, transform.localScale.y * 1.1f, 1f), Time.deltaTime * pulseSpeed);
        }
        else
        {
            transform.localScale = Vector3.Lerp(transform.localScale, new Vector3(transform.localScale.x * 0.9f, transform.localScale.y * 0.9f, 1f), Time.deltaTime * pulseSpeed);
        }
        
	}

    private void SwitchPulseDirection()
    {
        pulseOut = !pulseOut;
    }
}
