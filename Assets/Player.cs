using UnityEngine;
using System.Collections;

public class Player : MonoBehaviour {
    private Transform cam = Camera.main.transform;
	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
        if (Input.GetMouseButtonDown(0))
        {
            RaycastHit hit;
            // get the forward vector of the player's camera 
            Vector3 fwd = transform.TransformDirection(Vector3.forward);
            //create a ray using fwd vector as direction and a max size of 20.0f 
            //hit is the out parameter if something is detected
            if (Physics.Raycast(transform.position, fwd, out hit, 20.0F))
            {
                //if there something in our front, check if it's the monolith
                //  if (hit.collider.gameObject.name.Equals("monolith"))
                print(hit.collider.gameObject.name);
            }

        }


        //var ray = new Ray(cam.position, cam.forward);
        //RaycastHit hit;
        //if (Physics.Raycast(ray,
        //{

        //}
    }


    void Laser(Vector3 from, Vector3 to)
    {

    }
}
