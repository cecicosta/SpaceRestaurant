using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class ListIngredientsCards : MonoBehaviour {
	public IngredientCard ingredientCard;
	private List<IngredientCard> cards = new List<IngredientCard> ();
	private EstablishmentManagement establishmentManager;
	private Establishment establishment;
	public enum IngredientViwer {Provider, Inventory};
	public IngredientViwer viwerType;
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
		EventManager.OnUpdateIngredients += CreateCards;
	}
	
	void OnDisable(){
		EventManager.OnUpdateIngredients -= CreateCards;
	}
	
	void CreateCards(){
		if(cards.Count != 0){
			foreach(IngredientCard i in cards){
				Destroy(i.gameObject);
			}
			cards.Clear();
		}
		List<Ingredient> ingredients_list = null;
		if (viwerType == IngredientViwer.Provider) {
			ingredients_list = establishment.logistics.GetProviderIngredientsList ();
		} else if(viwerType == IngredientViwer.Inventory) {
			ingredients_list = establishment.logistics.GetAquiredIngredientsList ();
		}
		if (ingredients_list == null) {
			Debug.LogError("Error getting the ingredients list");
			return;
		}

		foreach(Ingredient e in ingredients_list){
			IngredientCard card = Instantiate(ingredientCard);
			card.transform.SetParent(this.transform);
			card.transform.localScale = new Vector3(1,1,1);
			//TODO: find image by candidate name
			card.name.text = e.name.ToString();
			card.description.text = e.description.ToString();
			card.cost.text = e.cost.ToString();
			card.daysLeft.text = (establishment.GetStorageTime() - 
			                      (establishment.CurrentDay() - e.aquired_day)).ToString();
			cards.Add(card);
		}
	}	
}
