using UnityEngine;
using System.Collections;

public class Grenade_frag : MonoBehaviour {

	public float lifespan = 3.0f;
	public GameObject explosionEffect;
	GameObject tempExplosion;

	void explode(){
		tempExplosion = (GameObject)(Instantiate (explosionEffect, gameObject.transform.position, Quaternion.identity));
		Destroy (gameObject);
	}
	
	// Update is called once per frame
	void Update () {
			lifespan -= Time.deltaTime;
			if (lifespan <= 0.0f) {
				explode ();
			}
	}
}
