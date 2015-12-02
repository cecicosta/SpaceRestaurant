using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

public class EstablishmentManagement{ 
	public Establishment establishment;

	private EstablishmentManagement(){
		establishment = new Establishment ();
	
	}
	private static EstablishmentManagement establishment_man = null;
	public static EstablishmentManagement GetInstance(){
		if (establishment_man == null) {
			establishment_man = new EstablishmentManagement ();

			if(!establishment_man.establishment.Initiate()){
				Debug.LogError("Could not initiate some essencial resources.");
				return null;
			}
		}
		return establishment_man;
	}

	public void RunDailyPhase(){
		Establishment e = establishment_man.establishment;
		int number_of_requests = e.CalculateRequests ();
		int requests_capacity = e.CalculateCapacity ();

		MenuProvider menu = MenuProvider.GetInstance ();
		if (menu == null) {
			Debug.LogError ("Failed to access the dishes menu");
			return;
		}

		previous_day_cash = e.finances.StartingDayCash ();
		management_costs = e.Cash () - previous_day_cash;

		System.Random rand = new System.Random ();

		List<Dish> dishes_list = null;
		if (establishment.CurrentDay() <= 5) {
			dishes_list = menu.GetDishList ().FindAll (x => x.nivel == 1);
		}
		else if (establishment.CurrentDay() <= 15) {
			dishes_list = menu.GetDishList ().FindAll (x => x.nivel == 1 || x.nivel == 2);
		}
		else if (establishment.CurrentDay() > 15) {
			dishes_list = menu.GetDishList ();
		}

		List<int> orders = new List<int> ();
		for(int i=0; i<number_of_requests; i++){
			int index = rand.Next (0, dishes_list.Count);
			Debug.Log ("Prato: " + dishes_list[index].id);
			orders.Add(dishes_list[index].id);
		}

		int attended_count = 0;
		List<int> attended = new List<int> ();
		List<int> not_attended = new List<int>();
		while(attended_count < requests_capacity && orders.Count > 0 ) {
			if(!e.IsOrderAvailable(orders[0])){
				not_attended.Add(orders[0]);
				e.DecreaseSatisfactionByOrder();
				orders.RemoveAt(0);
				continue;
			}
			if(!e.MakeOrder(orders[0])){
				not_attended.Add(orders[0]);
				e.DecreaseSatisfactionByOrder();
				orders.RemoveAt(0);
				continue;
			}
			attended.Add(orders[0]);
			e.IncreaseSatisfactionByOrder();
			orders.RemoveAt(0);
			attended_count++;
		}

		if (attended_count < number_of_requests) {
			//TODO: Message
		}

		total_request_number = number_of_requests;
		attended_requests_number = attended_count;
		current_capacity = requests_capacity;
		day_income = e.Cash () - previous_day_cash - management_costs;
	
		e.NextDay ();
	}

	public double management_costs;
	public double previous_day_cash;
	public double day_income;
	public int total_request_number;
	public int attended_requests_number;
	public int current_capacity;
}
