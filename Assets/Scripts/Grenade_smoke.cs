using UnityEngine;
using System.Collections;

public class Grenade_smoke : MonoBehaviour {
	
	public float lifespan = 3.0f;
	public float smokelifespan = 10.0f;
	bool smokePhase = false;
	public GameObject explosionEffect;
	GameObject tempExplosion;
	
	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		if (smokePhase) {
			smokelifespan -= Time.deltaTime;
			if(smokelifespan <= 0.0f){
				tempExplosion.particleEmitter.emit = false;
				Destroy (gameObject);
				if(tempExplosion.particleEmitter.particleCount <= 0){
					Destroy (tempExplosion);
				}
			}
		} else {
			lifespan -= Time.deltaTime;
			if (lifespan <= 0.0f) {
				smokePhase = true;
				explode ();
			}
		}
	}
	
	void explode(){
		tempExplosion = (GameObject)(Instantiate (explosionEffect, gameObject.transform.position, Quaternion.identity));
	}

	void OnCollisionEnter(){

	}
}
