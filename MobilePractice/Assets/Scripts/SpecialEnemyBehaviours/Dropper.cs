using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Dropper : MonoBehaviour {

    public GameObject obj;
    private GameObject player;

    public float dropFrequency;

    // Use this for initialization
    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");
        InvokeRepeating("DropObj", dropFrequency, dropFrequency);
    }
	
	// Update is called once per frame
	void Update () {
		
	}

    void DropObj()
    {
        //We want to keep the poison obj always ~half the size of the player, no matter how big the dropper is
        if (player != null)
        {
            float playerSize = player.transform.localScale.x;
            float size = Random.Range(playerSize * 0.5f, playerSize * 0.7f);
            float randRotZ = Random.Range(0f, 360f);
            GameObject drop = Instantiate(obj, transform.position, Quaternion.Euler(new Vector3(0f, 0f, randRotZ)));
            drop.transform.localScale = new Vector3(size, size, 1f);
        }
    }
}
