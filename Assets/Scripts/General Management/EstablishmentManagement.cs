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
			establishment_man = new EstablishmentManagement();

			if(!establishment_man.establishment.Initiate()){
				Debug.LogError("Could not initiate some essencial resources.");
				return null;
			}
		}
		return establishment_man;
	}

	public static void EraseInstance(){
		establishment_man = null;
	}


	Establishment e;
	MenuProvider menu;
	int number_of_requests;
	int requests_capacity;
	List<Dish> dishes_list;
	List<int> orders;
	List<int> attended;
	List<int> not_attended;
	int attended_count;

	System.Random rand = new System.Random ();

	public void DayPhaseSetup(){
		e = establishment_man.establishment;
		menu = MenuProvider.GetInstance ();
		if (menu == null) {
			Debug.LogError ("Failed to access the dishes menu");
			return;
		}
		
		number_of_requests = e.CalculateRequests ();
		requests_capacity = e.CalculateCapacity ();
		previous_day_cash = e.finances.StartingDayCash ();
		management_costs = e.Cash () - previous_day_cash;
		
		if (establishment.CurrentDay() <= 5) {
			dishes_list = menu.GetDishList ().FindAll (x => x.nivel == 1);
		}
		else if (establishment.CurrentDay() <= 15) {
			dishes_list = menu.GetDishList ().FindAll (x => x.nivel == 1 || x.nivel == 2);
		}
		else if (establishment.CurrentDay() > 15) {
			dishes_list = menu.GetDishList ();
		}
		
		orders = new List<int> ();
		for(int i=0; i<number_of_requests; i++){
			int index = rand.Next (0, dishes_list.Count);
			orders.Add(dishes_list[index].id);
		}
		attended_count = 0;
		day_phase_over = false;
		attended = new List<int> ();
		not_attended = new List<int>();
		time_per_order = initial_time_per_order;
		last_order_time = Time.time+4;
	}


	public void RunDayPhase(){
	
		if(paused){
			return;	
		}
		//Actual day phase loop
		if (attended_count > requests_capacity || orders.Count == 0) {
			day_phase_over = true;
			return;
		}

		//Create a wating time between orders 
		if (Time.time - last_order_time < time_per_order) {
			return;
		}

		GameLog.Log(GameLog.kTClientOrder, menu.GetDishByID(orders[0]).name);

		if(!e.IsOrderAvailable(orders[0])){
			GameLog.Log (GameLog.kTDishCannotBePrepared);
			GameLog.Log(GameLog.kTClientOrderNotAttended);
			not_attended.Add(orders[0]);
			e.DecreaseSatisfactionByOrder();
			orders.RemoveAt(0);
			last_order_time = Time.time;
			return;
		}
		if(!e.MakeOrder(orders[0])){
			GameLog.Log(GameLog.kTClientOrderNotAttended);
			not_attended.Add(orders[0]);
			e.DecreaseSatisfactionByOrder();
			orders.RemoveAt(0);
			last_order_time = Time.time;
			return;
		}

		GameLog.Log(GameLog.kTClientOrderAttended);
		attended.Add(orders[0]);
		e.IncreaseSatisfactionByOrder();
		orders.RemoveAt(0);
		attended_count++;
		last_order_time = Time.time;
		return;

	}

	public void CloseDayPhase(){
		if (orders.Count > 0) {
			GameLog.LogValueToken(orders.Count + " ", GameLog.kTClientsLeft);
		}
		time_per_order = initial_time_per_order;
		total_request_number = number_of_requests;
		attended_requests_number = attended_count;
		current_capacity = requests_capacity;
		day_income = e.Cash () - previous_day_cash - management_costs;
		e.NextDay ();
	}
	
	public void LocalSaveState(){
		SaveCacheGameData ();

		string dadosSaveBase64 = SaveDataToBase64();
		PlayerPrefs.SetString(
			UserService.Instance.userEmail + "_SaveBase64" + save_key, dadosSaveBase64);	
		PlayerPrefs.Save ();
		GameLog.Log(GameLog.kProgressSaved);
	}

	public void LocalLoadState(){
		string data = PlayerPrefs.GetString (UserService.Instance.userEmail + "_SaveBase64" + save_key);
		
		if (LoadDataFromBase64 (data)) {
			LoadCacheGameData ();
			GameLog.Log(GameLog.kProgressLoaded);
		}
	}

	public string SaveDataToBase64(){
		byte[] toEncodeAsBytes = Encoding.UTF8.GetBytes(save_game);
		string dadosSaveBase64 = System.Convert.ToBase64String(toEncodeAsBytes);
		return dadosSaveBase64;
	}

	public bool LoadDataFromBase64 (string data_base64){
		byte[] bytes = System.Convert.FromBase64String(data_base64);
		if (bytes.Count() == 0 || bytes.Count() > (512 * 1024)) {
			Debug.Log("The size of the loaded data is invalid.");
			return false;
		}
		load_game = Encoding.UTF8.GetString (bytes).Split ('\n');
		return true;
	}

	public void SaveCacheGameData(){
		save_game = "";
		SaveAttribute (management_costs);
		SaveAttribute (previous_day_cash);
		SaveAttribute (day_income);
		SaveAttribute (total_request_number);
		SaveAttribute (attended_requests_number);
		
		establishment.SaveObjectState ();
		AttributesManager.GetInstance ().SaveObjectState ();
	}

	public void LoadCacheGameData(){
		loaded_inter = 0;
		LoadAttribute (out management_costs);
		LoadAttribute (out previous_day_cash);
		LoadAttribute (out day_income);
		LoadAttribute (out total_request_number);
		LoadAttribute (out attended_requests_number);
		
		establishment.LoadObjectState ();
		AttributesManager.GetInstance ().LoadObjectState ();
	}

	public void Pause(){
		paused = true;
	}

	public void Resume(){
		paused = false;
	}
	public bool IsDayPhaseOver(){
		return day_phase_over;
	}

	public void FastPhaseTime(){
		time_per_order = 1;
	}

	public void SkipPhaseTime(){
		time_per_order = 0;
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
		if(!System.Int32.TryParse(load_game [loaded_inter], out attribute))
			Debug.Log(load_game [loaded_inter]);
		loaded_inter++;
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

	public static string save_key = "res_esq_fim_uni_sav";
	public static string save_game = "";
	public static string[] load_game;
	public static string save_game_base64;
	private static int loaded_inter;

	public double management_costs;
	public double previous_day_cash;
	public double day_income;
	public int total_request_number;
	public int attended_requests_number;
	public int current_capacity;

	private bool paused = false;
	private bool day_phase = false;
	private bool day_phase_over = true;

	private float initial_time_per_order = 4;
	private float time_per_order = 4;
	private float last_order_time = 0;

}
