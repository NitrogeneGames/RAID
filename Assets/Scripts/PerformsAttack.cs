using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class PerformsAttack : MonoBehaviour {

	//How many seconds does it take between shots?
	public float cooldown = 0.2f;
	public float range = 100.0f;
	public float pickUpRange = 7.0f;
	public static float recoil = 0f;
	public float muzzleTimer = 0.02f;
	public float grenadeImpulse = 30f;
	public float inventoryPickUpTime = 5f;
	bool inventoryActivated = false;
	float cooldownRemaining = 0.0f;

	public GameObject DebrisPrefab;
	GameObject MuzzleFlash;
	GameObject MuzzleLight;
	public GameObject AkModel, M4model, Mp5model, Revolvermodel, SCARLmodel, G36Cmodel;
	public static GameObject M4A1, AK47, MP5, Revolver, SCARL, G36C;
	public GameObject Grenade_smoke, Grenade_flash, Grenade_CS, Grenade_frag;

	// Use this for initialization
	void Start () {
		AK47 = AkModel;
		M4A1 = M4model;
		MP5 = Mp5model;
		Revolver = Revolvermodel;
		SCARL = SCARLmodel;
		G36C = G36Cmodel;
	}

	public void MuzzleStart(){
		MuzzleFlash = gameObject.transform.GetChild (0).GetChild(0).FindChild ("Weapon Tip").GetChild (0).gameObject;
		MuzzleLight = gameObject.transform.GetChild (0).GetChild(0).FindChild ("Weapon Tip").GetChild (1).gameObject;
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
		List<Texture> templist = gameObject.GetComponent<InventoryController>().getItemList ();
		if (templist[4] != null) {
			if (Input.GetButtonDown ("Fire2") && templist[4].name == "smokegrenadetexture2") {
				GameObject grenade = (GameObject)Instantiate (Grenade_smoke, Camera.main.transform.position + Camera.main.transform.forward, Camera.main.transform.rotation);
				grenade.rigidbody.AddForce (Camera.main.transform.forward * grenadeImpulse, ForceMode.Impulse);
				gameObject.GetComponent<InventoryController>().RemoveSlot (4);
			} else if (Input.GetButtonDown ("Fire2") && templist[4].name == "dm51_a1_diff_spc") {
				GameObject grenade = (GameObject)Instantiate (Grenade_frag, Camera.main.transform.position + Camera.main.transform.forward, Camera.main.transform.rotation);
				grenade.rigidbody.AddForce (Camera.main.transform.forward * grenadeImpulse, ForceMode.Impulse);
				gameObject.GetComponent<InventoryController>().RemoveSlot (4);
			}
		}

		if(Input.GetKey(KeyCode.F) || inventoryActivated){
			gameObject.GetComponent<InventoryController>().inventoryOn = true;
			if(!inventoryActivated) gameObject.GetComponent<InventoryController>().guiAlpha = 1f;
		}else{
			gameObject.GetComponent<InventoryController>().inventoryOn = false;
		}

		if (Input.GetKeyDown (KeyCode.E)) {
			Ray ray = new Ray(Camera.main.transform.position, Camera.main.transform.forward);
			RaycastHit hitInfo;

			if(Physics.Raycast (ray, out hitInfo, pickUpRange)){
				Vector3 hitPoint = hitInfo.point;
				GameObject go = hitInfo.collider.gameObject;
				if(go.name == "M4A1_Sopmod_Body" && WeaponController.currentPrimary.GetID() != "M4A1"){
					//Instantiate new weapon from prefab to replace the version on the ground. Use the weapon currently held.
					switchPrimaryWeapon (go.transform.parent.gameObject, "M4A1");
				} else if(go.name == "Ak47Body" && WeaponController.currentPrimary.GetID() != "AK47"){
					switchPrimaryWeapon (go.transform.parent.gameObject, "AK47");
				} else if(go.name == "MP5" && WeaponController.currentPrimary.GetID() != "MP5"){
					switchPrimaryWeapon (go.transform.parent.gameObject, "MP5");
				} else if(go.name == "ScarLpart1" && WeaponController.currentPrimary.GetID() != "SCARL"){
					switchPrimaryWeapon (go.transform.parent.transform.parent.gameObject, "SCARL");
				} else if(go.name == "Cylinder001" && WeaponController.currentPrimary.GetID() != "G36C"){
					switchPrimaryWeapon (go.transform.parent.gameObject, "G36C");
				} else if(go.name == "Smoke Grenade"){
					Destroy (go.gameObject);
					gameObject.GetComponent<InventoryController>().SetSlot("SMOKEGRENADE");
					openInventory();
				} else if(go.name == "bodyDM" || go.name == "splintDM" || go.name == "spoonDM" || go.name == "capDM" || go.name == "springDM"){
					//frag grenade parts (bones)
					Destroy (go.transform.parent.gameObject);
					gameObject.GetComponent<InventoryController>().SetSlot("DM51A1");
					openInventory();
				} else if(go.name == "CS Grenade"){
					Destroy (go.gameObject);
					gameObject.GetComponent<InventoryController>().SetSlot("CSGRENADE");
					openInventory();
				}
			}
		}

	}

	void switchPrimaryWeapon(GameObject parentgo, string replacementgun){
		if(WeaponController.currentPrimary.GetID () == "M4A1") Instantiate (M4A1, Camera.main.transform.position + Camera.main.transform.forward, Quaternion.identity);
		else if(WeaponController.currentPrimary.GetID () == "AK47") Instantiate (AK47, Camera.main.transform.position + Camera.main.transform.forward, Quaternion.identity);
		else if(WeaponController.currentPrimary.GetID () == "MP5") Instantiate (MP5, Camera.main.transform.position + Camera.main.transform.forward, Quaternion.identity);
		else if(WeaponController.currentPrimary.GetID () == "SCARL") Instantiate (SCARL, Camera.main.transform.position + Camera.main.transform.forward, Quaternion.identity);
		else if(WeaponController.currentPrimary.GetID () == "G36C") Instantiate (G36C, Camera.main.transform.position + Camera.main.transform.forward, Quaternion.identity);
		Destroy (parentgo);
		WeaponController.currentPrimary.getGameObject().SetActive (false);
		WeaponController.currentPrimary = WeaponController.getWeaponByID(replacementgun);
		WeaponController.currentPrimary.getGameObject().SetActive (true);
		WeaponController.currentSecondary.getGameObject().SetActive (false);
		WeaponController.switchWeapon (WeaponController.currentPrimary);
		gameObject.GetComponent<InventoryController>().SetSlot(replacementgun);
		openInventory();
	}

	public static void dropItem(string Name){
		switch (Name) {
		case "AK47":
			Instantiate (AK47, Camera.main.transform.position + Camera.main.transform.forward, Quaternion.identity);
			break;
		case "M4A1": Instantiate (M4A1, Camera.main.transform.position + Camera.main.transform.forward, Quaternion.identity);
			break;
		case "MP5": Instantiate (MP5, Camera.main.transform.position + Camera.main.transform.forward, Quaternion.identity);
			break;
		}
	}

	IEnumerator MuzzleRegister(){
		MuzzleFlash.SetActive (true);
		MuzzleLight.SetActive (true);
		yield return new WaitForSeconds(muzzleTimer);
		MuzzleLight.SetActive (false);
		MuzzleFlash.SetActive (false);
	}

	void openInventory(){
		if (!inventoryActivated) {
			gameObject.GetComponent<InventoryController>().startGUIFade(1f, 0.3f, inventoryPickUpTime);
			StartCoroutine (AutoInventory ());
			gameObject.GetComponent<InventoryController>().guiAlpha = 1f;
		}
	}

	IEnumerator AutoInventory(){
		inventoryActivated = true;
		yield return new WaitForSeconds (inventoryPickUpTime);
		inventoryActivated = false;
	}

	void PlayFireSound() {
		GameObject go = gameObject.transform.GetChild (0).GetChild(0).GetChild(WeaponController.weaponlist.IndexOf(WeaponController.displayWeapon)).gameObject;
		go.audio.PlayOneShot (go.audio.clip);
	}
}
