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
		if (!LoadAttributes ()) {
			return false;
		}
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

	public void CleanOutOfDateIngredients(){
		foreach (Ingredient i in inventory) {
			if(storage_time - current_day - i.aquired_day <= 0){
				inventory.Remove(i);
			}
		}
	}

	public void NextDay(){
		current_day++;
	}

	private int current_day;
	public int CurrentDay{
		get{
			return current_day; 	
		}
	}
	public int StorageTime{
		get{
			return storage_time;
		}
		set{
			storage_time = value;
		}
	}
	public int StorageCapacity{
		get{
			return storage_capacity;
		}
		set{
			storage_capacity = value;
		}
	}
	public int InventoryCount{
		get{
			return inventory.Count;
		}
	}

	public static bool LoadAttributes(){
		AttributesManager at_m = AttributesManager.GetInstance ();
		if (at_m == null)
			return false;
		storage_time = at_m.IntValue ("storage_time");
		storage_capacity = at_m.IntValue ("inventory_capacity");
		return true;
	}
	
	private static int storage_time = 3;
	private static int storage_capacity = 20;
	private IngredientsProvider provider;
	private List<Ingredient> inventory;
}
