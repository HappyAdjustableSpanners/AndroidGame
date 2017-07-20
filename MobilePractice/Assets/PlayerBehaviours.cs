using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerBehaviours : MonoBehaviour {

    private AudioSource audioSource;
    private bool growing = false;
    private float newSize;
    private float growSpeed = 3f;
    private ParticleSystem ps;

    // Use this for initialization
    void Start () {
        audioSource = GetComponent<AudioSource>();
        ps = GetComponentInChildren<ParticleSystem>();
        ScaleParticleSystemTrailLength(transform.localScale.x);
    }
	
	// Update is called once per frame
	void Update () {
		if(growing)
        {
            transform.localScale = Vector3.Lerp(transform.localScale, new Vector3(newSize, newSize, 1f), Time.deltaTime * growSpeed);
        }
	}

    public void Grow()
    {
        newSize = transform.localScale.x * 2f;
        GameObject.FindGameObjectWithTag("GameLevel").transform.localScale = new Vector3(newSize * 2, newSize * 2, 1f);
        growing = true;
        audioSource.minDistance = transform.localScale.x * 2f;
        audioSource.maxDistance = transform.localScale.x * 5f;

        ScaleParticleSystemTrailLength(newSize);
    }

    public void ScaleParticleSystemTrailLength(float size)
    {
        var mainModule = ps.main;
        mainModule.startSizeXMultiplier = size;
        mainModule.startSizeYMultiplier = size;
        mainModule.startSizeZMultiplier = size;                                                                                                                                                                                                                                                                                                                                                                                
    }
}
