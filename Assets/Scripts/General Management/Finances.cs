using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Finances {
	public int income;
	public int outgoing;

	public Finances(){
		cash = kInitialCash;
	}

	public bool Initiate(){
		menu = MenuProvider.GetInstance ();
		if (menu == null)
			return false;
		dishes = menu.GetDishList ();
		return dishes == null;
	}

	public bool IncreasePrice(string name){
		Dish dish = dishes.Find (x => x.name == name);
		if (dish == null)
			return false;
		dish.price += dish.price * kChangePricePercent; 
		return true;
	}

	public bool DecreasePrice(string name){
		Dish dish = dishes.Find (x => x.name == name);
		if (dish == null)
			return false;
		dish.price -= dish.price * kChangePricePercent; 
		return true;
	}

	private MenuProvider menu;
	private List<Dish> dishes; 
	private double cash;
	public double Cash {
		get{
			return cash;
		}
	}

	private const float kChangePricePercent = 0.1f; 
	private const int kInitialCash = 200;
}
