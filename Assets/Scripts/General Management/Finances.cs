using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Finances {
	public int income;
	public int outgoing;

	public Finances(){
		cash = initialCash;
	}

	public bool Initiate(){
		if (!LoadAttributes ()) {
			return false;
		}
		menu = MenuProvider.GetInstance ();
		if (menu == null)
			return false;
		dishes = menu.GetDishList ();
		return dishes != null;
	}

	public bool IncreasePrice(string name){
		Dish dish = dishes.Find (x => x.name == name);
		if (dish == null)
			return false;
		dish.price += dish.price * changePricePercent; 
		return true;
	}

	public bool DecreasePrice(string name){
		Dish dish = dishes.Find (x => x.name == name);
		if (dish == null)
			return false;
		dish.price -= dish.price * changePricePercent; 
		return true;
	}

	//Cash control
	private double cash;
	public double Cash {
		get{
			return cash;
		}
		set{
			cash = value;
		}
	}

	private MenuProvider menu;
	private List<Dish> dishes; 

	public static bool LoadAttributes(){
		AttributesManager at_m = AttributesManager.GetInstance ();
		if (at_m == null)
			return false;
		changePricePercent = at_m.DoubleValue ("initial_cash");
		initialCash = at_m.DoubleValue ("food_price_increment");
		return true;
	}
	
	private static double changePricePercent = 0.1; 
	private static double initialCash = 200;
}
