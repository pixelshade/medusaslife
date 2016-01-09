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
        }
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
