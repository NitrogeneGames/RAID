using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class WeaponController : MonoBehaviour {

	public static List<Weapon> weaponlist;
	public static Weapon currentPrimary, currentSecondary, displayWeapon;
	public static int smokeAmt, csAmt, flashAmt, fragAmt = 0;
	public static bool isReloading = false;

	// Use this for initialization
	void Start () {
		GameObject go = gameObject.transform.GetChild(0).GetChild(0).gameObject;
		weaponlist = new List<Weapon>();
		foreach (Transform child in go.transform) {
			if(child.name == "M4A1"){
				Weapon m4 = new Weapon("M4A1", "primary", child.gameObject, 22, 40f, 0, 3f, 0.2f, 0f);
				m4.setBullets (m4.GetClipSize());
				weaponlist.Add(m4);
				currentPrimary = m4;
				m4.getGameObject().SetActive (true);
			} else if(child.name == "AK47"){
				Weapon ak47 = new Weapon("AK47", "primary", child.gameObject, 25, 50f, 0, 4f, 0.15f, 0f);
				ak47.setBullets (ak47.GetClipSize());
				weaponlist.Add(ak47);
				ak47.getGameObject().SetActive (false);
			} else if(child.name == "MP5"){
				Weapon mp5 = new Weapon("MP5", "primary", child.gameObject, 40, 15, 0, 0.7f, 0.6f, 0f);
				mp5.setBullets (mp5.GetClipSize());
				weaponlist.Add(mp5);
				mp5.getGameObject().SetActive (false);
			} else if (child.name == "44Revolver"){
				Weapon revolver = new Weapon("REVOLVER", "secondary", child.gameObject, 40, 15, 0, 0.7f, 0.6f, 0f);
				revolver.setBullets (revolver.GetClipSize());
				weaponlist.Add(revolver);
				currentSecondary = revolver;
				revolver.getGameObject().SetActive (false);
			} else{
				//Debug.Log ("Too many weapons in WEAPON tree");
			}
		}
		displayWeapon = currentPrimary;
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
