using UnityEngine;
using System.Collections;

[RequireComponent (typeof(CharacterController))]
public class FirstPersonController : MonoBehaviour {

	public float movementSpeed = 5.0f;
	public float upDownRange = 60.0f;
	public float jumpSpeed = 10.0f;
	float verticalRotation = 0f;

	float verticalVelocity = 0.0f;

	CharacterController characterController;

	// Use this for initialization
	void Start () {
		Screen.lockCursor = true;
		characterController = GetComponent<CharacterController> ();
	}
	
	// Update is called once per frame
	void Update () {

		//rotation
		float rotLeftRight = Input.GetAxis ("Mouse X");
		transform.Rotate (0, rotLeftRight, 0);

		verticalRotation -= Input.GetAxis ("Mouse Y");
		verticalRotation = Mathf.Clamp (verticalRotation, -upDownRange, upDownRange);
		//verticalRotation -= PerformsAttack.recoil;
		StartCoroutine (DoRecoil(PerformsAttack.recoil, 5f));
		Camera.main.transform.localRotation = Quaternion.Euler (verticalRotation, 0, 0);

		//movement
		float movementScalar;
		if (characterController.isGrounded) {
			movementScalar = 1f;
		} else {
			movementScalar = 0.5f;
		}
		float forwardSpeed = Input.GetAxis ("Vertical") * movementSpeed * movementScalar;
		float sideSpeed = Input.GetAxis ("Horizontal") * movementSpeed * movementScalar;

		verticalVelocity += Physics.gravity.y * Time.deltaTime;
		if (characterController.isGrounded && Input.GetButtonDown ("Jump"))
		{
			verticalVelocity = jumpSpeed;
		}

		Vector3 speed = new Vector3 (sideSpeed , verticalVelocity, forwardSpeed);


		speed = transform.rotation * speed;
		characterController.Move (speed * Time.deltaTime);
	}

	IEnumerator DoRecoil(float recoil, float smoothness){
		float totaltime = .01F;
		for(float i = 0F; i<smoothness; i++) {
			verticalRotation -= recoil/smoothness;
			yield return new WaitForSeconds(totaltime/smoothness);
		}
	}

}