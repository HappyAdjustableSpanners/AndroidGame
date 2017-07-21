using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerBehaviours : MonoBehaviour {

    //Components
    private AudioSource audioSource;
    private ParticleSystem ps;

    //Growing
    private float newSize;
    private bool growing = false;
    private float growSpeed = 3f;

    // Use this for initialization
    void Start () {
        audioSource = GetComponent<AudioSource>();
        ps = GetComponentInChildren<ParticleSystem>();
        ScaleParticleSystemTrailLength(transform.localScale.x);
    }
	
	// Update is called once per frame
	void Update () {
        //If growing, lerp to new size
		if(growing)
        {
            transform.localScale = Vector3.Lerp(transform.localScale, new Vector3(newSize, newSize, 1f), Time.deltaTime * growSpeed);
        }
	}

    public void Grow()
    {
        //Calculate a new size, set whatever GO that need to scale with player to this size as well
        newSize = transform.localScale.x * 2f;

        //Scale elements
        GameObject.FindGameObjectWithTag("GameLevel").transform.localScale = new Vector3(newSize * 2, newSize * 2, 1f);
        ScaleAudioSourceDistances();
        ScaleParticleSystemTrailLength(newSize);

        //Set growing to true
        growing = true;
    }

    public void ScaleParticleSystemTrailLength(float size)
    {
        //Scale each axis of the particle system 3D size to the provided size
        var mainModule = ps.main;
        mainModule.startSizeXMultiplier = size;
        mainModule.startSizeYMultiplier = size;
        mainModule.startSizeZMultiplier = size;                                                                                                                                                                                                                                                                                                                                                                                
    }

    public void ScaleAudioSourceDistances()
    {
        //Scale audio source
        audioSource.minDistance = transform.localScale.x * 2f;
        audioSource.maxDistance = transform.localScale.x * 5f;
    }


}
