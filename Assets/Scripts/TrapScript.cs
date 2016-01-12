using UnityEngine;
using System.Collections;

public class TrapScript : MonoBehaviour {
    private NavMeshObstacle _navMeshObstacle;
	// Use this for initialization
	void Start () {
        _navMeshObstacle = GetComponent<NavMeshObstacle>();
        _navMeshObstacle.enabled = false;
        var anim = GetComponent<Animation>();
        anim["Up Down"].wrapMode = WrapMode.Once;
        anim.Play("Up Down");
    }
	
	// Update is called once per frame
	void Update () {
	
	}

    void DisableTrapAndKillBandit(GameObject bandit)
    {
        
        if (bandit == null) return;
        var anim = GetComponent<Animation>();
        anim["Up Down"].wrapMode = WrapMode.Once;
        anim.Play("Up Down");
        _navMeshObstacle.enabled = true; //others will see the trap

        var banditScript = bandit.GetComponent<BanditMain>();
        banditScript.Die();        
    }

    void OnTriggerEnter(Collider col)
    {
        //Debug.Log("Trap catched:" + col.gameObject.name);
        if (col.gameObject.CompareTag("Enemy"))
        {
            DisableTrapAndKillBandit(col.gameObject);
        }
    }
}
