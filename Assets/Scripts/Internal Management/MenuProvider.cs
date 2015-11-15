using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Dish{
	public Dish(){
		available = true;
		ingredients = new List<string> ();
	}
	public string name;
	public int nivel;
	public int price;
	public string description;
	public bool available;
	public List<string> ingredients;

}

public class MenuProvider{
	public MenuProvider(){
		dishes = new List<Dish> ();
	}
	public bool Initiate(){
		string dishes_file = System.IO.File.ReadAllText ("Assets/dishes.txt");
		if(dishes_file.CompareTo("") == 0){
			return false;
		}
		string[] lines = dishes_file.Split('\n');
		foreach(string str in lines) {
			string[] fields = str.Split('\t');
			BuildDishesList(fields);
		}
		return true;
	}
	
	private void BuildDishesList(string[] fields){
		Dish dish = new Dish();
		dish.name = fields [0];
		System.Int32.TryParse (fields [1], out dish.nivel);
		System.Int32.TryParse (fields [2], out dish.price);

		string[] ingred = fields [3].Split (',');
		foreach (string str in ingred) {
			dish.ingredients.Add(str);
		}

		dish.description = fields [4];
		dishes.Add (dish);
	}
	
	public void PrettyPrint(){
		foreach (Dish dish in dishes) {
			Debug.Log (dish.name);
			Debug.Log(dish.nivel);
			Debug.Log(dish.price);

			foreach(string ingred in dish.ingredients){
				Debug.Log(ingred);
			}
			Debug.Log(dish.description);
		}
	}
	public List<Dish> dishes;
}
