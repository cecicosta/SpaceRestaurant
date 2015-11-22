using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class BuyIngredient : MonoBehaviour {

	public Text ingredientName; 
	private EstablishmentManagement establishmentManager;
	
	// Use this for initialization
	void Awake () {
		establishmentManager = EstablishmentManagement.GetInstance ();
		if (establishmentManager == null)
			Debug.LogError ("EstablishmentManager object is null");
	}
	
	public void Buy(){
		if (!establishmentManager.establishment.BuyIngredient(ingredientName.text)) {
			Debug.Log ("Error to buy ingredient");
		}
		EventManager.UpdateIngredients ();
	}
}
