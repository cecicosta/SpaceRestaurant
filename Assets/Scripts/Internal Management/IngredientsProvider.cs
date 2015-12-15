using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Ingredient{
	public Ingredient(){
		available = true;
	}
	public Ingredient(Ingredient i){
		name = i.name;
		code = i.code;
		description = i.description;
		Cost = i.Cost;
		satisf_bonus = i.satisf_bonus;
		aquired_day = i.aquired_day;
		available = i.available;
	}

	public void SaveObjectState(){
		EstablishmentManagement.SaveAttribute (name);
		EstablishmentManagement.SaveAttribute (code);
		EstablishmentManagement.SaveAttribute (description);
		EstablishmentManagement.SaveAttribute (cost);
		EstablishmentManagement.SaveAttribute (satisf_bonus);
		EstablishmentManagement.SaveAttribute (aquired_day);
		EstablishmentManagement.SaveAttribute (available);
	}

	public void LoadObjectState(){
		EstablishmentManagement.LoadAttribute (out name);
		EstablishmentManagement.LoadAttribute (out code);
		EstablishmentManagement.LoadAttribute (out description);
		EstablishmentManagement.LoadAttribute (out cost);
		EstablishmentManagement.LoadAttribute (out satisf_bonus);
		EstablishmentManagement.LoadAttribute (out aquired_day);
		EstablishmentManagement.LoadAttribute (out available);
	}

	public string name;
	public string code;
	public string description;
	private double cost;
	public double Cost{
		get{
			return Mathf.Floor((float)cost);
		}
		set{
			cost = value;
		}
	}
	public int satisf_bonus;
	public int aquired_day;
	public bool available;
}

public class IngredientsProvider {
	public IngredientsProvider(){
		ingredients = new List<Ingredient> ();
	}
	public bool Initiate(){

		TextAsset bindata= Resources.Load("ingredients") as TextAsset;
		string ingredients_file = bindata.text;
		//string ingredients_file = System.IO.File.ReadAllText ("Assets/ingredients.txt");
		if(ingredients_file.CompareTo("") == 0){
			return false;
		}
		string[] lines = ingredients_file.Split('\n');
		foreach(string str in lines) {
			string[] fields = str.Split('\t');
			BuildIngredientsList(fields);
		}
		return true;
	}

	private void BuildIngredientsList(string[] fields){
		Ingredient ingredient = new Ingredient();
		ingredient.name = fields [0];
		ingredient.code = fields [1];
		double cost;
		System.Double.TryParse (fields [2], out cost);
		ingredient.Cost = cost;
		System.Int32.TryParse (fields [3], out ingredient.satisf_bonus);
		ingredient.description = fields [4];
		ingredients.Add (ingredient);
	}


	//Returns a copy of the employees list
	public List<Ingredient> GetIngredientsList(){
		List<Ingredient> copy = new List<Ingredient>();
		foreach (Ingredient i in ingredients) {
			Ingredient d_cpy = new Ingredient(i);
			copy.Add(d_cpy);
		}
		return copy;
	}
	
	public Ingredient GetIngredient(string name){
		Ingredient ingredient = 
			ingredients.Find (
				x => x.name == name);
		return ingredient;
	}
	
	public bool SetAvailable(string name, bool available){
		Ingredient ingredient = 
			ingredients.Find (
				x => x.name == name);
		if (ingredient == null)
			return false;
		
		ingredient.available = available;
		return true;
	}

	public void PrettyPrint(){
		foreach (Ingredient ing in ingredients) {
			Debug.Log(ing.name);
			Debug.Log(ing.code);
			Debug.Log(ing.Cost);
			Debug.Log(ing.satisf_bonus);
			Debug.Log(ing.description);
		}
	}

	public void SaveObjectState(){
		EstablishmentManagement.SaveAttribute (ingredients.Count);
		foreach(Ingredient i in ingredients){
			i.SaveObjectState();
		}
	}
	
	public void LoadObjectState(){
		int size;
		ingredients.Clear ();
		EstablishmentManagement.LoadAttribute (out size);
		for(int i=0; i<size; i++){
			Ingredient ing = new Ingredient();
			ing.LoadObjectState();
			ingredients.Add(ing);
		}
	}

	public List<Ingredient> ingredients;
}
