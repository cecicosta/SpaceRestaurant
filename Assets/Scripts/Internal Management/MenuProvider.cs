using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Dish{
	public Dish(){
		available = true;
		ingredients = new List<string> ();
	}
	public Dish(Dish d){
		name = d.name;
		id = d.id;
		nivel = d.nivel;
		price = d.price;
		description = d.description;
		available = d.available;
		ingredients = new List<string> ();
		foreach (string i in d.ingredients) {
			ingredients.Add(i);
		}
	}
	public string name;
	public int id;
	public string description;
	public int nivel;
	public bool available;
	public double price;
	public List<string> ingredients;
}

public class MenuProvider{

	private static MenuProvider menu_provider;
	private MenuProvider(){
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

	public static MenuProvider GetInstance(){
		if (menu_provider == null) {
			menu_provider = new MenuProvider();
			if(!menu_provider.Initiate()){
				Debug.LogError("Failed to load menu");
				menu_provider = null;
			}
		}
		return menu_provider;
	}
	
	private void BuildDishesList(string[] fields){
		Dish dish = new Dish();
		dish.name = fields [0];
		System.Int32.TryParse (fields [1], out dish.id);
		System.Int32.TryParse (fields [2], out dish.nivel);
		System.Double.TryParse(fields [3], out dish.price);

		string[] ingred = fields [4].Split (',');
		foreach (string str in ingred) {
			dish.ingredients.Add(str);
		}

		dish.description = fields [5];
		dishes.Add (dish);
	}

	//Returns a copy of the employees list
	public List<Dish> GetDishList(){
		List<Dish> copy = new List<Dish>();
		foreach (Dish d in dishes) {
			Dish d_cpy = new Dish(d);
			copy.Add(d_cpy);
		}
		return copy;
	}

	public Dish GetDish(string name){
		Dish dish = 
			dishes.Find (
				x => x.name == name);
		return dish;
	}

	public bool SetAvailable(string name, bool available){
		Dish dish = 
			dishes.Find (
				x => x.name == name);
		if (dish == null)
			return false;
		
		dish.available = available;
		return true;
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
