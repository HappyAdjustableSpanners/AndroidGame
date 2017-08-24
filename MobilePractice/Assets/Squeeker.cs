using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Squeeker : MonoBehaviour {

    //FX
    public GameObject soundWaveSprite;
    private AudioSource audioSource;
    private bool soundPlayed = false;

    //Sounds
    public AudioClip[] sounds;

    //Squeek frequency
    public float squeekFrequency = 10f;

    //Duration of squeek
    public float squeekDuration = 2.5f;

    //Number of waves during duration
    private float numWaves = 3f;
    private float currWaves = 0f;

    //Squeek interval (calculated by diving duration by numWaves)
    private float squeekInterval;
    
    //Timer to count between overall squeeks
    private float timer = 0f;

    //Timer to cound between individual sound waves
    private float intervalTimer = 0f;

    public bool playOnAwake = false;


	// Use this for initialization
	void Start () {

        //Calculate interval
        squeekInterval = squeekDuration / numWaves;

        //Get as
        audioSource = transform.Find("Squeeker").GetComponent<AudioSource>();
	}
	
	// Update is called once per frame
	void Update () {

        //Inc timer in seconds
        timer += Time.deltaTime % 60f;

        //Ready for squeek?
        if (timer > squeekFrequency || playOnAwake)
        {
           
            intervalTimer += Time.deltaTime % 60f;

            if (!soundPlayed)
            {
                //Do squeek sound
                int randomSoundIndex = Random.Range(0, sounds.Length);
                //Load random clip
                audioSource.clip = sounds[randomSoundIndex];
                audioSource.pitch = Random.Range(0.5f, 1.3f);
                audioSource.PlayDelayed(0.5f);
                soundPlayed = true;
            }

            if(intervalTimer > squeekInterval)
            {
                //Load wave
                GameObject wave = Instantiate(soundWaveSprite, transform.position, Quaternion.identity);
                wave.transform.SetParent(transform);
                Destroy(wave, 1f);

      
                //Set squeek size to half the size of this
                wave.transform.localScale = transform.localScale / 2;

                //Inc num waves
                currWaves += 1f;

                if (currWaves >= numWaves)
                {
                    //Reset overall timer
                    timer = 0f;
                    currWaves = 0f;
                    soundPlayed = false;

                    if (playOnAwake)
                    {
                        playOnAwake = false;
                    }
                }

                //Reset interval timer
                intervalTimer = 0f;
            }
        }
		
	}
}
