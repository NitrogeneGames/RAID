using UnityEngine;
using System.Collections;

public class Wep_MoveWithPlayer : MonoBehaviour {

	float verticalRotation = 0f;
	public float upDownRange = 60.0f;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
		//transform.Rotate (Camera.main.transform.forward);

		verticalRotation -= Input.GetAxis ("Mouse Y");
		verticalRotation = Mathf.Clamp (verticalRotation, -upDownRange, upDownRange);
		transform.localRotation = Quaternion.Euler (-verticalRotation, 180, 0);

		//transform.Rotate (Camera.main.transform.rotation.x, 0, 0);
		//transform.localEulerAngles = new Vector3 (Camera.main.transform.localRotation.x, 0, 0);
		//transform.rotation = Quaternion.Euler (Camera.main.transform.rotation.x, 180, 0);
		Debug.Log (transform.rotation.x + "          " + Camera.main.transform.rotation.x);
	}
}
