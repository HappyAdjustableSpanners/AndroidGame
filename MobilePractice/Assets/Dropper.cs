using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Dropper : MonoBehaviour {

    public GameObject obj;

    public float dropFrequency;

    public Vector2 dropSizeBodyRelation;

    // Use this for initialization
    void Start()
    {
        InvokeRepeating("DropObj", dropFrequency, dropFrequency);
    }
	
	// Update is called once per frame
	void Update () {
		
	}

    void DropObj()
    {
        //Choose size
        float size = Random.Range(transform.localScale.x * dropSizeBodyRelation[0], transform.localScale.x * dropSizeBodyRelation[1]);

        float randRotZ = Random.Range(0f, 360f);
        GameObject drop = Instantiate(obj, transform.position, Quaternion.Euler(new Vector3(0f, 0f, randRotZ)));
    }
}
