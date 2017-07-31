using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShinkAndGrowable : MonoBehaviour {

    public float sizeChangeFrequency = 7f;
    public float shrinkSpeed, growSpeed;
    public float growFactor = 2f;
    private bool shrinking = false;
    private bool growing = true;
    private Vector3 origSize;

	// Use this for initialization
	void Start () {
        InvokeRepeating("ShrinkAndGrow", sizeChangeFrequency, sizeChangeFrequency);
        origSize = transform.localScale;
    }
	
	// Update is called once per frame
	void Update () {
        if (shrinking)
        {
            transform.localScale = Vector3.Lerp(transform.localScale, origSize / growFactor, Time.deltaTime * shrinkSpeed);
        }
        else if(growing)
        {
            transform.localScale = Vector3.Lerp(transform.localScale, origSize * growFactor, Time.deltaTime * shrinkSpeed);
        }
    }
    void ShrinkAndGrow()
    {
        origSize = transform.localScale;
        if(shrinking)
        {
            shrinking = false;
            growing = true;
        }
        else if(growing)
        {
            shrinking = true;
            growing = false;
        }
    }
}
