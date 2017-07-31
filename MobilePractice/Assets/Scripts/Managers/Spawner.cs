using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spawner : MonoBehaviour {


    public float spawnInterval, backgroundSpawnInterval, extraSpawnInterval;
    public bool spawnEnemies, spawnBackgroundEnemies, spawnPlankton = true;
    private bool spawnInArea = false;

    //Number of initial enemies (that get culled by zoom-in)
    public int numInitialEnemies;

    //Obj to spawn
    private GameObject[] objToSpawn = null;
    public GameObject[] bgObjToSpawn = new GameObject[4];
    public GameObject[] planktonObjToSpawn = new GameObject[1];

    //Spawn id of obj (unique identifier)
    private int spawnIDForeground, spawnIDBackground, spawnIDExtra = 0;

    //Player
    private GameObject player;

    //Spawn circle
    private CircleCollider2D spawnCircle;
    private BoxCollider2D spawnBox;

    // Use this for initialization
    void Start () {

        //Can turn spawning on/off
        if (spawnEnemies)
        {
            //Spawn foreground and background enemy every interval
            InvokeRepeating("Spawn", spawnInterval, spawnInterval);
        }
        if (spawnBackgroundEnemies)
        {
            InvokeRepeating("SpawnBackground", backgroundSpawnInterval, backgroundSpawnInterval);
        }
        if(spawnPlankton)
        {
            InvokeRepeating("SpawnPlankton", extraSpawnInterval, extraSpawnInterval);
        }

        //Get reference to player
        player = GameObject.FindGameObjectWithTag("Player");

        //Get spawn circle
        spawnCircle = GetComponent<CircleCollider2D>();
        spawnBox = GetComponent<BoxCollider2D>();

        SpawnInitialEnemies();

        if (spawnEnemies == true)
        {
            StartCoroutine("delay");
        }
	}

    private void SpawnInitialEnemies()
    {
        spawnInArea = true;
        for(int i = 0; i < numInitialEnemies; i++)
        {
            Spawn();
            SpawnBackground();

            //Only spawn a third of these
            if (i % 3 == 0)
            {
                SpawnPlankton();
            }
        }
        spawnInArea = false;
    }
	
    private void Spawn()
    {
        //If we have reference to player and have an obj to spawn
        if (player != null && objToSpawn != null)
        {
            //Set the spawn circle radius so we can visually see in the scene view the spawn area
            //spawnCircle.radius = player.transform.localScale.x * 15;

            //Choose random move speed from min and max using player speed as reference. This means as the player scales up things stay consistent
            float playerMoveSpeed = player.GetComponent<PlayerMoveJoystick>().GetMoveSpeed();
            float minMoveSpeed = playerMoveSpeed * 0.2f;
            float maxMoveSpeed = playerMoveSpeed * 2f;
            float moveSpeed = Random.Range(minMoveSpeed, maxMoveSpeed);

            //Choose random scale based on player size
            float minSize = player.transform.localScale.x * 0.6f;
            float maxSize = player.transform.localScale.x * 2f;
            float size = Random.Range(minSize, maxSize);
   
            //We have the option of spawning enemies anywhere in the circle (rather than on the circumferance) 
            //We want to do this for the initial spawning of enemies
            Vector2 spawnPos = Vector2.zero;
            if (!spawnInArea)
            {
                //Get spawn pos on circumferance
                //spawnPos = new Vector2(player.transform.position.x, player.transform.position.y) + Random.insideUnitCircle.normalized * (spawnCircle.radius * transform.parent.localScale.x);
                spawnPos = MathFunctions.FindRandomPointOnRectanglePerimeter(spawnBox);
            }
            else
            {
                //Get spawn point in area of circle
                //spawnPos = Random.insideUnitCircle * (spawnCircle.radius * transform.parent.localScale.x);
                //size *= 5;

                if (spawnPos.magnitude < 25)
                {
                    return;
                }
            }

            //Make sure we spawn them on top of each other by doing an overlap circle
            if (NoOverlap(spawnPos, size, "Enemy"))
            {
                //Instantiate obj, set its scale, speed and name
                GameObject obj = Instantiate(objToSpawn[Random.Range(0, objToSpawn.Length)], spawnPos, Quaternion.Euler(new Vector3(transform.rotation.eulerAngles.x, transform.rotation.eulerAngles.y, Random.Range(0f, 360f))));
                obj.transform.localScale = new Vector3(size, size, obj.transform.localScale.z);
                obj.GetComponent<wander>().SetMoveSpeed(moveSpeed);
                obj.GetComponent<wander>().SetTurnSpeed(0.2f / obj.transform.localScale.x);
                obj.name = "Enemy" + spawnIDForeground;

                //Just to neaten up inspector while running, hide all spawned enemies under the GameManager gameObject
                obj.transform.parent = GameObject.FindGameObjectWithTag("GameManager").transform.Find("ForegroundEnemies").transform;

                //Increment the spawn id
                spawnIDForeground++;
            }                               
        }       
    }

    private void SpawnBackground()
    {
        if (player != null && GameObject.FindGameObjectsWithTag("Background_Sprite").Length < 10 || spawnInArea)
        {
            //Choose random move speed from min and max using player speed as reference. This means as the player scales up things stay consistent
            float playerMoveSpeed = player.GetComponent<PlayerMoveJoystick>().GetMoveSpeed();
            float minMoveSpeed = playerMoveSpeed / 6f;
            float maxMoveSpeed = playerMoveSpeed / 3f;
            float moveSpeed = Random.Range(minMoveSpeed, maxMoveSpeed);

            //Choose random scale based on player size
            float minSize = player.transform.localScale.x * 2f;
            float maxSize = player.transform.localScale.x * 7f;
            float size = Random.Range(minSize, maxSize);

            //We have the option of spawning enemies anywhere in the circle (rather than on the circumferance) 
            //We want to do this for the initial spawning of enemies
            Vector2 spawnPos = Vector2.zero;
            if (!spawnInArea)
            {
                //Get spawn pos on circumferance
                //spawnPos = new Vector2(player.transform.position.x, player.transform.position.y) + Random.insideUnitCircle.normalized * (spawnCircle.radius * transform.parent.localScale.x);
                spawnPos = MathFunctions.FindRandomPointOnRectanglePerimeter(spawnBox);
            }
            else
            {
                //Get spawn point in area of circle
                //spawnPos = Random.insideUnitCircle * (spawnCircle.radius * transform.parent.localScale.x);
                //size *= 5;
            }

            //Make sure we spawn them on top of each other by doing an overlap circle
            if (NoOverlap(spawnPos, size, "Background"))
            {
                //Instantiate obj, set its scale, speed and name
                GameObject obj = Instantiate(bgObjToSpawn[Random.Range(0, bgObjToSpawn.Length)], spawnPos, Quaternion.Euler(new Vector3(transform.rotation.eulerAngles.x, transform.rotation.eulerAngles.y, Random.Range(0f, 360f))));
                obj.transform.localScale = new Vector3(size, size, obj.transform.localScale.z);

                if (obj.GetComponent<wander>())
                {
                    obj.GetComponent<wander>().SetMoveSpeed(moveSpeed);
                    obj.GetComponent<wander>().SetTurnSpeed(0.2f / obj.transform.localScale.x);
                }
                obj.name = "EnemyBg" + spawnIDBackground;

                //Choose a random color for the enemy
                obj.GetComponent<SpriteRenderer>().color = new Color(255f, 255f, 255f, Random.Range(0.18f, 0.45f));

                //Just to neaten up inspector while running, hide all spawned enemies under the GameManager gameObject
                obj.transform.parent = GameObject.FindGameObjectWithTag("GameManager").transform.Find("BackgroundEnemies").transform;

                //Increment the spawn id
                spawnIDBackground++;
            }
        }       
    }

    private void SpawnPlankton()
    {
        if (player != null && GameObject.FindGameObjectsWithTag("Plankton").Length < 10 || spawnInArea)
        {
            int index = Random.Range(0, planktonObjToSpawn.Length);

            float minSize = 1f;
            float maxSize = 1f;
            if(planktonObjToSpawn[index].name == "plankton_cylinder")
            {
                //Choose random scale based on player size
                minSize = player.transform.localScale.x * 5f;
                maxSize = player.transform.localScale.x * 20f;
            }
            else if(planktonObjToSpawn[index].name == "plankton_disc")
            {
                //Choose random scale based on player size
                minSize = player.transform.localScale.x * 10f;
                maxSize = player.transform.localScale.x * 20f;
            }

            //Assign size between min and max
            float size = Random.Range(minSize, maxSize);

            //We have the option of spawning enemies anywhere in the circle (rather than on the circumferance) 
            //We want to do this for the initial spawning of enemies
            Vector2 spawnPos = Vector2.zero;
            if (!spawnInArea)
            {
                //Get spawn pos on circumferance
                //spawnPos = new Vector2(player.transform.position.x, player.transform.position.y) + Random.insideUnitCircle.normalized * (spawnCircle.radius * transform.parent.localScale.x);
                spawnPos = MathFunctions.FindRandomPointOnRectanglePerimeter(spawnBox);
            }
            else
            {
                //Get spawn point in area of circle
                //spawnPos = Random.insideUnitCircle * (spawnCircle.radius * transform.parent.localScale.x);

                if (spawnPos.magnitude < 20)
                {
                    return;
                }
            }

            if (NoOverlap(spawnPos, size, "Plankton"))
            {
                //Instantiate obj, set its scale, speed and name
                GameObject obj = Instantiate(planktonObjToSpawn[index], spawnPos, planktonObjToSpawn[index].transform.rotation);

                //obj.transform.rotation = Quaternion.Euler(new Vector3(180, obj.transform.rotation.eulerAngles.y, Random.Range(0f, 360f)));

                obj.transform.localScale = new Vector3(size, size, size);
                obj.name = "Plankton" + spawnIDExtra;

                //Just to neaten up inspector while running, hide all spawned enemies under the GameManager gameObject
                obj.transform.parent = GameObject.FindGameObjectWithTag("GameManager").transform.Find("Extra").transform;

                //Increment the spawn id
                spawnIDExtra++;
            }
        }
    }

    //Manually set the objects to spawn
    public void setObjsToSpawn(GameObject[] tempObjToSpawn)
    {
        objToSpawn = tempObjToSpawn;
    }


    private bool NoOverlap(Vector2 spawnPos, float size, string tag)
    {
        //Return true if there is no overlap with other enemies, else return false
        Collider2D[] hitColliders = Physics2D.OverlapCircleAll(spawnPos, size);
        foreach (Collider2D col in hitColliders)
        {
            if (col.tag.Contains(tag) || col.tag.Equals(tag))
            {
                return false;
            }
        }
        return true;
    }

    private IEnumerator delay()
    {
        spawnEnemies = false;
        yield return new WaitForSeconds(6f);
        spawnEnemies = true;
    }
}
