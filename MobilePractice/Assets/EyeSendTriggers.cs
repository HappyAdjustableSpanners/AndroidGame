using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EyeSendTriggers : MonoBehaviour {

    public EyeMoveBehaviour leftEye;
    public EyeMoveBehaviour rightEye;

    // Use this for initialization
    void Start () {
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    void OnTriggerEnter2D(Collider2D col)
    {
        leftEye.OnTrigEnter2D(col);
        rightEye.OnTrigEnter2D(col);
    }

    void OnTriggerExit2D(Collider2D col)
    {
        leftEye.OnTrigExit2D(col);
        rightEye.OnTrigExit2D(col);
    }
}
