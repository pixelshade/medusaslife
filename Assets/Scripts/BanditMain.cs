using System;
using UnityEngine;
using System.Collections;
using Random = UnityEngine.Random;

public class BanditMain : MonoBehaviour
{
    public bool isHoldingTreasure = false;
    [SerializeField] private GameObject holdingTreasure;
    [SerializeField] private GameObject treasureToDrop;
    public float TimeNeededToTurnToStone = 1;
    public Material stoneMaterial;
    [SerializeField] private AudioClip[] screamSounds;
    [SerializeField] private AudioClip[] footstepSounds;    // an array of footstep sounds that will be randomly selected from.
    

    [SerializeField] private float stepInterval = 5;
    private float nextStep;

    private NavMeshAgent agent;
    private Animator animator;
    private GameObject nextTreasureGO;
    
    Vector2 smoothDeltaPosition = Vector2.zero;
    Vector2 velocity = Vector2.zero;
    
    private float timeTurningToStone = 0;
    private bool isMoving = false;
    private AudioSource audioSource;
    private bool _dead = false;

    // Use this for initialization
    private void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        animator = GetComponent<Animator>();
        agent.updatePosition = false;
        audioSource = GetComponent<AudioSource>();

        //reset height on spawn
        transform.position = new Vector3(transform.position.x, 0, transform.position.y);

        GoForNextRandomTreasure();
    }

    public void StartTurningToStone()
    {
        timeTurningToStone += Time.deltaTime;
        if (timeTurningToStone > TimeNeededToTurnToStone)
        {
            turnToStone();
        }
    }

    void turnToStone()
    {
        Die();  
        //agent.enabled = false;
        //Debug.Log("Turned to stone");

        transform.GetChild(1).GetComponent<Renderer>().material = stoneMaterial;
    }

    public void Die()
    {
        if (_dead) return;
        
        _dead = true;
        
        if (isHoldingTreasure)
        {
            Debug.Log("Dropujem treasure");
            Instantiate(treasureToDrop, transform.position+(new Vector3(2,0.45f,2)), Quaternion.identity);
            isHoldingTreasure = false;
            holdingTreasure.SetActive(false);
        }
        
        animator.Stop();
        agent.destination = transform.position;
        agent.Stop();        
        var rndScream = screamSounds[Random.Range(0, screamSounds.Length-1)];
        //Debug.Log("Im dying...GG"+rndScream.name);
        audioSource.PlayOneShot(rndScream);
    }

    private void BringTreasureToSpawnPoint()
    {
        Debug.Log("im bringin treasure");
        agent.destination = SpawnPoints.Instance.GetPositionOfRandomSpawnPoint();
    }

    private bool GoForNextRandomTreasure()
    {
        var gos = GameObject.FindGameObjectsWithTag("TREASURE");
        if (gos.Length == 0)
        {
            //there are no more treasures, we stop
            agent.Stop();
            return false;
        }
        // we choose random treasure
        var goalId = Random.Range(0, gos.Length);
        nextTreasureGO = gos[goalId];
        agent.destination = nextTreasureGO.GetComponent<Transform>().position;
        return true;
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

        isMoving = velocity.magnitude > 1f && agent.remainingDistance > agent.radius;
        //Debug.Log(gameObject.name+" vel:"+velocity.magnitude);
        // Update animation parameters
        animator.SetBool("move", isMoving);
        animator.SetFloat("velx", velocity.x);
        //Debug.Log(velocity);
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
        if (isMoving && Time.time > nextStep)
        {
            var delay = stepInterval + (10*stepInterval / velocity.magnitude);
            //Debug.Log(delay);
            nextStep = Time.time + (delay);
            PlayFootStepAudio();
        }
    }

    private void OnCollisionEnter(Collision col)
    {
        //Debug.Log("COLLISION:" + col.gameObject.name);
        if (col.gameObject.CompareTag("TREASURE"))
        {
            isHoldingTreasure = true;
            holdingTreasure.SetActive(true);
            BringTreasureToSpawnPoint();
            Destroy(col.gameObject);
            
        } 
        
    }

    private void PlayFootStepAudio()
    {
        //if (!m_CharacterController.isGrounded)
        //{
        //    return;
        //}
        // pick & play a random footstep sound from the array,
        // excluding sound at index 0
        if (_dead) return;
        int n = Random.Range(1, footstepSounds.Length);
        audioSource.clip = footstepSounds[n];
        audioSource.PlayOneShot(audioSource.clip);
        // move picked sound to index 0 so it's not picked next time
        footstepSounds[n] = footstepSounds[0];
        footstepSounds[0] = audioSource.clip;
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
        if (isHoldingTreasure && col.gameObject.transform.parent !=null && col.gameObject.transform.parent.GetComponent<SpawnPoints>() != null)
        {
            Debug.Log("Brought treasure to SP");
            isHoldingTreasure = false;
            holdingTreasure.SetActive(false);
            if (!GoForNextRandomTreasure())
            {
                agent.destination = transform.position;
                agent.Stop();
                Destroy(gameObject);
            }
            
            
            
        }
    }

}
