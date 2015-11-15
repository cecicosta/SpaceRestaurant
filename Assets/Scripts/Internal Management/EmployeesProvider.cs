using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Candidate{
	public Candidate(){
		available = true;
		dishes = new List<int> ();
	}
	public string name;
	public string type;
	public string description;
	public int level;
	public int cost;
	public bool available;
	public List<int> dishes;

	public void Print(){
		Debug.Log (name);
		Debug.Log(type);
		Debug.Log(level);
		foreach(int dish in dishes){
			Debug.Log(dish);
		}
		Debug.Log(cost);
		Debug.Log(description);
	}
}

public class EmployeesProvider{
	public EmployeesProvider(){
		candidates = new List<Candidate> ();
	}
	public bool Initiate(){
		string employees = System.IO.File.ReadAllText ("Assets/employees.txt");
		if(employees.CompareTo("") == 0){
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
		Candidate candidate = new Candidate();
		candidate.name = fields [0];
		candidate.type = fields [1];
		candidate.description = fields [5];
		System.Int32.TryParse (fields [2],out candidate.level);
		System.Int32.TryParse (fields [4],out candidate.cost);

		string[] dishes = fields [3].Split (',');
		foreach (string str in dishes) {
			int dish = 0;
			System.Int32.TryParse (str, out dish);
			candidate.dishes.Add(dish);
		}

		candidates.Add (candidate);
	}

	public Candidate GetCandidate(string name){
		Candidate candidate = 
			candidates.Find (
				x => x.name == name);
		return candidate;
	}

	public bool SetAvailable(string name, bool available){
		Candidate candidate = 
			candidates.Find (
				x => x.name == name);
		if (candidate == null)
			return false;

		candidate.available = available;
		return true;
	}

	public void PrettyPrint(){
		foreach (Candidate cand in candidates) {
			cand.Print();
		}
	}
	
	public List<Candidate> candidates;
}
