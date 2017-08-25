using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{

    //The current stage
    public int currentStage = 1;

    //Spawner GO
    private Spawner spawner;

    //The next sprite to be on top of the progress slider
    public Image nextEnemySliderImg;

    //Reference to player
    private GameObject player;
    
    void Awake()
    {
        //Get reference to spawner GO
        spawner = GameObject.FindGameObjectWithTag("Spawner").GetComponent<Spawner>();

        //Set the initial objects to spawn (need this in awake so that initial enemies can be spawned)
        SetObjToSpawn();
    }

    // Use this for initialization
    void Start()
    {
        // Get reference to player
        player = GameObject.FindGameObjectWithTag("Player");  

        //Add listener for playerkilled event
        EventManager.playerKilledMethods += OnPlayerKilled;
    }

    public void IncrementStage()
    {
        //Increment current stage
        currentStage += 1;

        //Set the enemies to spawn this stage
        SetObjToSpawn();

        //Make player bigger
        float newSize = player.transform.localScale.x * 2;
        player.GetComponent<PlayerGrow>().Grow(newSize);

        //Zoom camera out
        GameObject.FindGameObjectWithTag("MainCamera").GetComponent<CameraBehaviour>().Zoom(player.transform.localScale.x * 100f);

        //Trigger stageUpEvent
        EventManager.OnStageUp(currentStage);
    }

    private void SetObjToSpawn()
    {
        //Set the enemies to spawn based on the current stage
        GameObject[] objToSpawn = null;
        if (currentStage == 1)
        {
            nextEnemySliderImg.sprite = Resources.Load<Sprite>("Sprites/enemy_sprite_yellow");
            objToSpawn = new GameObject[1];
            objToSpawn[0] = Resources.Load<GameObject>("Prefabs/ForegroundEnemies/Enemy_Red");
        }
        else if (currentStage == 2)
        {
            nextEnemySliderImg.sprite = Resources.Load<Sprite>("Sprites/enemy_sprite_green");
            objToSpawn = new GameObject[2];
            objToSpawn[0] = Resources.Load<GameObject>("Prefabs/ForegroundEnemies/Enemy_Red");
            objToSpawn[1] = Resources.Load<GameObject>("Prefabs/ForegroundEnemies/Enemy_Yellow");
        }
        else if (currentStage == 3)
        {
            objToSpawn = new GameObject[3];
            nextEnemySliderImg.sprite = Resources.Load<Sprite>("Sprites/enemy_sprite_purple");
            objToSpawn[0] = Resources.Load<GameObject>("Prefabs/ForegroundEnemies/Enemy_Red");
            objToSpawn[1] = Resources.Load<GameObject>("Prefabs/ForegroundEnemies/Enemy_Yellow");
            objToSpawn[2] = Resources.Load<GameObject>("Prefabs/ForegroundEnemies/Enemy_Purple");
        }
        else if (currentStage == 4)
        {
            objToSpawn = new GameObject[4];
            objToSpawn[0] = Resources.Load<GameObject>("Prefabs/ForegroundEnemies/Enemy_Red");
            objToSpawn[1] = Resources.Load<GameObject>("Prefabs/ForegroundEnemies/Enemy_Yellow");
            objToSpawn[2] = Resources.Load<GameObject>("Prefabs/ForegroundEnemies/Enemy_Blue");
            objToSpawn[3] = Resources.Load<GameObject>("Prefabs/ForegroundEnemies/Enemy_Purple");
        }

        //Finally, send the spawner the objects to spawn
        if (objToSpawn != null)
        {
            spawner.setObjsToSpawn(objToSpawn);
        }
    }

    //Go back to menu when player killed
    private void OnPlayerKilled()
    {
        SceneManager.LoadScene("menu");
        EventManager.ClearAll();
    }
}
