using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class WeaponController : MonoBehaviour {

	public static List<Weapon> weaponlist;
	public static Weapon currentPrimary, currentSecondary, displayWeapon;
	public static bool isReloading = false;

	// Use this for initialization
	void Start () {
		GameObject go = gameObject.transform.GetChild(0).GetChild(0).gameObject;
		weaponlist = new List<Weapon>();
		foreach (Transform child in go.transform) {
			if(child.name == "M4A1"){
				addWeapon ("M4A1", "primary", child.gameObject, 30, 58f, 20, 3.5f, 0.06667f, 7.5f, true, true);
			} else if(child.name == "AK47"){
				addWeapon ("AK47", "primary", child.gameObject, 30, 70f, 40, 5f, 0.1f, 9.8f, false, false);
			} else if(child.name == "m16a4"){
				addWeapon ("G36C", "primary", child.gameObject, 30, 61f, 14, 3.3f, 0.08f, 6.2f, false, false);
			} else if(child.name == "SCARL"){
				addWeapon ("SCARL", "primary", child.gameObject, 30, 64f, 24, 2f, 0.96f, 0f, false, false);
			} else if(child.name == "MP5"){
				addWeapon("MP5", "primary", child.gameObject, 30, 68f, 50, 2.3f, 0.06667f, 5.5f, false, false);
			} else if (child.name == "44Revolver"){
				addWeapon("REVOLVER", "secondary", child.gameObject, 6, 78f, 37, 11f, 0.8f, 2.3f, false, true);
			} else{
				//Debug.Log ("Too many weapons in WEAPON tree");
			}
		}
		displayWeapon = currentPrimary;
	}

	void addWeapon(string ID, string slot, GameObject child, int clipsize, float damage, int accuracy, float recoil, float firerate, float mass, bool start, bool have){
		Weapon temp = new Weapon(ID, slot, child, clipsize, damage, accuracy, recoil, firerate, mass);
		temp.setBullets (temp.GetClipSize ());
		weaponlist.Add (temp);
		if(start) temp.getGameObject().SetActive (true);
		else temp.getGameObject().SetActive (false);
		if(have) gameObject.GetComponent<InventoryController>().SetSlot(ID);
		if (have && slot == "primary") currentPrimary = temp;
		else if (have && slot == "secondary") currentSecondary = temp;
	}

	// Update is called once per frame
	void Update () {

		if(Input.GetKeyDown(KeyCode.R)){
			//if(!isReloading){
			//	isReloading = true;
			if(displayWeapon.GetWeaponType () == "primary"){
				currentPrimary.reload ();
			} else if (displayWeapon.GetWeaponType () == "secondary"){
				currentSecondary.reload ();
			}
			//}
	}

		//Placeholder for weapon switching
		if (Input.GetKeyDown (KeyCode.Q)) {
			if(displayWeapon.GetWeaponType () == "primary"){
				switchWeapon(currentSecondary);
				(currentSecondary).getGameObject().SetActive(true);
				(currentPrimary).getGameObject().SetActive(false);
			} else if(displayWeapon.GetWeaponType () == "secondary"){
				switchWeapon(currentPrimary);
				(currentPrimary).getGameObject().SetActive(true);
				(currentSecondary).getGameObject().SetActive(false);
			}
						/*
			if(currentEquipped.GetID() == "M4A1"){
				switchWeapon((Weapon)(weaponlist[1]));
				((Weapon) weaponlist[1]).getGameObject().SetActive(true);
				((Weapon) weaponlist[0]).getGameObject().SetActive(false);
			} else{
				switchWeapon((Weapon)(weaponlist[0]));
				((Weapon) weaponlist[0]).getGameObject().SetActive(true);
				((Weapon) weaponlist[1]).getGameObject().SetActive(false);
			}
			*/
			}
	}

	public static void switchWeapon(Weapon switchToWep){
		displayWeapon = switchToWep;
	}

	public static Weapon getWeaponByID(string ID){
		foreach (Weapon w in weaponlist) {
			if(w.GetID() == ID){
				return w;
			}
		}
		return null;
	}

}

public class Weapon{
	private GameObject parentgo;
	private int clipsize, accuracy;
	private float mass, recoil, firerate, damage;
	private int bulletsLeft;
	private string id;
	private string type;

	public Weapon(string ID, string type, GameObject parentgo, int clipsize, float damage, int accuracy, float recoil, float firerate, float mass){
		this.parentgo = parentgo;
		this.clipsize = clipsize;
		this.damage = damage;
		this.accuracy = accuracy;
		this.recoil = recoil;
		this.firerate = firerate;
		this.mass = mass;
		this.id = ID;
		this.type = type;
	}

	public float GetDamage(){
		return damage;
	}

	public void useBullet(){
		if (bulletsLeft > 0) {
			bulletsLeft--;
		}

	}

	public void setBullets(int amt){
		bulletsLeft = amt;
	}

	public void reload(){
		bulletsLeft = this.clipsize;
	}

	public float GetRecoil(){
		return recoil;
	}

	public int GetBulletsLeft(){
		return bulletsLeft;
	}

	public string GetID(){
		return id;
	}	

	public GameObject getGameObject(){
		return parentgo;
	}

	public int GetClipSize(){
		return clipsize;
	}

	public string GetWeaponType(){
		return type;
	}
}
