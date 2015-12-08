using UnityEngine;
using System.Collections;

public class FP_Shooting : MonoBehaviour {

	public GameObject bullet_prefab;
	public float bulletImpulse = 100f;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		if (Input.GetButtonDown ("Fire1")) {
			Camera cam = Camera.main;
			GameObject bullet = (GameObject) Instantiate(bullet_prefab, cam.transform.position + cam.transform.forward, cam.transform.rotation);
			bullet.GetComponent<Rigidbody>().AddForce(cam.transform.forward * bulletImpulse, ForceMode.Impulse);
		}
	}
}
