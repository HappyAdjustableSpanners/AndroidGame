using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class BackToMenu : MonoBehaviour, IPointerDownHandler, IPointerUpHandler  {

    private bool pressed = false;
    private Image img;

	// Use this for initialization
	void Start () {
        img = GetComponent<Image>();
	}
	
	// Update is called once per frame
	void Update () {
        if (pressed)
        {
            img.fillAmount -= 0.03f;
            if (img.fillAmount == 0f)
            {
                EventManager.OnPlayerKilled();
            }
        }
        else
            img.GetComponentInChildren<Image>().fillAmount += 0.03f;
    }

    public void OnPointerDown(PointerEventData data)
    {
        pressed = true;
    }

    public void OnPointerUp(PointerEventData data)
    {
        pressed = false;
    }
}
