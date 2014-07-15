using UnityEngine;
using System.Collections;

public class BasicHealth : MonoBehaviour {

	public float health = 100.0f;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		if (health <= 0f) {
			Destroy (gameObject);
		}
	}

	public void Damage(float amt){
		health -= amt;
	}
}
