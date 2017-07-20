using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParticleSystemSettings : MonoBehaviour {

    private ParticleSystem ps;
    ParticleSystem.SizeOverLifetimeModule sizeSettings;

    // Use this for initialization
    void Start () {
        ps = GetComponent<ParticleSystem>();

        sizeSettings = ps.sizeOverLifetime;
        sizeSettings.xMultiplier = 2;
    }
	
	// Update is called once per frame
	void Update () {
		
	}

    void OnPlayerChangeSize(float newSize)
    {
        sizeSettings.xMultiplier = newSize * 4;
    }
}
