﻿using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

public class Employee{
	public Employee(){
		dishes = new List<int> ();
		happiness = initialHappiness;
	}
	//Copy constructor
	public Employee(Employee c){
		name = c.name;
		type = c.type;
		HireCosts = c.HireCosts;
		level = c.level;
		happiness = c.happiness;
		description = c.description;
		id = c.id;
		dishes = new List<int> ();
		foreach (int d in c.dishes) {
			dishes.Add(d);
		}
	}
	
	public double HireCosts{
		get{
			return Mathf.Floor((float)hire_costs);
		}
		set{
			hire_costs = value;
		}
	}
	public double DismissCosts{
		get{
			return level * dismissCostMultiplier;
		}
	}
	public double TrainSkillCosts{
		get{
			return (level+1) * trainCostMultiplier;
		}
	}
	public double TrainHappinessCosts{
		get{
			return (happiness+1) * trainCostMultiplier;
		}
	}
	public double Salary{
		get{
			return level * salaryMultiplier;
		}
	}
	public int Capacity{
		get{
			return level * capacityMultiplier;
		}
	}

	public static Type StringToType(string t){
		Type tp = Type.chef;
		switch (t) {
		case "chef":
			tp = Employee.Type.chef;
			break;
		case "waiter":
			tp = Employee.Type.waiter;
			break;
		case "marketing":
			tp = Employee.Type.marketing;
			break;
		case "finances":
			tp = Employee.Type.finances;
			break;
		}
		return tp;
	}
	
	public static bool LoadAttributes(){

		AttributesManager at_m = AttributesManager.GetInstance ();
		if (at_m == null)
			return false;
		capacityMultiplier = at_m.IntValue("capacity_multiplier");
		trainCostMultiplier = at_m.IntValue ("train_cost_multiplier");
		dismissCostMultiplier = at_m.IntValue("dismiss_multiplier");
		salaryMultiplier = at_m.IntValue("salary_multiplier");
		initialHappiness = at_m.IntValue("initial_happiness");
		return true;
	}

	public void SaveObjectState(){
		EstablishmentManagement.SaveAttribute (name);
		EstablishmentManagement.SaveAttribute (description);
		EstablishmentManagement.SaveAttribute (type.ToString());
		EstablishmentManagement.SaveAttribute (level);
		EstablishmentManagement.SaveAttribute (happiness);
		EstablishmentManagement.SaveAttribute (hire_costs);
		EstablishmentManagement.SaveAttribute (id);
		EstablishmentManagement.SaveAttribute (dishes.Count);
		foreach (int d in dishes) {
			EstablishmentManagement.SaveAttribute(d);
		}
	}

	public void LoadObjectState(){
		EstablishmentManagement.LoadAttribute (out name);
		EstablishmentManagement.LoadAttribute (out description);

		string t;
		EstablishmentManagement.LoadAttribute (out t);
		type = Employee.StringToType (t);

		EstablishmentManagement.LoadAttribute (out level);
		EstablishmentManagement.LoadAttribute (out happiness);
		EstablishmentManagement.LoadAttribute (out hire_costs);
		EstablishmentManagement.LoadAttribute (out id);

		dishes.Clear();
		int count;
		EstablishmentManagement.LoadAttribute (out count);
		for (int i=0; i<count; i++) {
			int d;
			EstablishmentManagement.LoadAttribute(out d);
			dishes.Add(d);
		}
	}
	
	public string name;
	public string description;
	public Type type;
	public int level;
	public int happiness;
	public double hire_costs;
	public int id;
	public List<int> dishes;
	public enum Type{chef, waiter, marketing, finances};

	private static int capacityMultiplier;
	private static int trainCostMultiplier;
	private static int dismissCostMultiplier;
	private static int salaryMultiplier;
	private static int initialHappiness;
}

public class EmployeesProvider{
	public EmployeesProvider(){
		candidates = new List<Employee> ();
	}

	public bool Initiate(){


		TextAsset bindata= Resources.Load("employees") as TextAsset;
		string employees = bindata.text;

		//string employees = System.IO.File.ReadAllText ("Assets/employees.txt");
		if(employees.CompareTo("") == 0){
			return false;
		}
		if (!Employee.LoadAttributes ()) {
			return false;
		}

		string[] lines = employees.Split('\n');
		foreach(string str in lines) {
			string[] fields = str.Split('\t');
			BuildCandidatesList(fields);
		}
		return true;
	}

	public void BuildCandidatesList(string[] fields){
		Employee candidate = new Employee();
		candidate.name = fields [0];

		switch (fields [1]) {
		case "chef":
			candidate.type = Employee.Type.chef;
			break;
		case "waiter":
			candidate.type = Employee.Type.waiter;
			break;
		case "marketing":
			candidate.type = Employee.Type.marketing;
			break;
		case "finances":
			candidate.type = Employee.Type.finances;
			break;
		}

		System.Int32.TryParse (fields [2],out candidate.level);
		double cost = 0;
		System.Double.TryParse (fields [4],out cost);
		candidate.HireCosts = cost;

		candidate.description = fields [5];

		string[] dishes = fields [3].Split (',');
		foreach (string str in dishes) {
			int dish = 0;
			System.Int32.TryParse (str, out dish);
			candidate.dishes.Add(dish);
		}
		candidate.id = candidates.Count;
		candidates.Add (candidate);
	}

	//Returns a copy of the employees list
	public List<Employee> GetCandidatesList(){
		List<Employee> copy = new List<Employee>();
		foreach (Employee c in candidates) {
			Employee c_cpy = new Employee(c);
			copy.Add(c_cpy);
		}
		return copy;
	}

	public List<Employee> GetCandidatesOfType(string type){
		return candidates.FindAll(x => x.type.ToString() == type);
	}

	public List<Employee> GetCandidatesOfType(Employee.Type type){
		return candidates.FindAll(x => x.type == type);
	}

	//Returns a reference for a candidate
	public Employee GetCandidate(string name){
		Employee candidate = 
			candidates.Find (
				x => x.name == name);
		return candidate;
	}

	public bool RemoveCandidate(Employee c){
		Employee existing = candidates.Find(x => x.name == c.name);
		if (existing == null)
			return false;
		return candidates.Remove (existing);
	}

	public bool AddCandidate(Employee c){
		Employee existing = candidates.Find(x => x.name == c.name);
		if (existing != null)
			return false;
		candidates.Add (c);
		return true;
	}

	public void SaveObjectState(){
		EstablishmentManagement.SaveAttribute (candidates.Count);
		foreach(Employee e in candidates){
			e.SaveObjectState();
		}
	}

	public void LoadObjectState(){
		candidates.Clear ();
		int size;
		EstablishmentManagement.LoadAttribute (out size);
		for (int i=0; i<size; i++) {
			Employee e = new Employee();
			e.LoadObjectState();
			candidates.Add(e);
		}
	}

	private List<Employee> candidates;
}
