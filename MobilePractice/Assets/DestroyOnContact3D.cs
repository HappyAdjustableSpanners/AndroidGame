using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestroyOnContact3D : MonoBehaviour
{

    void OnTriggerExit2D(Collider2D col)
    {
        if(col.CompareTag("Background_Sprite"))
        {
            Destroy(col.gameObject);
        }
    }
}
