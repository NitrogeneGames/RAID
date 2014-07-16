using UnityEngine;
using System.Collections;

[RequireComponent (typeof(CharacterController))]
public class FirstPersonController : MonoBehaviour {

	public float movementSpeed = 5.0f;
	public float upDownRange = 60.0f;
	public float jumpSpeed = 10.0f;
	bool iCR = false;
	bool isSprinted;
	bool sprintProcessed = true;
	bool crouchProcessed = false;
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
		float movementScalar;
		if (Input.GetKey (KeyCode.C)) {
			movementScalar = 0.5f;
			isSprinted = false;
			//gameObject.transform.position.Set (0,0,0);
			if((!crouchProcessed) && (!iCR)){
				crouchProcessed = true;
				iCR = true;
				StartCoroutine (DoCrouch(-.5f, 50));
			}
		} else if (Input.GetKey (KeyCode.LeftShift)) {
			movementScalar = 1.3f;
			isSprinted = true;
			//gameObject.transform.position.Set (0,1,0);
			if((crouchProcessed) && (!iCR)){
				crouchProcessed = false;
				iCR = true;
				StartCoroutine (DoCrouch(.5f,50));
			}
		} else{
			movementScalar = 1f;
			isSprinted = false;
			//gameObject.transform.Translate (new Vector3(0,1,0));
			if((crouchProcessed) && (!iCR)){
				crouchProcessed = false;
				iCR = true;
				StartCoroutine (DoCrouch(.5f, 50));
			}
		}

		if (isSprinted && sprintProcessed) {
			sprintProcessed = false;
			Camera.main.fieldOfView += 2f;
		}
		if (!isSprinted && !sprintProcessed) {
			sprintProcessed = true;
			Camera.main.fieldOfView -= 2f;
		}
		
		if (!characterController.isGrounded) {
			movementScalar *= 0.5f;
		}
			//gameObject.transform.Translate (new Vector3(0,2,0));
			//gameObject.transform.position.y;

		//rotation
		float rotLeftRight = Input.GetAxis ("Mouse X");
		transform.Rotate (0, rotLeftRight, 0);

		verticalRotation -= Input.GetAxis ("Mouse Y");
		verticalRotation = Mathf.Clamp (verticalRotation, -upDownRange, upDownRange);
		//verticalRotation -= PerformsAttack.recoil;
		StartCoroutine (DoRecoil(PerformsAttack.recoil, 5f));
		Camera.main.transform.localRotation = Quaternion.Euler (verticalRotation, 0, 0);

		//movement
		float forwardSpeed = Input.GetAxis ("Vertical") * movementSpeed * movementScalar;
		float sideSpeed;
		if (!isSprinted) {
			sideSpeed = Input.GetAxis ("Horizontal") * movementSpeed * movementScalar;
		} else {
			sideSpeed = 0f;
		}
		Debug.Log (Input.GetAxis ("Horizontal"));

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
			yield return wait(totaltime/smoothness);
		}
	}
	
	IEnumerator DoCrouch(float amt, int smoothness){
		float totaltime = .01F;
		for(int i = 0; i<smoothness; i++) {
			changeCrouch((float) (amt/smoothness));
			yield return wait((float) (totaltime/smoothness));
		}
		iCR = false;
	}
	
	
	
	IEnumerator wait(float time) {
		time = time / 1000;    
		float timecapture1 = Time.deltaTime;
		float timecapture2 = timecapture1;
		while (timecapture2-timecapture1 < time){
			timecapture2 = Time.deltaTime;       
		}
		yield return new WaitForSeconds (0f);
	}
	
	public void changeCrouch(float amt){
		gameObject.transform.Translate (new Vector3(0f,amt,0f));
		gameObject.GetComponent<CharacterController> ().height += 2*amt;
	}
}