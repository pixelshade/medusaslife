using System;
using UnityEngine;
using System.Collections;
using Random = UnityEngine.Random;

public class BanditMain : MonoBehaviour {
    private NavMeshAgent agent;
	// Use this for initialization
    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        
        var gos = GameObject.FindGameObjectsWithTag("TREASURE");
        // we choose random target
        var goalId = Random.Range(0, gos.Length - 1);
        var goal = gos[goalId];
        agent.destination = goal.GetComponent<Transform>().position;

    }

    

	// Update is called once per frame
	void Update () {
	
	}

    void FixedUpdate()
    {
        
    }

    void OnCollisionEntrer(Collision col)
    {
        if (col.gameObject.name == "treasure")
        {
            Destroy(col.gameObject);
        }
    }
}
