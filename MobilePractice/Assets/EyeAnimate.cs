using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EyeAnimate : MonoBehaviour {

    private Vector2 point;
    private Vector2[] orig_pos = new Vector2[2];
    public GameObject[] eyes;
    public float moveSpeed;

	// Use this for initialization
	void Start () {
        InvokeRepeating("GetRandPointOnCircle", 0f, 2f);

        for(int i = 0; i < eyes.Length; i++)
        {
            orig_pos[i] = (Vector2)eyes[i].transform.localPosition;
        }
    }
	
	// Update is called once per frame
	void Update () {
        for (int i = 0; i < eyes.Length; i++)
        {
            eyes[i].transform.localPosition = Vector2.Lerp(eyes[i].transform.localPosition, point + orig_pos[i], Time.deltaTime * moveSpeed);
        }
	}

    private void GetRandPointOnCircle()
    {
        for (int i = 0; i < eyes.Length; i++)
        {
            point = Random.insideUnitCircle.normalized * 10f;
        }
    }
}
