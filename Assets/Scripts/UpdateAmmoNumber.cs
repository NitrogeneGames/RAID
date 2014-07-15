using UnityEngine;
using System.Collections;

public class UpdateAmmoNumber : MonoBehaviour {

	void OnGUI () {
		GUI.Label (new Rect (0,0,100,50), WeaponController.displayWeapon.GetBulletsLeft() + "/" + WeaponController.displayWeapon.GetClipSize());
	}

	// Update is called once per frame
	void Update () {

	}
	
}
