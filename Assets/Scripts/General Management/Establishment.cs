using UnityEngine;
using System.Collections;
using System.Collections.Generic;

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
		if (candidate != null)
			return false;
		if (!alien_resources.HireEmployee (candidate.name))
			return false;
		finances.Cash -= candidate.cost;

		return true;
	}
	public bool TrainLevel(string name){
		List<Employee> employees_list = alien_resources.GetEmployeesList ();
		Employee employee = employees_list.Find (x => x.name == name);
		if (employee != null)
			return false;
		if (!alien_resources.TrainEmployeeLevel (employee.name))
			return false;
		finances.Cash -= (employee.level+1)*kTrainCostMultiplier;

		return true;
	}
	public bool TrainHappyness(string name){
		List<Employee> employees_list = alien_resources.GetEmployeesList ();
		Employee employee = employees_list.Find (x => x.name == name);
		if (employee != null)
			return false;
		if (!alien_resources.TrainEmployeeHapyness (employee.name))
			return false;
		finances.Cash -= (employee.level+1)*kTrainCostMultiplier;
		
		return true;
	}
	public bool Dismiss(string name){
		List<Employee> employees_list = alien_resources.GetEmployeesList ();
		Employee employee = employees_list.Find (x => x.name == name);
		if (employee != null)
			return false;
		if (!alien_resources.DismissEmployee (employee.name))
			return false;
		finances.Cash -= employee.level * kDismissCostMultiplier;

		return true;
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
	public void NextDay(){
		logistics.NextDay ();
		alien_resources.ClearTrained (kSatisfactionCost);
	}

	private const int kSatisfactionCost = 5;
	private const int kDismissCostMultiplier = 12;
	private const int kTrainCostMultiplier = 10;
	private const int kCapacityMultiplier = 3;
	private System.Random generator;
}
