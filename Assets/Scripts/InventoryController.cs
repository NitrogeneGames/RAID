using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class InventoryController : MonoBehaviour {
	public Texture InventoryBack;
	public bool inventoryOn;
	bool SlotsSet = false;
	bool started = false;
	public int availableStorage = 5;
	int nextAvailableStore;
	public float guiAlpha = 1f;

	public Texture ak47;
	public Texture m4a1;
	public Texture mp5;
	public Texture revolver;
	public Texture smokegrenade;
	public Texture fraggrenade;

	List<Texture> ItemList;

	// Use this for initialization
	void Start () {
		if (!started) {
			ItemList = new List<Texture> ();
			//ItemList.Capacity = 20 + availableStorage;
			for (int i = 0; i < 5 + availableStorage; i++) {
				ItemList.Add (null);
			}
			nextAvailableStore = 5;
			started = true;
		}
	}

	/*
	 *Inventory Slot guide:
	 *1-Primary weapon
	 *2-Sidearm/Secondary weapon
	 *3-Active melee weapon
	 *4-Active grenade weapon
	 *else if(ItemList[1] != null && nextAvailableStore == -1){
				PerformsAttack.dropItem (ItemList[1].name);
			}
	*/
	public void SetSlot(string ItemName){
		if (!started) {
			this.Start ();
		}
		CalculateStorage();
		if (ItemName == "AK47") {
			ItemList[1] = ak47;
		} else if (ItemName == "M4A1") {
			ItemList[1] = m4a1;
		} else if (ItemName == "MP5") {
			ItemList[1] = mp5;
		}
		else if (ItemName == "REVOLVER") {
			ItemList[2] = revolver;
		} else if (ItemName == "SMOKEGRENADE") {
			if(ItemList[4] != null && nextAvailableStore != -1){
				ItemList[nextAvailableStore] = ItemList[4];
			} 
			ItemList[4] = smokegrenade;
		} else if (ItemName == "DM51A1") {
			if(ItemList[4] != null && nextAvailableStore != -1){
				ItemList[nextAvailableStore] = ItemList[4];
			} 
			ItemList[4] = fraggrenade;
		} else if (ItemName == "CSGRENADE") {
			if(ItemList[4] != null && nextAvailableStore != -1){
				ItemList[nextAvailableStore] = ItemList[4];
			} 
			ItemList[4] = fraggrenade;
		} 
		CalculateStorage();
		SlotsSet = true;
	}

	public void RemoveSlot (int slot){
		Texture temp = ItemList [slot];
		ItemList [slot] = null;
		for(int p = 0; p < availableStorage; p++){
			if(ItemList[p+5] == temp){
				ItemList [slot] = ItemList[p+5];
				ItemList[p+5] = null;
				break;
			}
		}
	}

	//Draws GUI every update/frame
	void OnGUI(){
		if (inventoryOn && SlotsSet) {
			GUI.color = new Color(GUI.color.r, GUI.color.g, GUI.color.b, guiAlpha);
			//All Item backgrounds
			GUI.Label (new Rect (20,20,100,100), InventoryBack);
			GUI.Label (new Rect (140,20,100,100), InventoryBack);
			GUI.Label (new Rect (260,20,100,100), InventoryBack);
			GUI.Label (new Rect (380,20,100,100), InventoryBack);
			for(int j = 0; j < availableStorage; j++){
				GUI.Label (new Rect (20 + (j*120),160,100,100), InventoryBack);
			}

			if(ItemList[1] != null){
				GUI.Label (new Rect (20,20,100,100), ItemList[1]);
				GUI.Label (new Rect (24,22,20,20), "1");
				GUI.Label (new Rect (76,96,100,20), WeaponController.currentPrimary.GetBulletsLeft() + "/" + WeaponController.currentPrimary.GetClipSize());
			}
			if(ItemList[2] != null){
				GUI.Label (new Rect (140,20,100,100), ItemList[2]);
				GUI.Label (new Rect (144,22,20,20), "2");
				GUI.Label (new Rect (196,96,100,20), WeaponController.currentSecondary.GetBulletsLeft() + "/" + WeaponController.currentSecondary.GetClipSize());
			}
			if(ItemList[3] != null){
				GUI.Label (new Rect (260,20,100,100), ItemList[3]);
				GUI.Label (new Rect (264,22,20,20), "3");
			}
			if(ItemList[4] != null){
				GUI.Label (new Rect (380,20,100,100), ItemList[4]);
				GUI.Label (new Rect (384,22,20,20), "4");
			}
			for(int h = 0; h < availableStorage; h++){
				if(ItemList[h+5] != null){
					GUI.Label (new Rect (20 + (h*120),160,100,100), ItemList[h+5]);
				}
			}
		}
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	public List<Texture> getItemList(){
		return ItemList;
	}

	public void CalculateStorage(){
		nextAvailableStore = -1;
		for (int i = 0; i < availableStorage; i++) {
			if (ItemList [i + 5] == null) {
				nextAvailableStore = i + 5;
				break;
				}
			}
		}
	
	public void startGUIFade(float start, float end, float length){
		StartCoroutine (GUIFade (start, end, length));
	}

	public IEnumerator GUIFade (float start, float end, float length) {
		for (float i = 0.0f; i <= 1.0f; i += Time.deltaTime*(1/length)) {
			guiAlpha = Mathf.Lerp(start, end, i);
			yield return new WaitForSeconds(Time.deltaTime*(1/length));
		}
		guiAlpha = 0f;
	}
}
