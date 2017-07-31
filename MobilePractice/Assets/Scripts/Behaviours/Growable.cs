﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Growable : MonoBehaviour {

    //Parameters
    public float growSpeed = 1f;
    
    //State
    private bool growing = false;

    //Player reference
    private GameObject player;

    //Components
    private ParticleSystem ps;
    private AudioSource audioSource;

    //Global vars
    private float newSize;

    // Use this for initialization
    void Start () {

        //Get ps
        ps = GetComponentInChildren<ParticleSystem>();

        //Get player ref
        player = GameObject.FindGameObjectWithTag("Player");

        //Get components
        audioSource = GetComponent<AudioSource>();

        //Initially scale elements
        ScaleAudioSourceDistances(transform.localScale.x * 3);
        ScaleParticleSystemTrailLength(transform.localScale.x * 5);
        
    }
	
	// Update is called once per frame
	void Update () {

        //If we are growing and we have a reference to the player (we need this to scale ourselves from)
        if (growing && player != null)
        {
            //We don't want enemies growing stupidly in comparison with the player, so we put a threshold of 5 times the size of the player
            if (transform.localScale.x < player.transform.localScale.x * 5f)
            {
                //Lerp to new size
                transform.localScale = Vector3.Lerp(transform.localScale, new Vector3(newSize, newSize, 1f), Time.deltaTime * growSpeed);
            }
        }
    }

    public void Grow(float newSizeTemp)
    {
        //Set our newSize global var
        newSize = newSizeTemp;

        //Start our grow co-routine (we want this to run in parallel so we can still move etc while growing)
        StartCoroutine("GrowTimed");
    }

    private IEnumerator GrowTimed()
    {
        //Set growing to true, wait 1s, then set growing to false
        growing = true;
        yield return new WaitForSeconds(1f);
        growing = false;
        ScaleAudioSourceDistances(transform.localScale.x * 3);
        ScaleParticleSystemTrailLength(transform.localScale.x * 5);
    }

    public void ScaleAudioSourceDistances(float size)
    {
        audioSource.minDistance = size * 2f;
        audioSource.maxDistance = size * 15f;
    }

    public void ScaleParticleSystemTrailLength(float size)
    {
        var mainModule = ps.main;
        mainModule.startSizeXMultiplier = size;
        mainModule.startSizeYMultiplier = size;
        mainModule.startSizeZMultiplier = size;
    }
}
