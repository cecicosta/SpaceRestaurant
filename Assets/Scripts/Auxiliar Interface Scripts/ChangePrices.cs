using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class ChangePrices : MonoBehaviour {
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

	public void IncreasePrices(){
		MenuProvider m_provider = MenuProvider.GetInstance ();
		if (m_provider == null) {
			Debug.LogError("Error getting the menu");
			return;
		}
		List<Dish> menu_list = m_provider.GetDishList (); 
		foreach(Dish d in menu_list){
			establishment.IncreasePrices (d.name);
		}
	}

	public void DecreasePrices(){
		MenuProvider m_provider = MenuProvider.GetInstance ();
		if (m_provider == null) {
			Debug.LogError("Error getting the menu");
			return;
		}
		List<Dish> menu_list = m_provider.GetDishList (); 
		foreach(Dish d in menu_list){
			establishment.DecreasePrices (d.name);
		}
	}
}
