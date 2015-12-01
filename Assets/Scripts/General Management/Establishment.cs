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
		List<Employee> waiters = alien_resources.GetEmployeesOfType (Employee.Type.waiter);
		int capacity = 0;
		foreach(Employee e in waiters){
			capacity += e.Capacity;
		}
		return capacity;
	}
	public int CalculateRequests(){
		List<Advertising> ads = marketing.GetActiveAdvertisementsList ();
		int additional = 0;
		if (ads != null) {
			foreach (Advertising ad in ads) {
				additional += generator.Next (ad.min_reach, ad.max_reach);
			}
		}

		int factor = requestCalcFactorRange[generator.Next (0, requestCalcFactorRange.Length)];
		return (marketing.Satisfaction / factor) + additional;
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
	public void IncreasePrices(){
		MenuProvider m_provider = MenuProvider.GetInstance ();
		if (m_provider == null) {
			Debug.LogError("Error getting the menu");
			return;
		}
		List<Dish> menu_list = m_provider.GetDishList (); 
		foreach(Dish d in menu_list){
			finances.IncreasePrice (d.name);
		}
		marketing.Satisfaction -= satisfactionIncrement;
	}
	public void DecreasePrices(){
		MenuProvider m_provider = MenuProvider.GetInstance ();
		if (m_provider == null) {
			Debug.LogError("Error getting the menu");
			return;
		}
		List<Dish> menu_list = m_provider.GetDishList (); 
		foreach(Dish d in menu_list){
			finances.DecreasePrice (d.name);
		}
		marketing.Satisfaction += satisfactionIncrement;
	}
	public bool IsOrderAvailable(int number){

		List<Employee> chefs = alien_resources.GetEmployeesOfType (Employee.Type.chef);
		foreach(Employee e in chefs){
			if(e.dishes.Contains(number))
				return true;
		}
		Debug.Log ("Nenhum chefe pode preparar o prato " + number);
		return false;
	}

	public bool MakeOrder(int number){
		MenuProvider menu = MenuProvider.GetInstance ();
		if (menu == null) {
			Debug.LogError("Error getting the menu");
			return false;
		}
		Dish dish = menu.GetDishByID (number);
		if (dish == null) {
			Debug.Log("Prato nao existe");
			return false;
		}

		foreach(string s in dish.ingredients){
			if(!logistics.HasIngredient(s)){
				Debug.Log("Nao possui todos os ingredientes necessarios.");
				return false;
			}
		}
		foreach(string s in dish.ingredients){
			if(logistics.HasIngredient(s)){
				logistics.SpendIngredient(s);
			}
		}
		finances.Cash += dish.price;
		return true;
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
		finances.CloseDayBalance ();
		infrastructure.DirtnessIncrease ();
		DirtnessDailyEffects ();

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
	

		foreach(Modifier mod in equipment.variable_modifiers)
			AttributeModifiers.ApplyModifier (this, mod);

		if (equipment.constant_modifiers.Count > 0) {
			Debug.Log("constant attribute");
			AttributesManager at_m = AttributesManager.GetInstance ();
			foreach (Modifier mod in equipment.constant_modifiers) {
				Debug.Log(mod.value + ", " + mod.attribute);
				at_m.SetAttribute (mod.attribute, mod.value);
			}
			at_m.UpdateAttributes();
		}

		Debug.Log (infrastructure.Dirtiness);
		Debug.Log (marketing.Satisfaction);
		Debug.Log (logistics.StorageTime);
		return true;
	}
	
	public void DirtnessDailyEffects(){
		if (infrastructure.Dirtiness >= 10) {
			GameOver();
		}
		if (infrastructure.Dirtiness == 0 ||
			infrastructure.Dirtiness == 1 || 
		    infrastructure.Dirtiness == 2) {
			marketing.Satisfaction += 2;
		}
		if (infrastructure.Dirtiness == 6 || 
		    infrastructure.Dirtiness == 7) {
			marketing.Satisfaction -= 1;
		}
		if (infrastructure.Dirtiness == 8 || 
		    infrastructure.Dirtiness == 9) {
			marketing.Satisfaction -= 2;
		}
		DirtnessTemporaryEffects ();
	}
	public void DirtnessTemporaryEffects(){
		if (!dirness_temp_effect_active && 
		    (infrastructure.Dirtiness == 8 || 
			 infrastructure.Dirtiness == 9 ||
		 	 infrastructure.Dirtiness == 10)) {
			dirness_temp_effect_active = true;
			original_storage_time = logistics.StorageTime;
			logistics.StorageTime = (int)Mathf.Ceil((float)logistics.StorageTime/2.0f); 
			Debug.Log(logistics.StorageTime);
			AttributeModifiers.EmployeeHappinessModifierAll(this, -1);
		} 
		if (dirness_temp_effect_active &&
		   (infrastructure.Dirtiness != 8 && 
		    infrastructure.Dirtiness != 9 &&
		 	infrastructure.Dirtiness != 10)){
			dirness_temp_effect_active = false;
			logistics.StorageTime = original_storage_time; 
			AttributeModifiers.EmployeeHappinessModifierAll(this, 1);
		}
	}
	public void DoCleaning(){
		if (infrastructure.Dirtiness <= 0)
			return;
		if (finances.Cash - cleaning_costs < 0) {
			Debug.Log ("Nao ha dinheiro suficiente para fazer a limpeza.");
			return;
		}
		infrastructure.MakeCleaning ();
		finances.Cash -= cleaning_costs;

		DirtnessTemporaryEffects ();
	}
	public int CleaningCosts{
		get{
			return cleaning_costs;
		}
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
	public void GameOver(){
	}

	public static bool LoadAttributes(){
		AttributesManager at_m = AttributesManager.GetInstance ();
		if (at_m == null)
			return false;
		initialActionPoints = at_m.IntValue ("action_points");
		initialReactionPoints = at_m.IntValue ("reaction_points");
		actionReationConvertion = at_m.IntValue ("action_reaction_convertion");
		satisfactionIncrement = at_m.IntValue ("satisfaction_increment");
		cleaning_costs = at_m.IntValue ("cleaning_costs");
		requestCalcFactorRange = at_m.RangeValue ("requests_calc_factor_range");
		return true;
	}

	private static int initialReactionPoints;
	private static int initialActionPoints;
	private static int actionReationConvertion;
	private static int satisfactionIncrement; //Incremental value of satisfaction over price changes
	private static int cleaning_costs;
	private static int[] requestCalcFactorRange;

	private System.Random generator;
	private bool dirness_temp_effect_active = false;
	private int original_storage_time = 0;
}
