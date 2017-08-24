using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Boostable : MonoBehaviour {

    public float boostFrequency = 10f;
    public float boostMultiplier = 1.7f;
    public float boostDuration = 3f;

	// Use this for initialization
	void Start () {
        InvokeRepeating("Boost", boostFrequency, boostFrequency);
	}
	
    void Boost()
    {
        StartCoroutine("ActivateSpeedBoost");
    }

    IEnumerator ActivateSpeedBoost()
    {
        GetComponent<wander>().SetMoveSpeed(GetComponent<wander>().GetMoveSpeed() * boostMultiplier);
        ParticleSystem.MainModule main = transform.Find("Trail").GetComponent<ParticleSystem>().main;
        main.startColor = Color.green;
        yield return new WaitForSeconds(boostDuration);
        main.startColor = Color.black;
        GetComponent<wander>().SetMoveSpeed(GetComponent<wander>().GetMoveSpeed() / boostMultiplier);
    }
}
