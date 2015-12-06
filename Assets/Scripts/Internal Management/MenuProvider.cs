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
		Price = d.Price;
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
	public double Price{
		get{
			return Mathf.Floor((float)price);
		}
		set{
			price = value;
		}
	}
	public List<string> ingredients;

	public void SaveObjectState(){
		EstablishmentManagement.SaveAttribute (name);
		EstablishmentManagement.SaveAttribute (id);
		EstablishmentManagement.SaveAttribute (description);
		EstablishmentManagement.SaveAttribute (nivel);
		EstablishmentManagement.SaveAttribute (price);
		EstablishmentManagement.SaveAttribute (available);
		EstablishmentManagement.SaveAttribute (ingredients.Count);
		foreach (string s in ingredients) {
			EstablishmentManagement.SaveAttribute (s);
		}
	}
	public void LoadObjectState(){
		EstablishmentManagement.LoadAttribute (out name);
		EstablishmentManagement.LoadAttribute (out id);
		EstablishmentManagement.LoadAttribute (out description);
		EstablishmentManagement.LoadAttribute (out nivel);
		EstablishmentManagement.LoadAttribute (out price);
		EstablishmentManagement.LoadAttribute (out available);
		int size;
		EstablishmentManagement.LoadAttribute (out size);
		for(int i=0; i<size; i++) {
			string s;
			EstablishmentManagement.LoadAttribute (out s);
			ingredients.Add(s);
		}
	}
}

public class MenuProvider{

	public List<Dish> dishes;
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
		double price;
		System.Double.TryParse(fields [3], out price);
		dish.Price = price;

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

	public Dish GetDishByID(int id){
		Dish dish = 
			dishes.Find (
				x => x.id == id);
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
			Debug.Log(dish.Price);

			foreach(string ingred in dish.ingredients){
				Debug.Log(ingred);
			}
			Debug.Log(dish.description);
		}
	}

	public void SaveObjectState(){
		EstablishmentManagement.SaveAttribute (dishes.Count);
		foreach(Dish dish in dishes){
			dish.SaveObjectState();
		}
	}
	
	public void LoadObjectState(){
		int size;
		dishes.Clear ();
		EstablishmentManagement.LoadAttribute (out size);
		for (int i=0; i<size; i++) {
			Dish dish = new Dish ();
			dish.LoadObjectState ();
			dishes.Add (dish);
		}
	}
}
