using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spawner : MonoBehaviour {

    //Obj to spawn
    private GameObject[] objToSpawn = null;
    private GameObject[] bgObjToSpawn = new GameObject[4];

    public float spawnInterval;
    public float backgroundSpawnInterval;

    //Player
    private GameObject player;

    public bool spawnEnemies = false;
    public bool spawnBackgroundEnemies = true;

    int spawnID = 0;

    //Spawn circle
    private CircleCollider2D spawnCircle;


	// Use this for initialization
	void Start () {

        bgObjToSpawn[0] = Resources.Load<GameObject>("Prefabs/BackgroundEnemies/Enemy_Red");
        bgObjToSpawn[1] = Resources.Load<GameObject>("Prefabs/BackgroundEnemies/Enemy_Yellow");
        bgObjToSpawn[2] = Resources.Load<GameObject>("Prefabs/BackgroundEnemies/Enemy_Green");
        bgObjToSpawn[3] = Resources.Load<GameObject>("Prefabs/BackgroundEnemies/Enemy_Purple");

        InvokeRepeating("Spawn", spawnInterval, spawnInterval);
        InvokeRepeating("SpawnBackground", backgroundSpawnInterval, backgroundSpawnInterval);

        player = GameObject.FindGameObjectWithTag("Player");

        spawnCircle = GetComponent<CircleCollider2D>();
	}
	
    private void Spawn()
    {
        if (spawnEnemies)
        {
            if (player != null && objToSpawn != null)
            {
                //Choose random move speed from half the player's speed to 3 times the player speed
                float playerMoveSpeed = player.GetComponent<JoystickControlled>().GetMoveSpeed();
                float minMoveSpeed = playerMoveSpeed * 0.2f;
                float maxMoveSpeed = playerMoveSpeed * 2f;
                float moveSpeed = Random.Range(minMoveSpeed, maxMoveSpeed);

                float minSize = player.transform.localScale.x * 0.6f;
                float maxSize = player.transform.localScale.x * 2f;
                float size = Random.Range(minSize, maxSize);

                spawnCircle.radius = player.transform.localScale.x * 20f;

                //Make sure they don't spawn on top of each other 
                Vector2 spawnPos = new Vector2(player.transform.position.x, player.transform.position.y) + Random.insideUnitCircle.normalized * spawnCircle.radius;
                Collider2D[] hitColliders = Physics2D.OverlapCircleAll(spawnPos, size);

                foreach (Collider2D col in hitColliders)
                {
                    if (col.tag.Contains("Enemy"))
                    {
                        return;
                    }
                }

                GameObject obj = Instantiate(objToSpawn[Random.Range(0, objToSpawn.Length)], spawnPos, Quaternion.identity);
                obj.transform.localScale = new Vector3(size, size, obj.transform.localScale.z);
                obj.GetComponent<wander>().SetMoveSpeed(moveSpeed);

                obj.name = "Enemy" + spawnID;
                spawnID++;
                
            }
        }
    }

    private void SpawnBackground()
    {
        if (spawnBackgroundEnemies)
        {
            if (player != null && GameObject.FindGameObjectsWithTag("Background_Sprite").Length < 10)
            {
                //Choose random attributes within 20% of base
                float moveSpeed = player.GetComponent<JoystickControlled>().GetMoveSpeed() / 20;
                float size = Random.Range(player.transform.localScale.x / 4f, player.transform.localScale.x / 2f);

                spawnCircle.radius = player.transform.localScale.x * 20f;
                Vector2 spawnPos = new Vector2(player.transform.position.x, player.transform.position.y) + Random.insideUnitCircle.normalized * spawnCircle.radius;

                GameObject obj = Instantiate(bgObjToSpawn[Random.Range(0, bgObjToSpawn.Length)], spawnPos, Quaternion.identity);
                obj.transform.localScale = new Vector3(size, size, obj.transform.localScale.z);
                obj.GetComponent<wander>().SetMoveSpeed(moveSpeed);
                obj.GetComponent<SpriteRenderer>().color = new Color(255f, 255f, 255f, Random.Range(0.18f, 0.45f));
            }
        }
    }

    public void setObjsToSpawn(GameObject[] tempObjToSpawn)
    {
        objToSpawn = tempObjToSpawn;
    }
}
