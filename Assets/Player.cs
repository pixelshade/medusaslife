using UnityEngine;
using System.Collections;

public class Player : MonoBehaviour {
    private Transform cam;
    private LineRenderer laserLine;
    private Vector2 uvOffset = Vector2.zero;
    private Vector2 uvAnimationRate = new Vector2(15, 0);
    public GameObject laserHitParticleSystem;
    // Use this for initialization
    void Start () {
        cam = Camera.main.transform;
        laserLine = GetComponent<LineRenderer>();
        laserLine.SetWidth(0.2f, 0.2f);

	}
	
	// Update is called once per frame
	void Update () {
        if (Input.GetMouseButton(0))
        {
            RaycastHit hit;
            // get the forward vector of the player's camera 
            //Vector3 fwd = cam.TransformDirection(cam.forward);
            //Laser(transform.position, transform.position + fwd);

            Ray ray = new Ray(transform.position, cam.forward);            
            if (Physics.Raycast(ray, out hit))
            {
                Laser(ray.origin, hit.point);

                //if there something in our front, check if it's the monolith
                if (hit.collider.gameObject.CompareTag("Enemy"))
                {
                    var bandit = hit.collider.gameObject.GetComponent<BanditMain>();
                    if (bandit == null) return;
                    bandit.StartTurningToStone();

                }
                
            } else
            {
                Laser(ray.origin, ray.GetPoint(50));
            }

        }else
        {
            Laser(Vector3.zero, Vector3.zero);
        }
        


        //var ray = new Ray(cam.position, cam.forward);
        //RaycastHit hit;
        //if (Physics.Raycast(ray,
        //{

        //}
    }


    void Laser(Vector3 from, Vector3 to)
    {
        laserLine.SetPosition(0, from);
        laserLine.SetPosition(1, to);
        
        uvOffset = (uvAnimationRate * Mathf.Sin(Time.deltaTime));
        laserLine.materials[0].SetTextureOffset("_MainTex", uvOffset);
    }
}
