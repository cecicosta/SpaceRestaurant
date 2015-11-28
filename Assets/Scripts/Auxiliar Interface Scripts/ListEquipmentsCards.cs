using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class ListEquipmentsCards : MonoBehaviour {

	public EquipmentCard equipmentCard;
	private List<EquipmentCard> cards = new List<EquipmentCard> ();
	private EstablishmentManagement establishmentManager;
	private Establishment establishment;
	public enum EquipmentViwer {Available, Aquired};
	public EquipmentViwer viwerType;
	// Use this for initialization
	void Awake () {
		establishmentManager = EstablishmentManagement.GetInstance ();
		if (establishmentManager == null) {
			Debug.LogError ("EstablishmentManager object is null");
			return;
		}
		establishment = establishmentManager.establishment;
	}
	
	void OnEnable() {
		CreateCards ();
		EventManager.OnUpdateEquipments += CreateCards;
	}
	
	void OnDisable(){
		EventManager.OnUpdateEquipments -= CreateCards;
	}
	
	void CreateCards(){
		if(cards.Count != 0){
			foreach(EquipmentCard ad in cards){
				Destroy(ad.gameObject);
			}
			cards.Clear();
		}
		List<Equipment> equipments_list = null;
		if (viwerType == EquipmentViwer.Available) {
			equipments_list = establishment.infrastructure.GetProviderEquipmentsList ();
		} else if(viwerType == EquipmentViwer.Aquired) {
			equipments_list = establishment.infrastructure.GetAquiredEquipmentsList ();
		}

		if (equipments_list == null) {
			Debug.LogError("Error getting the equipments list");
			return;
		}

		foreach(Equipment ad in equipments_list){
			EquipmentCard card = Instantiate(equipmentCard);
			card.transform.SetParent(this.transform);
			//TODO: find image by candidate name
			cards.Add(card);
		}
	}	
}
