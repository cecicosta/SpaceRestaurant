using UnityEngine;
using System.Collections;
using System.Collections.Generic;

//The stablishment adds a layer to the functionalities of the basic structure.
//Some operation as hiring an employee may depends on information from other
//structures as the finances to evaluate if there is budget enough to hire
//the employee.

public class Establishment{

	public AlianResources alien_resources;
	public Marketing marketing;
	public Finances finances;
	public Logistics logistics;
	public Infrastructure infrastructure;

	public int action_points;
	public int reaction_points;

	public Establishment(){
		generator = new System.Random ();
		alien_resources = new AlianResources ();
		marketing = new Marketing ();
		finances = new Finances ();
		logistics = new Logistics ();
		infrastructure = new Infrastructure ();
	}

	public bool Initiate(){
		if (!LoadAttributes ()) {
			Debug.LogError("failed to initiate alien resources");
			return false;
		}
		if (!alien_resources.Initiate ()) {
			Debug.LogError("failed to initiate alien resources");
			return false;
		}
		if (!marketing.Initiate ()) {
			Debug.LogError("failed to initiate marketing");
			return false;
		}
		if (!finances.Initiate ()) {
			Debug.LogError("failed to initiate finances");
			return false;
		}
		if (!logistics.Initiate ()) {
			Debug.LogError("failed to initiate logistics");
			return false;
		}
		if (!infrastructure.Initiate ()) {
			Debug.LogError("failed to initiate infrastructure");
			return false;
		}
		return true;
	}

	//Primary status group
	public double Cash(){
		return finances.Cash;
	}
	public int Dirtiness(){
		return infrastructure.Dirtiness;
	}
	public int Satisfaction(){
		return marketing.Satisfaction;
	}

	//Calculate volume of requests and capacity
	public int CalculateCapacity(){
		List<Employee> waiters = alien_resources.GetEmployeesOfType (Employee.Type.Waiter);
		int capacity = 0;
		foreach(Employee e in waiters){
			capacity += e.Capacity;
		}
		return capacity;
	}
	public int CalculateRequests(){
		int factor = requestCalcFactorRange[generator.Next (0, requestCalcFactorRange.Length)];
		return marketing.Satisfaction / factor;
	}

	//Alien Resources Options
	public bool Hire(string name){
		List<Employee> candidates_list = alien_resources.GetCandidatesList ();
		Employee candidate = candidates_list.Find (x => x.name == name);
		if (candidate == null)
			return false;
		if (finances.Cash - candidate.HireCosts < 0)
			return false;
		if (!alien_resources.HireEmployee (candidate.name))
			return false;
		finances.Cash -= candidate.HireCosts;

		return true;
	}
	public bool TrainLevel(string name){
		List<Employee> employees_list = alien_resources.GetEmployeesList ();
		Employee employee = employees_list.Find (x => x.name == name);
		if (employee == null)
			return false;
		if (alien_resources.WasTrained (name))
			return false;
		if (finances.Cash - employee.TrainSkillCosts < 0)
			return false;

		double train_costs = employee.TrainSkillCosts;
		if (!alien_resources.TrainEmployeeLevel (employee.name))
			return false;
		finances.Cash -= train_costs;

		return true;
	}
	public bool TrainHappiness(string name){
		List<Employee> employees_list = alien_resources.GetEmployeesList ();
		Employee employee = employees_list.Find (x => x.name == name);
		if (employee == null)
			return false;
		if (alien_resources.WasTrained (name))
			return false;
		if (finances.Cash - employee.TrainHappinessCosts < 0)
			return false;

		double train_costs = employee.TrainSkillCosts;
		if (!alien_resources.TrainEmployeeHapyness (employee.name))
			return false;
		finances.Cash -= train_costs;
		
		return true;
	}
	public bool Dismiss(string name){
		List<Employee> employees_list = alien_resources.GetEmployeesList ();
		Employee employee = employees_list.Find (x => x.name == name);
		if (employee == null)
			return false;
		if (finances.Cash - employee.DismissCosts < 0)
			return false;
		if (!alien_resources.DismissEmployee (employee.name))
			return false;
		finances.Cash -= employee.DismissCosts;

		return true;
	}

	public bool BuyIngredient(string name){
		Debug.Log (finances.Cash);
		List<Ingredient> ingredients_list = logistics.GetProviderIngredientsList ();
		Ingredient ingredient = ingredients_list.Find (x => x.name == name);
		if (ingredient == null)
			return false;
		if (finances.Cash - ingredient.cost < 0)
			return false;
		if (logistics.InventoryCount >= GetStorageCapacity ()) 
			return false;
		if (!logistics.AquireIngredient (ingredient.name))
			return false;
		finances.Cash -= ingredient.cost;

		return true;
	}
	public bool SpendIngredient(string name){
		return logistics.SpendIngredient(name);
	}

	//Finances Options
	//Returns the financy status and balance clients 
	public void GeneralBalance(){
		//TODO:
	}
	public void IncreasePrices(string dish){
		finances.IncreasePrice (dish);
		marketing.Satisfaction -= satisfactionIncrement;
	}
	public void DecreasePrices(string dish){
		finances.DecreasePrice (dish);
		marketing.Satisfaction += satisfactionIncrement;
	}

	//Logistics
	public int CurrentDay(){
		return logistics.CurrentDay;
	}
	public void NextDay(){
		logistics.NextDay ();
		logistics.CleanOutOfDateIngredients ();
		alien_resources.ClearTrained ();
		marketing.ClearHiredAds ();
		RestorePoints ();
	}
	public int GetStorageTime(){
		//TODO: some equipments can add to the storage time
		//TODO: some situations cam subtract from the storage time
		return logistics.StorageTime;
	}
	public int GetStorageCapacity(){
		//TODO: some equipments can add to the storage time
		//TODO: some situations cam subtract from the storage time
		return logistics.StorageCapacity;
	}

	//Marketing 
	public bool HireAdvertisement(string type){
		List<Advertising> ads_list = marketing.GetAdvertisementsList ();
		Advertising advertisement = ads_list.Find (x => x.type == type);
		if (advertisement == null)
			return false;
		if (marketing.WasHired (type))
			return false;
		if (finances.Cash - advertisement.price < 0)
			return false;
		if (!marketing.HireAdvertisement (type))
			return false;
		finances.Cash -= advertisement.price;
		
		return true;
	}

	//Infrastructure
	public bool BuyEquipment(string name){
		List<Equipment> equips_list = infrastructure.GetProviderEquipmentsList ();
		Equipment equipment = equips_list.Find (x => x.name == name);
		if (equipment == null)
			return false;
		if (finances.Cash - equipment.price < 0)
			return false;
		if (!infrastructure.BuyEquipment (name))
			return false;
		finances.Cash -= equipment.price;
	
		//TODO: Update All Attributes and modifiers
		return true;
	}

	//Establishment
	public void RestorePoints(){
		action_points = initialActionPoints;
		reaction_points = initialReactionPoints;
	}
	public bool ConvertActionPointsToResponsePoint(){
		if (action_points - actionReationConvertion < 0)
			return false;
		action_points -= actionReationConvertion;
		reaction_points++;
		return true;
	}

	public static bool LoadAttributes(){
		AttributesManager at_m = AttributesManager.GetInstance ();
		if (at_m == null)
			return false;
		initialActionPoints = at_m.IntValue ("action_points");
		initialReactionPoints = at_m.IntValue ("reaction_points");
		actionReationConvertion = at_m.IntValue ("action_reaction_convertion");
		satisfactionIncrement = at_m.IntValue ("satisfaction_increment");
		requestCalcFactorRange = at_m.RangeValue ("requests_calc_factor_range");
		return true;
	}

	private static int initialReactionPoints;
	private static int initialActionPoints;
	private static int actionReationConvertion;
	private static int satisfactionIncrement; //Incremental value of satisfaction over price changes
	private static int[] requestCalcFactorRange;
	private System.Random generator;
}
