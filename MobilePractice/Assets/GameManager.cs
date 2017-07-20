using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{

    public int currentStage = 1;
    private Spawner spawner;
    public Image nextEnemySliderImg;
    

    // Use this for initialization
    void Start()
    {
        spawner = GameObject.FindGameObjectWithTag("Spawner").GetComponent<Spawner>();
        SetObjToSpawn();

        EventManager.playerKilledMethods += OnPlayerKilled;
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void IncrementStage()
    {
        //Increment current stage
        currentStage += 1;

        //Set the enemies to spawn this stage
        SetObjToSpawn();

        //Make player bigger
        GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerBehaviours>().Grow();

        //Zoom camera out
        GameObject.FindGameObjectWithTag("MainCamera").GetComponent<CameraBehaviour>().Zoom();
    }

    private void SetObjToSpawn()
    {
        GameObject[] objToSpawn = null;
        if (currentStage == 1)
        {
            nextEnemySliderImg.sprite = Resources.Load<Sprite>("Sprites/enemy_sprite_yellow");
            objToSpawn = new GameObject[1];
            objToSpawn[0] = Resources.Load<GameObject>("Prefabs/Enemy_Red");
        }
        else if (currentStage == 2)
        {
            nextEnemySliderImg.sprite = Resources.Load<Sprite>("Sprites/enemy_sprite_green");
            objToSpawn = new GameObject[2];
            objToSpawn[0] = Resources.Load<GameObject>("Prefabs/Enemy_Red");
            objToSpawn[1] = Resources.Load<GameObject>("Prefabs/Enemy_Yellow");
        }
        else if (currentStage == 3)
        {
            objToSpawn = new GameObject[3];
            nextEnemySliderImg.sprite = Resources.Load<Sprite>("Sprites/enemy_sprite_purple");
            objToSpawn[0] = Resources.Load<GameObject>("Prefabs/Enemy_Red");
            objToSpawn[1] = Resources.Load<GameObject>("Prefabs/Enemy_Yellow");
            objToSpawn[2] = Resources.Load<GameObject>("Prefabs/Enemy_Green");
        }
        else if (currentStage == 4)
        {
            objToSpawn = new GameObject[4];
            objToSpawn[0] = Resources.Load<GameObject>("Prefabs/Enemy_Red");
            objToSpawn[1] = Resources.Load<GameObject>("Prefabs/Enemy_Yellow");
            objToSpawn[2] = Resources.Load<GameObject>("Prefabs/Enemy_Green");
            objToSpawn[3] = Resources.Load<GameObject>("Prefabs/Enemy_Purple");
        }

        if (objToSpawn != null)
        {
            spawner.setObjsToSpawn(objToSpawn);
        }
    }

    private void OnPlayerKilled()
    {
        SceneManager.LoadScene("menu");
    }
}
