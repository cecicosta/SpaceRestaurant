using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Modifier{
	public Modifier(){
		arguments = new List<string> ();
	}
	public Modifier(Modifier mod){
		value = mod.value;
		attribute = mod.attribute;
		arguments = new List<string> ();
		foreach(string s in mod.arguments){
			arguments.Add(s);
		}
	}

	public void SaveObjectState(){
		EstablishmentManagement.SaveAttribute (value);
		EstablishmentManagement.SaveAttribute (attribute);
		EstablishmentManagement.SaveAttribute (arguments.Count);
		foreach (string s in arguments) {
			EstablishmentManagement.SaveAttribute (s);
		}
	}
	public void LoadObjectState(){
		EstablishmentManagement.LoadAttribute (out value);
		EstablishmentManagement.LoadAttribute (out attribute);
		int size;
		arguments.Clear ();
		EstablishmentManagement.LoadAttribute (out size);
		for(int i=0; i<size; i++) {
			string s;
			EstablishmentManagement.LoadAttribute (out s);
			arguments.Add(s);
		}
	}

	public string value;
	public string attribute;
	public List<string> arguments;
}

public class AttributeModifiers{
	public static void EmployeeSkillModifier(Establishment e, string type, int increment){
		List<Employee> employees = e.alien_resources.GetEmployeesOfType (type);
		foreach (Employee emp in employees) {
			Employee employee = e.alien_resources.GetEmployee (emp.name);
			employee.level += increment; 
		}
		List<Employee> candidates = e.alien_resources.GetCandidatesList ();
		List<Employee> candidates_of_type = candidates.FindAll (x => x.type.ToString () == type);
		foreach (Employee emp in candidates_of_type) {
			Employee employee = e.alien_resources.GetCandidate (emp.name);
			employee.level += increment; 
		}
	}
	public static void EmployeeSkillModifierAll(Establishment e, int increment){
		List<Employee> employees = e.alien_resources.GetEmployeesList ();
		foreach (Employee emp in employees) {
			Employee employee = e.alien_resources.GetEmployee (emp.name);

			if (employee.type == Employee.Type.marketing) {
				AttributeModifiers.AdvertisementPriceModifier(e,  employee.level*2);
				AttributeModifiers.AdvertisementImpactModifier(e, -employee.level);
			}
			if (employee.type == Employee.Type.finances) {
				AttributeModifiers.IngredientsPriceModifier(e, employee.level*2);
			}
			e.alien_resources.IncreaseEmployeeLevel(employee);

			if (employee.type == Employee.Type.marketing) {

				AttributeModifiers.AdvertisementPriceModifier(e,  -employee.level*2);
				AttributeModifiers.AdvertisementImpactModifier(e, employee.level);
			}
			if (employee.type == Employee.Type.finances) {
				AttributeModifiers.IngredientsPriceModifier(e, -employee.level*2);
			}
		}
		List<Employee> candidates = e.alien_resources.GetCandidatesList ();
		foreach (Employee emp in candidates) {
			Employee employee = e.alien_resources.GetCandidate (emp.name);

			if (employee.type == Employee.Type.marketing) {
				AttributeModifiers.AdvertisementPriceModifier(e,  employee.level*2);
				AttributeModifiers.AdvertisementImpactModifier(e, -employee.level);
			}
			if (employee.type == Employee.Type.finances) {
				AttributeModifiers.IngredientsPriceModifier(e, employee.level*2);
			}
			e.alien_resources.IncreaseEmployeeLevel(employee);
			
			if (employee.type == Employee.Type.marketing) {
				
				AttributeModifiers.AdvertisementPriceModifier(e,  -employee.level*2);
				AttributeModifiers.AdvertisementImpactModifier(e, employee.level);
			}
			if (employee.type == Employee.Type.finances) {
				AttributeModifiers.IngredientsPriceModifier(e, -employee.level*2);
			}
		}


	}
	public static void EmployeeHappinessModifier(Establishment e, string type, int increment){
		List<Employee> employees = e.alien_resources.GetEmployeesOfType (type);
		foreach (Employee emp in employees) {
			Employee employee = e.alien_resources.GetEmployee (emp.name);
			employee.happiness += increment; 
		}
		List<Employee> candidates = e.alien_resources.GetCandidatesList ();
		List<Employee> candidates_of_type = candidates.FindAll (x => x.type.ToString () == type);
		foreach (Employee emp in candidates_of_type) {
			Employee employee = e.alien_resources.GetCandidate (emp.name);
			employee.happiness += increment; 
		}
	}
	public static void EmployeeHappinessModifierAll(Establishment e, int increment){
		List<Employee> employees = e.alien_resources.GetEmployeesList ();
		foreach (Employee emp in employees) {
			Employee employee = e.alien_resources.GetEmployee (emp.name);
			employee.happiness += increment; 
		}
		List<Employee> candidates = e.alien_resources.GetCandidatesList ();
		foreach (Employee emp in candidates) {
			Employee employee = e.alien_resources.GetCandidate (emp.name);
			employee.happiness += increment; 
		}
	}

	public static void DirtnessModifier(Establishment e, int increment){
		e.infrastructure.Dirtiness += increment;
	}
	public static void ClientsSatisfactionModifier(Establishment e, int increment){
		e.marketing.Satisfaction += increment;
	}

	public static void IngredientsPriceModifierPercent(Establishment e, double increment_percent ){
		List<Ingredient> ingredients = e.logistics.GetProvider().GetIngredientsList();
		foreach(Ingredient i in ingredients){
			Ingredient ingredient = e.logistics.GetProvider().GetIngredient(i.name);
			if(ingredient == null)
				continue;
			ingredient.Cost += increment_percent;
		}
	}

	public static void IngredientsPriceModifier(Establishment e, double increment ){
		List<Ingredient> ingredients = e.logistics.GetProvider().GetIngredientsList();
		foreach(Ingredient i in ingredients){
			Ingredient ingredient = e.logistics.GetProvider().GetIngredient(i.name);
			if(ingredient == null)
				continue;
			ingredient.Cost += increment;
		}
	}

	public static void AdvertisementPriceModifier(Establishment e, double increment ){
		List<Advertising> ads = e.marketing.AvailableAdvertisements();
		foreach(Advertising ad in ads){

			Advertising advertising = e.marketing.GetProvider().GetAd(ad.type);
			if(advertising == null)
				continue;
			advertising.Price += increment;
		}
	}
	public static void AdvertisementImpactModifier(Establishment e, int increment ){
		List<Advertising> ads = e.marketing.AvailableAdvertisements();
		foreach(Advertising ad in ads){
			
			Advertising advertising = e.marketing.GetProvider().GetAd(ad.type);
			if(advertising == null)
				continue;

			advertising.max_reach += increment;
			advertising.min_reach += increment;
		}
	}

	public static void ApplyModifier(Establishment e, Modifier modifier){
		switch(modifier.attribute){
		case "skill":
			if(modifier.arguments.Count > 0){
				int value;
				System.Int32.TryParse(modifier.value, out value);
				if(modifier.arguments[0].CompareTo("all") == 0)
					EmployeeSkillModifierAll(e, value);
				else
					EmployeeSkillModifier(e, modifier.arguments[0], value);
			}
			break;
		case "happiness":
			if(modifier.arguments.Count > 0){
				int value;
				System.Int32.TryParse(modifier.value, out value);
				if(modifier.arguments[0].CompareTo("all") == 0)
					EmployeeHappinessModifierAll(e, value);
				else
					EmployeeHappinessModifier(e, modifier.arguments[0], value);
			}
			break;
		case "dirtness":
			{
				int value;
				System.Int32.TryParse(modifier.value, out value);
				DirtnessModifier(e, value);
			}break;
		case "clients_satisfaction":
			{
				int value;
				System.Int32.TryParse(modifier.value, out value);
				ClientsSatisfactionModifier(e, value);
			}break;
		case "ingredients_price_discount":
			{
				double value;
				System.Double.TryParse(modifier.value, out value);
				IngredientsPriceModifierPercent(e, value);
			}break;
		}
	}
}
