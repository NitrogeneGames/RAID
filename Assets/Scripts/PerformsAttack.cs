using UnityEngine;
using System.Collections;

public class PerformsAttack : MonoBehaviour {

	//How many seconds does it take between shots?
	public float cooldown = 0.2f;
	public float range = 100.0f;
	public static float recoil = 0f;
	public float muzzleTimer = 0.02f;
	public float grenadeImpulse = 30f;
	float cooldownRemaining = 0.0f;

	public GameObject DebrisPrefab;
	GameObject MuzzleFlash;
	GameObject MuzzleLight;
	public GameObject Grenade_smoke, Grenade_flash, Grenade_CS, Grenade_frag;

	// Use this for initialization
	void Start () {

	}

	public void MuzzleStart(){
		MuzzleFlash = gameObject.transform.GetChild (0).GetChild (0).GetChild(WeaponController.weaponlist.Count).GetChild (0).gameObject;
		MuzzleLight = gameObject.transform.GetChild (0).GetChild (0).GetChild(WeaponController.weaponlist.Count).GetChild (1).gameObject;
		MuzzleLight.SetActive(false);
		MuzzleFlash.SetActive(false);
	}
	
	// Update is called once per frame
	void Update () {
		if (WeaponController.weaponlist != null && MuzzleFlash == null) {
			MuzzleStart ();
		}
		cooldownRemaining -= Time.deltaTime;
		if(Input.GetMouseButton(0) && cooldownRemaining <= 0 && WeaponController.displayWeapon.GetBulletsLeft() > 0){
			recoil = WeaponController.currentPrimary.GetRecoil();
			WeaponController.displayWeapon.useBullet();
			cooldownRemaining = cooldown;
			StartCoroutine(MuzzleRegister ());
			PlayFireSound ();

			Ray ray = new Ray(Camera.main.transform.position, Camera.main.transform.forward);
			RaycastHit hitInfo;

			//hitInfo.collider returns the object hit
			//GameObject go = hitInfo.collider;

			if(Physics.Raycast(ray, out hitInfo, range)){
				Vector3 hitPoint = hitInfo.point;
				GameObject go = hitInfo.collider.gameObject;
				if(go.GetComponent<BasicHealth>() != null){
					BasicHealth bh = go.GetComponent<BasicHealth>();
					bh.Damage (WeaponController.displayWeapon.GetDamage());
				}

				if(DebrisPrefab != null){
					Instantiate(DebrisPrefab, hitPoint, Quaternion.identity);
					//(ParticleSystem) DebrisPrefab.particleSystem.loop = false;
				}
			
			}
		} else{
			recoil = 0;
		}

		//Grenade handler
		if (Input.GetButtonDown ("Fire2")) {
			GameObject grenade = (GameObject) Instantiate(Grenade_smoke, Camera.main.transform.position + Camera.main.transform.forward, Camera.main.transform.rotation);
			grenade.rigidbody.AddForce(Camera.main.transform.forward * grenadeImpulse, ForceMode.Impulse);
		}

		if(Input.GetButtonDown

	}

	IEnumerator MuzzleRegister(){
		MuzzleFlash.SetActive (true);
		MuzzleLight.SetActive (true);
		yield return new WaitForSeconds(muzzleTimer);
		MuzzleLight.SetActive (false);
		MuzzleFlash.SetActive (false);
	}

	void PlayFireSound() {
		//Component[] audlist;
		GameObject go = gameObject.transform.GetChild (0).GetChild(0).GetChild(WeaponController.weaponlist.IndexOf(WeaponController.displayWeapon)).gameObject;
		//Debug.Log (WeaponController.weaponlist.IndexOf (WeaponController.currentEquipped));
		go.audio.PlayOneShot (go.audio.clip);
	}
}
