using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameLevelBehaviour : MonoBehaviour {

    //After a one second delay, shrink the game level, culling any enemies that were present for the initial zoom in
    private bool readyForShrink = false;

	// Use this for initialization
	void Start () {
        StartCoroutine("delay");
	}
	
	// Update is called once per frame
	void Update () {
		if(readyForShrink)
        {
            if (Mathf.Abs(transform.localScale.x - 1f) > 0.1f)
            {
                transform.localScale = Vector3.Lerp(transform.localScale, new Vector3(1f, 1f, 1f), Time.deltaTime * 3f);
            }
            else
                readyForShrink = false;
        }
	}

    private IEnumerator delay()
    {
        yield return new WaitForSeconds(2f);
        readyForShrink = true;
    }
}
