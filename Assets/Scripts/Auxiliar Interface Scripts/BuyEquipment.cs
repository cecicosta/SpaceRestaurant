using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class BuyEquipment : MonoBehaviour {

	public Text equipmentName; 
	private EstablishmentManagement establishmentManager;
	
	// Use this for initialization
	void Awake () {
		establishmentManager = EstablishmentManagement.GetInstance ();
		if (establishmentManager == null)
			Debug.LogError ("EstablishmentManager object is null");
	}
	
	public void Buy(){
		if (!establishmentManager.establishment.BuyEquipment(equipmentName.text)) {
			Debug.Log ("Error to buy equipment");
		}
		EventManager.UpdateEquipments ();
	}
}
