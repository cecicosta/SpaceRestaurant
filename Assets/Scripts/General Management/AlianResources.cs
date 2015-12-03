using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

public class AlianResources{
	public AlianResources(){
		employee_provider = new EmployeesProvider ();
		employees = new List<Employee> ();
		trained = new List<string> ();
	}

	public bool Initiate(){
		if (employee_provider == null)
			return false;
		if (!employee_provider.Initiate ())
			return false;

		//Hire initial employees
		HireEmployee ("Arlinum");
		HireEmployee ("Ornete");
		return true;
	}

	public bool HireEmployee(string name){
		Employee candidate = employee_provider.GetCandidate (name);
		if (candidate == null) {
			return false;
		}
		if(!employee_provider.RemoveCandidate (candidate))
			return false;
	
		employees.Add(candidate);
		return true;
	}

	public bool TrainEmployeeLevel(string name){
		Employee employee = employees.Find (x => x.name == name);
		if (employee == null)
			return false;
		//Add new dishes for the chef
		if(employee.type == Employee.Type.chef){
			MenuProvider menu = MenuProvider.GetInstance();
			if(menu == null)
				return false;
			List<Dish> dishes = menu.GetDishList().FindAll(
								x => x.nivel == employee.level+1);
			System.Random generator = new System.Random();
			Dish dish = dishes[generator.Next(0,dishes.Count-1)];
			dishes.Remove(dish);
			employee.dishes.Add(dish.id);
			dish = dishes[generator.Next(0,dishes.Count-1)];
			employee.dishes.Add(dish.id);
		}
		employee.level++;
		if(!WasTrained(name))
			trained.Add (employee.name);
		return true;
	}

	public bool IncreaseEmployeeLevel(Employee employee){
		if (employee == null)
			return false;
		//Add new dishes for the chef
		if(employee.type == Employee.Type.chef){
			MenuProvider menu = MenuProvider.GetInstance();
			if(menu == null)
				return false;
			List<Dish> dishes = menu.GetDishList().FindAll(
				x => x.nivel == employee.level+1);
			System.Random generator = new System.Random();
			Dish dish = dishes[generator.Next(0,dishes.Count-1)];
			dishes.Remove(dish);
			employee.dishes.Add(dish.id);
			dish = dishes[generator.Next(0,dishes.Count-1)];
			employee.dishes.Add(dish.id);
		}
		employee.level++;
		return true;
	}

	public bool TrainEmployeeHapyness(string name){
		Employee employee = employees.Find (x => x.name == name);
		if (employee == null)
			return false;
		employee.happiness++;
		if(!WasTrained(name))
			trained.Add (employee.name);
		return true;
	}

	public bool DismissEmployee(string name){
		Debug.Log (name);
		Employee employee = employees.Find (x => x.name == name);
		if (employee != null && !employees.Remove (employee))
			return false;
		if(!employee_provider.AddCandidate (employee))
			return false;


		return true;
	}

	public double CalculateEmployeesPayment(){
		double payment = 0;
		foreach(Employee e in employees){
			payment += e.Salary;
		}
		return payment;
	}

	public List<Employee> GetCandidatesList() {
		return employee_provider.GetCandidatesList ();
	}

	//Returns a copy of the list of employees
	public List<Employee> GetEmployeesList() {
		List<Employee> copy = new List<Employee> ();
		foreach(Employee e in employees){
			copy.Add(new Employee(e));
		}
		return copy;
	}
	//Returns a reference for an employee
	public Employee GetEmployee(string name){
		return employees.Find (x => x.name == name);
	}

	public Employee GetCandidate(string name){
		return employee_provider.GetCandidate (name);
	}

	public List<Employee> GetEmployeesOfType(Employee.Type type){
		List<Employee> emp = employees.FindAll (x => x.type == type);
		List<Employee> copy = new List<Employee> ();
		foreach (Employee e in emp) {
			copy.Add(new Employee(e));
		}
		return copy;
	}
	public List<Employee> GetEmployeesOfType(string type){
		List<Employee> emp = employees.FindAll (x => x.type.ToString() == type);
		List<Employee> copy = new List<Employee> ();
		foreach (Employee e in emp) {
			copy.Add(new Employee(e));
		}
		return copy;
	}

	public bool WasTrained(string name){
		return trained.Contains (name);
	}

	public void ClearTrained(){
		trained.Clear();
	}

	private List<string> trained;
	private List<Employee> employees;
	private EmployeesProvider employee_provider;


}
