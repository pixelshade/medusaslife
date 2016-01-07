using System;
using UnityEngine;
using System.Collections;
using Random = UnityEngine.Random;

public class BanditMain : MonoBehaviour
{
    private NavMeshAgent agent;
    private Animator animator;
    private GameObject nextTreasureGO;
    Vector2 smoothDeltaPosition = Vector2.zero;
    Vector2 velocity = Vector2.zero;
    public bool isHoldingTreasure = false;
    // Use this for initialization
    private void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        animator = GetComponent<Animator>();
        agent.updatePosition = false;

        //reset height on spawn
        transform.position = new Vector3(transform.position.x, 0, transform.position.y);

        GoForNextRandomTreasure();
    }

    private void BringTreasureToSpawnPoint()
    {
        Console.WriteLine("im bringin treasure");
        agent.destination = SpawnPoints.Instance.GetPositionOfRandomSpawnPoint();
    }

    private void GoForNextRandomTreasure()
    {
        var gos = GameObject.FindGameObjectsWithTag("TREASURE");
        if (gos.Length == 0)
        {
            //there are no more treasures, we stop
            agent.Stop();
            return;
        }
        // we choose random treasure
        var goalId = Random.Range(0, gos.Length);
        nextTreasureGO = gos[goalId];
        agent.destination = nextTreasureGO.GetComponent<Transform>().position;
    }


    // Update is called once per frame
    private void Update()
    {
        Vector3 worldDeltaPosition = agent.nextPosition - transform.position;

        // Map 'worldDeltaPosition' to local space
        float dx = Vector3.Dot(transform.right, worldDeltaPosition);
        float dy = Vector3.Dot(transform.forward, worldDeltaPosition);
        Vector2 deltaPosition = new Vector2(dx, dy);

        // Low-pass filter the deltaMove
        float smooth = Mathf.Min(1.0f, Time.deltaTime / 0.15f);
        smoothDeltaPosition = Vector2.Lerp(smoothDeltaPosition, deltaPosition, smooth);

        // Update velocity if time advances
        if (Time.deltaTime > 1e-5f)
            velocity = smoothDeltaPosition / Time.deltaTime;

        bool shouldMove = velocity.magnitude > 0.5f && agent.remainingDistance > agent.radius;

        // Update animation parameters
        animator.SetBool("move", shouldMove);
        animator.SetFloat("velx", velocity.x);
        Debug.Log(velocity);
        animator.SetFloat("vely", velocity.y);

        //GetComponent<LookAt>().lookAtTargetPosition = agent.steeringTarget + transform.forward;
    }

    void OnAnimatorMove()
    {
        // Update position to agent position
        transform.position = agent.nextPosition;
    }

    private void FixedUpdate()
    {
        //check if our treasure still exists
        if (nextTreasureGO == null && isHoldingTreasure == false)
        {
            GoForNextRandomTreasure();
        }
    }

    private void OnCollisionEnter(Collision col)
    {
        //Debug.Log("COLLISION:" + col.gameObject.name);
        if (col.gameObject.CompareTag("TREASURE"))
        {
            isHoldingTreasure = true;
            Destroy(col.gameObject);
            BringTreasureToSpawnPoint();
        } 
        
    }

    private void OnTriggerEnter(Collider col)
    {
        //Debug.Log("------------");        
        //Debug.Log(isHoldingTreasure);
        //Debug.Log(col);
        //Debug.Log(col.gameObject);
        //Debug.Log(col.gameObject.transform.parent);
        //Debug.Log(col.gameObject.transform.parent.GetComponent<SpawnPoints>());
        //Debug.Log("------------");
        // its one of the SPs in parent spawnpoints GO adn we are bringing treasure
        if (isHoldingTreasure && col.gameObject.transform.parent.GetComponent<SpawnPoints>() != null)
        {
            Debug.Log("Brought treasure to SP");
            agent.Stop();
            Destroy(this);
        }
    }

}
