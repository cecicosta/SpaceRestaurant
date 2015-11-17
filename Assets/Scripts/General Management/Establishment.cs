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

	private const int kCapacityMultiplier = 3;
	private System.Random generator;
}
