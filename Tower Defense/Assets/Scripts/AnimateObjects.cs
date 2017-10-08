using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimateObjects : MonoBehaviour
{
    public GameObject[] spawnObjects;
    public Transform spawnLocation;
    public Transform spawnLocation2;
    public Transform spawnContainer;

    public static List<GameObject> moveables;
    public static List<GameObject> moveables2;
    private float waitTime;
    private float currTimer;
    private bool gotTheTimer = false;
    private float waitTime2;
    private float currTimer2;
    private bool gotTheTimer2 = false;

    private void Awake()
    {
        moveables = new List<GameObject>();
        moveables2 = new List<GameObject>();
    }

    private void Update()
    {
        //ManageObjectSpawnTimer();
        //ManageObjectSpawnTimer2();

        //MoveTheMoveables();
        //MoveTheMoveables2();
    }

    // Spawn a car at every "waitTime"
    private void ManageObjectSpawnTimer()
    {
        if (gotTheTimer == false)
        {
            waitTime = Random.Range(18, 50);
            waitTime /= 8;
            gotTheTimer = true;

            SpawnObject();
        }
        else
        {
            currTimer += 1 * Time.deltaTime;

            if (currTimer >= waitTime)
            {
                currTimer = 0;
                gotTheTimer = false;
            }
        }
    }

    // Spawn a car at every "waitTime" for opposite cars
    private void ManageObjectSpawnTimer2()
    {
        if (gotTheTimer2 == false)
        {
            waitTime2 = Random.Range(18, 50);
            waitTime2 /= 8;
            gotTheTimer2 = true;

            SpawnObject2();
        }
        else
        {
            currTimer2 += 1 * Time.deltaTime;

            if (currTimer2 >= waitTime2)
            {
                currTimer2 = 0;
                gotTheTimer2 = false;
            }
        }
    }

    // Spawn one of 3 cars
    private void SpawnObject()
    {
        int objNumber = Random.Range(0, 3);

        GameObject spawnedObject = Instantiate(spawnObjects[objNumber], spawnLocation.position + new Vector3(0, 1.28f, 0), spawnObjects[1].transform.rotation, spawnContainer);
        moveables.Add(spawnedObject);
    }

    // Spawn one of 3 cars in oppisite direction
    private void SpawnObject2()
    {
        int objNumber = Random.Range(0, 3);

        GameObject spawnedObject = Instantiate(spawnObjects[objNumber], spawnLocation2.position + new Vector3(0, 1.28f, 0), spawnObjects[1].transform.rotation * Quaternion.Euler(0, 0, 180), spawnContainer);
        moveables2.Add(spawnedObject);
    }

    // Move all the cars
    private void MoveTheMoveables()
    {
        for (int i = 0; i < moveables.Count; i++)
        {
            float speed = Random.Range(16, 22);

            moveables[i].transform.Translate(speed * Vector3.right * Time.deltaTime);
        }     
    }

    // Move all the cars in opposite direction
    private void MoveTheMoveables2()
    {
        for (int i = 0; i < moveables2.Count; i++)
        {
            float speed = Random.Range(14, 18);

            moveables2[i].transform.Translate(speed * Vector3.right * Time.deltaTime);
        }
    }
}
