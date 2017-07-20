using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Boostable : MonoBehaviour {

    public float boostFrequency = 10f;
    public float boostMultiplier = 2f;
    public float boostDuration = 3f;

    private float boostTimer = 0f;

	// Use this for initialization
	void Start () {
        InvokeRepeating("Boost", boostFrequency, boostFrequency);
	}
	
	// Update is called once per frame
	void Update () {
	}

    void Boost()
    {
        StartCoroutine("ActivateSpeedBoost");
    }

    IEnumerator ActivateSpeedBoost()
    {
        GetComponent<wander>().SetMoveSpeed(GetComponent<wander>().GetMoveSpeed() * boostMultiplier);

        yield return new WaitForSeconds(boostDuration);

        GetComponent<wander>().SetMoveSpeed(GetComponent<wander>().GetMoveSpeed() / boostMultiplier);
    }
}
