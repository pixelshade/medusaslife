using UnityEngine;
using System.Collections;

public class Player : MonoBehaviour {
    private Transform cam;
    private LineRenderer laserLine;
    private Vector2 uvOffset = Vector2.zero;
    private Vector2 uvAnimationRate = new Vector2(15, 0);
    public GameObject laserHitParticleSystem;
    public GameObject VisionCollider;
    private ParticleSystem medusaVisionParticles;

    private GameObject hitParticleSystem;
    // Use this for initialization
    void Start () {
        cam = this.gameObject.GetComponentInChildren<Camera>().transform;
        //cam = Camera.main.transform;
        laserLine = GetComponent<LineRenderer>();
        laserLine.SetWidth(0.2f, 0.2f);
        hitParticleSystem = Instantiate(laserHitParticleSystem);
        hitParticleSystem.SetActive(false);
        medusaVisionParticles = gameObject.GetComponentInChildren<ParticleSystem>();
        
    }
	
	// Update is called once per frame
	void Update () {
        if (Input.GetButton("TurnToStone"))
        {
            //Fire();           
            medusaVisionParticles.enableEmission = true;
            TurnToStone(true);
        }else
        {
            TurnToStone(false);
            medusaVisionParticles.enableEmission = false;
           
                
            // turn of laser
            //hitParticleSystem.SetActive(false);
            //Laser(Vector3.zero, Vector3.zero);
        }
    }

    void TurnToStone(bool turnToStone)
    {
        if (VisionCollider == null) { Debug.LogError("Vision collider not set"); return; }
        var vc = VisionCollider.GetComponent<VisionCollider>();
        vc.TurnToStoneOnCollision = turnToStone;
    }

    void Fire()
    {
        RaycastHit hit;
        // get the forward vector of the player's camera 
        Vector3 fwd = cam.TransformDirection(cam.forward);
        //Laser(transform.position, transform.position + fwd);

        Ray ray = new Ray(transform.position, fwd);
        if (Physics.Raycast(ray, out hit))
        {
            Laser(ray.origin, hit.point);
            hitParticleSystem.transform.position = hit.point;
            hitParticleSystem.SetActive(true);
            //if there something in our front, check if it's the monolith
            if (hit.collider.gameObject.CompareTag("Enemy"))
            {

                var bandit = hit.collider.gameObject.GetComponent<BanditMain>();
                if (bandit == null) return;
                bandit.StartTurningToStone();

            }

        }
        else
        {
            Laser(ray.origin, ray.GetPoint(50));
            hitParticleSystem.SetActive(false);
        }
    }


    void Laser(Vector3 from, Vector3 to)
    {
        laserLine.SetPosition(0, from);
        laserLine.SetPosition(1, to);
        
        uvOffset = (uvAnimationRate * Mathf.Sin(Time.deltaTime));
        laserLine.materials[0].SetTextureOffset("_MainTex", uvOffset);
    }
}
