using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class ListEquipmentsCards : MonoBehaviour {

	public EquipmentCard equipmentCard;
	private List<EquipmentCard> cards = new List<EquipmentCard> ();
	private EstablishmentManagement establishmentManager;
	private Establishment establishment;
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
		List<Equipment> equipments_list = establishment.infrastructure.GetProviderEquipmentsList ();
		List<Equipment>	aquired_equipments_list = establishment.infrastructure.GetAquiredEquipmentsList ();
		 

		if (equipments_list == null) {
			Debug.LogError("Error getting the equipments list");
			return;
		}

		foreach(Equipment eq in equipments_list){
			EquipmentCard card = Instantiate(equipmentCard);
			card.transform.SetParent(this.transform);
			card.transform.localScale = new Vector3(1,1,1);
			card.name.text = eq.name;
			card.effect.text = eq.effect;
			card.description.text = eq.description;
			card.price.text = eq.price.ToString();
			if( aquired_equipments_list.Find(x => x.name == eq.name) != null )
				card.buy_button.interactable = false;
			else
				card.buy_button.interactable = true;
			//TODO: find image by candidate name
			cards.Add(card);
		}
	}	
}
