using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Follow : MonoBehaviour {

    public GameObject obj;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
        if (obj != null)
        {
            transform.position = new Vector3(obj.transform.position.x, obj.transform.position.y, transform.position.z);
        }
	}
}
