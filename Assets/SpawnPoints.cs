using System;
using System.Collections.Generic;
using UnityEngine;
using System.Collections;

public class SpawnPoints : MonoBehaviour
{
    private static SpawnPoints _instance;
    public static SpawnPoints Instance
    {
        get
        {
            if (!_instance)
            {
                _instance = (SpawnPoints)GameObject.FindObjectOfType(typeof(SpawnPoints));
                if (!_instance)
                {
                    GameObject container = new GameObject();
                    container.name = "SpawnPoints";
                    _instance = container.AddComponent(typeof(SpawnPoints)) as SpawnPoints;
                }
            }

            return _instance;
        }
    }


    public Transform BandiTransform;
    public int MaxBandits = 5;
    public float DelayInSpawn = 5;
    private float lastSpawnTime = 0;
    private int actualBandits = 0;

    public List<Transform> SpawnPointsGameObjects;

    // Use this for initialization
    void Start()
    {

        foreach (Transform child in transform)
        {
            //            print("Spawnpoints: " + child.name);
            SpawnPointsGameObjects.Add(child);
        }



    }

    // Update is called once per frame
    void Update()
    {

    }

    void FixedUpdate()
    {
        if (Time.fixedTime - lastSpawnTime > DelayInSpawn && actualBandits < MaxBandits)
        {
            lastSpawnTime = Time.fixedTime;
            SpawnBanditOnSpawnPoint(null);
            actualBandits++;
        }
    }


    void SpawnBanditOnSpawnPoint(int? index)
    {
        var pos = (index == null) ? GetPositionOfRandomSpawnPoint() : SpawnPointsGameObjects[index.Value].position;
        Instantiate(BandiTransform, pos, Quaternion.identity);
    }

    public Vector3 GetPositionOfRandomSpawnPoint()
    {
        var index = UnityEngine.Random.Range(0, SpawnPointsGameObjects.Count - 1);
        return SpawnPointsGameObjects[index].position;
    }


    void OnDrawGizmos()
    {
        //Gizmos.DrawWireCube(transform.position, new Vector3(3, 3, 3));
        
        foreach (Transform t in transform)
        {
            //if (t != null)
                //Debug.Log(t.position);
            //Gizmos.color = Color.yellow;
            Gizmos.DrawWireSphere(t.position, 1);
            //Gizmos.DrawIcon(t.position, "SpawnPoint",true);
            
            
        }
    }

}
