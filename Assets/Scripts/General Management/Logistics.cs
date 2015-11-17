using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Logistics {

	public Logistics(){
		provider = new IngredientsProvider ();
		inventory = new List<Ingredient> ();
		current_day = 0;
	}

	public bool Initiate(){
		return provider.Initiate ();
	}

	public bool AquireIngredient(string name){
		Ingredient ingredient = provider.GetIngredient (name);
		if (ingredient == null)
			return false;
		Ingredient i_cpy = new Ingredient (ingredient);
		i_cpy.aquired_day = current_day;
		inventory.Add (i_cpy);
		return true;
	}

	public bool SpendIngredient(string name){
		Ingredient ingredient = inventory.Find (x => x.name == name);
		if (ingredient == null)
			return false;
		return inventory.Remove (ingredient);
	}

	public string GetIngredientCodeFromName(string name){
		Ingredient ingredient = provider.GetIngredient (name);
		if (ingredient == null)
			return "";
		return ingredient.code;
	}

	public string GetIngredientNameFromCode(string code){
		List<Ingredient> ingredients = provider.GetIngredientsList();
		if (ingredients == null)
			return "";
		Ingredient ingredient = ingredients.Find (x => x.code == code);
		if (ingredient == null)
			return  "";
		return ingredient.name;
	}

	public List<Ingredient> GetAquiredIngredientsList(){
		List<Ingredient> copy = new List<Ingredient>();
		foreach (Ingredient i in inventory) {
			Ingredient i_cpy = new Ingredient(i);
			copy.Add(i_cpy);
		}
		return copy;
	}

	public List<Ingredient> GetProviderIngredientsList(){
		return provider.GetIngredientsList ();
	}

	public Ingredient GetIngredientFromName(string name){
		return inventory.Find (x => x.name == name);
	}

	public Ingredient GetIngredientFromCode(string code){
		return inventory.Find (x => x.code == code);
	}

	public void NextDay(){
		current_day++;
	}

	private int current_day;
	public int Day{
		get{
			return current_day; 	
		}
	}
	private IngredientsProvider provider;
	private List<Ingredient> inventory;
}
