using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;


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

			GameLog.Log(GameLog.kTClientOrder, menu.GetDishByID(orders[0]).name);

			if(!e.IsOrderAvailable(orders[0])){
				GameLog.Log(GameLog.kTClientOrderNotAttended);
				not_attended.Add(orders[0]);
				e.DecreaseSatisfactionByOrder();
				orders.RemoveAt(0);
				continue;
			}
			if(!e.MakeOrder(orders[0])){
				GameLog.Log(GameLog.kTClientOrderNotAttended);
				not_attended.Add(orders[0]);
				e.DecreaseSatisfactionByOrder();
				orders.RemoveAt(0);
				continue;
			}

			GameLog.Log(GameLog.kTClientOrderAttended);
			attended.Add(orders[0]);
			e.IncreaseSatisfactionByOrder();
			orders.RemoveAt(0);
			attended_count++;
		}

		if (orders.Count > 0) {
			GameLog.Log(orders.Count.ToString(), GameLog.kTClientsLeft);
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

	public static string save_key = "res_esq_fim_uni_sav";
	public static string save_game = "";
	public static string[] load_game;
	private static int loaded_inter;
	public void SaveGameState(){
		SaveAttribute (management_costs);
		SaveAttribute (previous_day_cash);
		SaveAttribute (day_income);
		SaveAttribute (total_request_number);
		SaveAttribute (attended_requests_number);

		establishment.SaveObjectState ();
		AttributesManager.GetInstance ().SaveObjectState ();

		byte[] toEncodeAsBytes = Encoding.UTF8.GetBytes(save_game);
		string dadosSaveBase64 = System.Convert.ToBase64String(toEncodeAsBytes);
		PlayerPrefs.SetString(
			UserService.Instance.userEmail + "_SaveBase64" + save_key, dadosSaveBase64);
	
		PlayerPrefs.Save ();
	}

	public void LoadGameState(){
		loaded_inter = 0;
		byte[] bytes = System.Convert.FromBase64String(PlayerPrefs.GetString (
			UserService.Instance.userEmail + "_SaveBase64" + save_key));
		load_game = Encoding.UTF8.GetString (bytes).Split ('\n');

		LoadAttribute (out management_costs);
		LoadAttribute (out previous_day_cash);
		LoadAttribute (out day_income);
		LoadAttribute (out total_request_number);
		LoadAttribute (out attended_requests_number);

		establishment.LoadObjectState ();
		AttributesManager.GetInstance ().LoadObjectState ();
	}

	public static void SaveAttribute(int attribute){
		save_game += attribute.ToString () + "\n";
	}
	public static void SaveAttribute(float attribute){
		save_game += attribute.ToString () + "\n";
	}
	public static void SaveAttribute(double attribute){
		save_game += attribute.ToString () + "\n";
	}
	public static void SaveAttribute(bool attribute){
		save_game += attribute.ToString () + "\n";
	}
	public static void SaveAttribute(string attribute){
		save_game += attribute + "\n";
	}

	public static void LoadAttribute(out int attribute){
		attribute = System.Int32.Parse(load_game [loaded_inter++]);
	}
	public static void LoadAttribute(out float attribute){
		attribute = (float)System.Double.Parse(load_game [loaded_inter++]);
	}
	public static void LoadAttribute(out double attribute){
		attribute = System.Double.Parse(load_game [loaded_inter++]);
	}
	public static void LoadAttribute(out bool attribute){
		System.Boolean.TryParse(load_game [loaded_inter++], out attribute);
	}
	public static void LoadAttribute(out string attribute){
		attribute = load_game [loaded_inter++];
	}
	
	public double management_costs;
	public double previous_day_cash;
	public double day_income;
	public int total_request_number;
	public int attended_requests_number;
	public int current_capacity;
}
