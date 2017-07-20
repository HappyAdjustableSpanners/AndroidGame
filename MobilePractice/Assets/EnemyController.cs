using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyController : MonoBehaviour {

    public enum State {  wandering, eating }
    public State state = State.wandering;
    public AudioSource audioSource;
    public bool backgroundEnemy = false;
    private ParticleSystem ps;

    private wander wander;

	// Use this for initialization
	void Start () {
        wander = GetComponent<wander>();
        ps = GetComponentInChildren<ParticleSystem>();
        if (!backgroundEnemy)
        {
            audioSource = GetComponent<AudioSource>();
            ScaleAudioSourceDistances();
            ScaleParticleSystemTrailLength();
        }
    }
	
	// Update is called once per frame
	void Update () {
		
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

    public void ScaleAudioSourceDistances()
    {
        audioSource.minDistance = transform.localScale.x * 2f;
        audioSource.maxDistance = transform.localScale.x * 5f;
    }

    public void ScaleParticleSystemTrailLength()
    {
        var mainModule = ps.main;
        mainModule.startSizeXMultiplier = transform.localScale.x;
        mainModule.startSizeYMultiplier = transform.localScale.y;
        mainModule.startSizeZMultiplier = transform.localScale.z;
        ps.startLifetime = transform.localScale.x + 1;
    }
}
