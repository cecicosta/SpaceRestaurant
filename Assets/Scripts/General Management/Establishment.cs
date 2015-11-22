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
	public int response_points;

	public Establishment(){
		generator = new System.Random ();
		alien_resources = new AlianResources ();
		marketing = new Marketing ();
		finances = new Finances ();
		logistics = new Logistics ();
		infrastructure = new Infrastructure ();
	}

	public bool Initiate(){
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
			capacity += e.level*kCapacityMultiplier;
		}
		return capacity;
	}
	public int CalculateRequests(){
		int[] request_factors = {8,10,12};
		int factor = request_factors[generator.Next (0, 2)];
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
		if (finances.Cash - (employee.level + 1) * kTrainCostMultiplier < 0)
			return false;
		if (!alien_resources.TrainEmployeeLevel (employee.name))
			return false;
		finances.Cash -= (employee.level+1)*kTrainCostMultiplier;

		return true;
	}
	public bool TrainHappiness(string name){
		List<Employee> employees_list = alien_resources.GetEmployeesList ();
		Employee employee = employees_list.Find (x => x.name == name);
		if (employee == null)
			return false;
		if (alien_resources.WasTrained (name))
			return false;
		if (finances.Cash - (employee.level + 1) * kTrainCostMultiplier < 0)
			return false;
		if (!alien_resources.TrainEmployeeHapyness (employee.name))
			return false;
		finances.Cash -= (employee.level+1)*kTrainCostMultiplier;
		
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
		marketing.Satisfaction -= kSatisfactionCost;
	}
	public void DecreasePrices(string dish){
		finances.DecreasePrice (dish);
		marketing.Satisfaction += kSatisfactionCost;
	}

	//Logistics
	public int CurrentDay(){
		return logistics.Day;
	}
	public void NextDay(){
		logistics.NextDay ();
		alien_resources.ClearTrained ();
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
	
	private const int kSatisfactionCost = 5;
	private const int kTrainCostMultiplier = 10;
	private const int kCapacityMultiplier = 3;
	private System.Random generator;
}
