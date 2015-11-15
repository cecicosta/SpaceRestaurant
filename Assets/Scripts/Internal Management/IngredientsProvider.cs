using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Ingredient{
	public Ingredient(){
		available = true;
	}
	public string name;
	public string code;
	public string description;
	public int cost;
	public int satisf_bonus;
	public bool available;
}

public class IngredientsProvider {
	public IngredientsProvider(){
		ingredients = new List<Ingredient> ();
	}
	public bool Initiate(){
		string ingredients_file = System.IO.File.ReadAllText ("Assets/ingredients.txt");
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
		System.Int32.TryParse (fields [2], out ingredient.cost);
		System.Int32.TryParse (fields [3], out ingredient.satisf_bonus);
		ingredient.description = fields [4];
		ingredients.Add (ingredient);
	}

	public void PrettyPrint(){
		foreach (Ingredient ing in ingredients) {
			Debug.Log(ing.name);
			Debug.Log(ing.code);
			Debug.Log(ing.cost);
			Debug.Log(ing.satisf_bonus);
			Debug.Log(ing.description);
		}
	}
	public List<Ingredient> ingredients;
}
