using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestroyOnContact : MonoBehaviour {

    public string[] GOTagsToDestroy; 

    void OnTriggerExit2D(Collider2D col)
    {
        foreach(string s in GOTagsToDestroy)
        {
            if (col.CompareTag(s) || col.tag.Contains(s))
            {
                Destroy(col.gameObject);
            }
        }
    }
}
