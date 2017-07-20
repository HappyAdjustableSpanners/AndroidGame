using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestroyOnContact : MonoBehaviour {

    void OnTriggerExit2D(Collider2D col)
    {
        if (col.tag.Contains("Enemy"))
        {
            Destroy(col.gameObject);
        }
        else if (col.tag == "Background_Sprite")
        {
            Destroy(col.gameObject);
        }
    }
}
