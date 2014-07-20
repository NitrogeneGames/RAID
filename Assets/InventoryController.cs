using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class InventoryController : MonoBehaviour {
	public Texture InventoryBack;
	static bool inventoryOn = true;
	bool SlotsSet = false;
	public int availableStorage = 5;
	int nextAvailableStore;

	public Texture ak47;
	public Texture m4a1;
	public Texture mp5;
	public Texture revolver;
	public Texture smokegrenade;

	List<Texture> ItemList;

	// Use this for initialization
	void Start () {
		ItemList = new List<Texture> ();
		for (int e = 0; e < 11; e++) {
			ItemList.Add (null);
		}
		nextAvailableStore = 5;
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
	public void SetSlot(int slot, string ItemName){
		CalculateStorage();
		if (ItemName == "AK47") {
			if(ItemList[1] != null && nextAvailableStore != -1 && ItemList[1] != ak47){
				ItemList[nextAvailableStore] = ItemList[1];
			}
			ItemList[1] = ak47;
		} else if (ItemName == "M4A1") {
			if(ItemList[1] != null && nextAvailableStore != -1 && ItemList[1] != m4a1){
				ItemList[nextAvailableStore] = ItemList[1];
			} 
			ItemList[1] = m4a1;
		} else if (ItemName == "MP5") {
			if(ItemList[1] != null && nextAvailableStore != -1 && ItemList[1] != mp5){
				ItemList[nextAvailableStore] = ItemList[1];
			} 
			ItemList[1] = mp5;
		}
		else if (ItemName == "REVOLVER") {
			if(ItemList[2] != null && nextAvailableStore != -1 && ItemList[1] != revolver){
				ItemList[nextAvailableStore] = ItemList[2];
			} 
			ItemList[2] = revolver;
		} else if (ItemName == "SMOKEGRENADE") {
			if(ItemList[4] != null && nextAvailableStore != -1){
				ItemList[nextAvailableStore] = ItemList[4];
			} 
			ItemList[4] = smokegrenade;
		}
		CalculateStorage();
		SlotsSet = true;
	}

	//Draws GUI every update/frame
	void OnGUI(){
		if (inventoryOn && SlotsSet) {
			//All Item backgrounds
			GUI.Label (new Rect (20,20,100,100), InventoryBack);
			GUI.Label (new Rect (140,20,100,100), InventoryBack);
			GUI.Label (new Rect (260,20,100,100), InventoryBack);
			GUI.Label (new Rect (380,20,100,100), InventoryBack);
			for(int j = 0; j < availableStorage; j++){
				GUI.Label (new Rect (380 + (j*120),160,100,100), InventoryBack);
			}

			if(ItemList[1] != null) GUI.Label (new Rect (20,20,100,100), ItemList[1]);
			if(ItemList[2] != null) GUI.Label (new Rect (140,20,100,100), ItemList[2]);
			if(ItemList[3] != null) GUI.Label (new Rect (260,20,100,100), ItemList[3]);
			if(ItemList[4] != null) GUI.Label (new Rect (380,20,100,100), ItemList[4]);
			for(int h = 0; h < availableStorage; h++){
				if(ItemList[h+5] != null){
					GUI.Label (new Rect (380 + (h*120),160,100,100), ItemList[h+5]);
				}
			}
		}
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	public void CalculateStorage(){
		nextAvailableStore = -1;
		for (int i = 0; i < availableStorage; i++) {
			if(ItemList[i+5] == null){
				nextAvailableStore = i+5;
				continue;
			}
		}
	}
}
