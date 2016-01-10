using UnityEngine;
using System.Collections;

public class VisionCollider : MonoBehaviour {
    public bool TurnToStoneOnCollision = false;
	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

    void OnTriggerStay(Collider col)
    {        
        if (!TurnToStoneOnCollision) return;        

        if (col.gameObject.CompareTag("Enemy"))
        {
            var bandit = col.gameObject.GetComponent<BanditMain>();
            if (bandit == null) return;
            bandit.StartTurningToStone();
            //isDirectHitWithoutBarrier(bandit.transform.position);
        }
    }

    bool isDirectHitWithoutBarrier(Vector3 target)
    {
        Debug.LogError("TODO: its now working");

        Vector3 direction = transform.TransformDirection(target);
        //Laser(transform.position, transform.position + fwd);
        RaycastHit hitInfo;
        Ray ray = new Ray(transform.parent.position, direction);
        Debug.DrawRay(transform.position, direction);
        if (Physics.Raycast(ray, out hitInfo))
        {
            if (hitInfo.transform.CompareTag("Enemy")){
                return true;
            }       
        }
        return false;
    }



    //void OnTriggerExit(Collider col)
    //{
    //    if (col.gameObject.CompareTag("Enemy"))
    //    {
    //        var bandit = col.gameObject.GetComponent<BanditMain>();
    //        if (bandit == null) return;
    //        bandit.StartTurningToStone();

    //    }
    //}
}
