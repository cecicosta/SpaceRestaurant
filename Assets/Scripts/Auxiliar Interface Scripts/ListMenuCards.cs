using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class ListMenuCards : MonoBehaviour {

	public MenuCard menuCard;
	private List<MenuCard> cards = new List<MenuCard> ();
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
		EventManager.OnUpdateMenu += CreateCards;
	}
	
	void OnDisable(){
		EventManager.OnUpdateMenu -= CreateCards;
	}
	
	void CreateCards(){
		if(cards.Count != 0){
			foreach(MenuCard m in cards){
				Destroy(m.gameObject);
			}
			cards.Clear();
		}
		MenuProvider m_provider = MenuProvider.GetInstance ();
		if (m_provider == null) {
			Debug.LogError("Error accessing menu");
			return;
		}

		List<Dish> menu_list = null;
		menu_list = m_provider.GetDishList ();
		if (menu_list == null) {
			Debug.LogError("Error getting the menu list");
			return;
		}

		int i = 0;
		foreach(Dish dish in menu_list){
			MenuCard card = Instantiate(menuCard);
			card.transform.SetParent(this.transform);
			//TODO: find image by candidate name
			card.number.text = i.ToString();
			card.name.text = dish.name.ToString();
			card.description.text = dish.description.ToString();
			card.price.text = dish.price.ToString();
			card.nivel.text = dish.nivel.ToString();
			string ing_name = "";
			foreach(string s in dish.ingredients){
				Ingredient ing = establishment.logistics.GetProviderIngredientFromCode(s);
				ing_name += ", " + ing.name;
			}
			card.ingredients.text = ing_name;
			cards.Add(card);
			i++;
		}
	}	
}
