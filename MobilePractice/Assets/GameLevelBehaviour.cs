using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameLevelBehaviour : MonoBehaviour {

    //After a one second delay, shrink the game level, culling any enemies that were present for the initial zoom in
    private bool readyForShrink = false;

    //Change color of the background plane
    private MeshRenderer bgPlaneSR;
    private Color nextColor = Color.white;
    private float colorChangeDuration = 5f;
    private float colorChangeTimer = 0f;
    private bool changeColor = false;

    public Color color1;
    public Color color2;
    public Color color3;
    public Color color4;
    public Color color5;

    // Use this for initialization
    void Start () {
        StartCoroutine("delay");
        bgPlaneSR = transform.Find("BgPlane").GetComponent<MeshRenderer>();
        EventManager.stageUpMethods += OnStageUp;
        EventManager.orthoSizeChangedMethods += OnOrthoSizeChanged;
	}
	
	// Update is called once per frame
	void Update () {
		if(readyForShrink)
        {
            if (Mathf.Abs(transform.localScale.x - 1f) > 0.1f)
            {
                transform.localScale = Vector3.Lerp(transform.localScale, new Vector3(1f, 1f, 1f), Time.deltaTime * 3f);
            }
            else
                readyForShrink = false;
        }

        if(changeColor)
        {
            colorChangeTimer += Time.deltaTime;

            if (colorChangeTimer <= colorChangeDuration)
            {
                bgPlaneSR.material.color = Color.Lerp(bgPlaneSR.material.color, nextColor, Time.deltaTime);
            }
            else
            {
                colorChangeTimer = 0f;
                changeColor = false;
            }
        }
	}

    private IEnumerator delay()
    {
        yield return new WaitForSeconds(3f);
        readyForShrink = true;
    }

    private void OnStageUp(int currentStage)
    {
        changeColor = true;
        if (currentStage == 2 )
        {
            nextColor = color1;
        }
        else if(currentStage == 3)
        {
            nextColor = color2;
        }
        else if(currentStage == 4)
        {
            nextColor = color3;
        }
        else if(currentStage == 5)
        {
            nextColor = color4;
        }
    }

    private void OnOrthoSizeChanged(float orthoSize)
    {
        //Change plane size
        float height = (orthoSize * 2f);
        float width = (height * Screen.width / Screen.height);
        transform.Find("BgPlane").transform.localScale = new Vector3(width / 7, 1f, height / 7);

        //Change spawner rectangle size
        transform.Find("Spawner").transform.localScale = new Vector3(width * 1.3f, height * 1.3f, 1f);
        transform.Find("DeathLimit").transform.localScale = new Vector3(width, height, 1f) * 1.4f;
    }
}
