﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShinkAndGrowable : MonoBehaviour {

    public float sizeChangeFrequency = 10f;
    public float shrinkSpeed, growSpeed;
    private bool shrinking = true;
    private bool growing = false;
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
            transform.localScale = Vector3.Lerp(transform.localScale, origSize / 2, Time.deltaTime * shrinkSpeed);
        }
        else if(growing)
        {
            transform.localScale = Vector3.Lerp(transform.localScale, origSize * 2, Time.deltaTime * shrinkSpeed);
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