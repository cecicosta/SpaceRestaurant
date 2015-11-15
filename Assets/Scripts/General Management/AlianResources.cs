using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

public class AlianResources{
	public AlianResources(){
		employee_provider = new EmployeesProvider ();
		employees = new List<Employee> ();
	}

	public bool Initiate(){
		if (employee_provider == null)
			return false;
		return employee_provider.Initiate ();
	}

	public bool HireEmployee(string name){
		if (!employee_provider.SetAvailable (name, false)) {
			return false;
		}
		Candidate candidate = employee_provider.GetCandidate (name);
		if (candidate == null) {
			return false;
		}

		Employee employee = new Employee ();
		employee.name = candidate.name;
		employee.level = candidate.level;
		employee.salary = candidate.level * kSalaryMultiplier;
		switch (candidate.type) {
		case "chef":
			employee.type = Employee.Occupation.Chef;
			break;
		case "waiter":
			employee.type = Employee.Occupation.Waiter;
			break;
		case "marketing":
			employee.type = Employee.Occupation.Marketing;
			break;
		case "finances":
			employee.type = Employee.Occupation.Waiter;
			break;
		}
		employees.Add(employee);
		return true;
	}

	public bool TrainEmployeeLevel(string name){
		Employee employee = employees.Find (x => x.name == name);
		if (employee == null)
			return false;
		employee.level++;
		return true;
	}
	public bool TrainEmployeeHapyness(string name){
		Employee employee = employees.Find (x => x.name == name);
		if (employee == null)
			return false;
		employee.hapyness++;
		return true;
	}

	public bool DismissEmployee(string name){
		Employee employee = employees.Find (x => x.name == name);
		if (employee != null && !employees.Remove (employee))
			return false;
		return employee_provider.SetAvailable (name, true);
	}

	public List<Candidate> GetCandidatesList() {
		List<Candidate> copy = employee_provider.candidates.ToList ();
		return copy;
	}

	public List<Employee> GetEmployeesList() {
		List<Employee> copy = employees.ToList();
		return copy;
	}

	public Employee GetEmployee(string name){
		return employees.Find (x => x.name == name);
	}

	private List<Employee> employees;
	private EmployeesProvider employee_provider;
	private const int kSalaryMultiplier = 10;

}
