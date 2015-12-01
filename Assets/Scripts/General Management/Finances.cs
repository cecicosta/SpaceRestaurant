using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Finances {
	public int income;
	public int outgoing;

	public Finances(){
	}

	public bool Initiate(){
		if (!LoadAttributes ()) {
			return false;
		}
		cash = initialCash;
		starting_day_cash = cash;
		menu = MenuProvider.GetInstance ();
		if (menu == null)
			return false;
		return true;
	}

	public bool IncreasePrice(string name){
		Dish dish = menu.GetDish (name);
		if (dish == null)
			return false;
		dish.price += dish.price * changePricePercent; 
		return true;
	}

	public bool DecreasePrice(string name){
		Dish dish = menu.GetDish (name);
		if (dish == null)
			return false;
		dish.price -= dish.price * changePricePercent; 
		return true;
	}


	public void CloseDayBalance(){
		starting_day_cash = cash;
	}

	//Cash control
	private double starting_day_cash;
	public double StartingDayCash(){
		return starting_day_cash;
	}
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

	public static bool LoadAttributes(){
		AttributesManager at_m = AttributesManager.GetInstance ();
		if (at_m == null)
			return false;
		changePricePercent = at_m.DoubleValue ("food_price_increment");
		initialCash = at_m.DoubleValue ("initial_cash");
		Debug.Log (initialCash);
		return true;
	}
	
	private static double changePricePercent; 
	private static double initialCash;
}
