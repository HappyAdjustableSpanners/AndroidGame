using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraBehaviour : MonoBehaviour {

    private GameObject player;

    public float cameraFollowSpeed;

    public Camera mainCamera;
    private float orthoSize;
    public float cameraZoomSpeed = 5f;

    private bool initialZoomFinished, initialZoomStarted = false;

	// Use this for initialization
	void Start () {
        player = GameObject.FindGameObjectWithTag("Player");
        mainCamera = GetComponent<Camera>();
        orthoSize = mainCamera.orthographicSize;
    }
	
	// Update is called once per frame
	void FixedUpdate () {

        if(!initialZoomStarted)
        {
            Zoom(6f);
            initialZoomStarted = true;
        }
        FollowPlayer();
	}

    private void FollowPlayer()
    {
        if (player != null)
        {
            transform.position = Vector3.Lerp(transform.position, new Vector3(player.transform.position.x, player.transform.position.y, transform.position.z), Time.smoothDeltaTime * cameraFollowSpeed);
        }

        if(Mathf.Abs(mainCamera.orthographicSize - orthoSize) > 0.1f)
        {
            GetComponent<Camera>().orthographicSize = Mathf.Lerp(GetComponent<Camera>().orthographicSize, orthoSize, Time.deltaTime * cameraZoomSpeed);
        }
        else if(!initialZoomFinished)
        {
            EventManager.OnFinishedInitialZoom();
            initialZoomFinished = true;

        }
    }

    public void Zoom(float size)
    {
        orthoSize = size;

        EventManager.OnOrthoSizeChanged(orthoSize);
    }

    public bool getInitialZoomDone()
    {
        return initialZoomFinished;
    }
}
