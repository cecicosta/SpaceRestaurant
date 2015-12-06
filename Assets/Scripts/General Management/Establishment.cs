using UnityEngine;
using System.Collections;
using System.Collections.Generic;

//The stablishment adds a layer to the functionalities of the basic structure.
//Some operation as hiring an employee may depends on information from other
//structures as the finances to evaluate if there is budget enough to hire
//the employee.

public class Establishment{

	public int kDaysBetweenPayment = 5;
	private static int kActionCleaningCost = 1;
	private static int kIncreaseSatisfactionByOrderRate = 2;
	private static int kDecreaseSatisfactionByOrderRate = -5;
	private static int kActionBuyEquipmentCost = 1;
	private static int kActionAdvertisementCost = 1;
	private static int kActionIncreaseDecreasePricesCost = 1;
	private static int kActionBuyIngredientCost = 1;
	private static int kActionDismissCost = 1;
	private static int kActionTrainHappinessCost = 1;
	private static int kActionTrainLevelCost = 1;
	private static int kActionHireCost = 1;
	private static int kAdsDiscountMultiplier = 2;
	private static int kIngredientsDiscountMultiplier = 2;

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
		action_points = initialActionPoints;
		reaction_points = initialReactionPoints;
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
		if(action_points - kActionHireCost < 0){
			GameLog.Log(GameLog.kTNotEnoughPoints);
			return false;
		}
		if (finances.Cash - candidate.HireCosts < 0) {
			GameLog.Log(GameLog.kTNotEnoughtMoney);
			return false;
		}
		if (candidate.type == Employee.Type.marketing) {
			AttributeModifiers.AdvertisementPriceModifier(this, -candidate.level*kAdsDiscountMultiplier);
			AttributeModifiers.AdvertisementImpactModifier(this, candidate.level);
		}
		if (candidate.type == Employee.Type.finances) {
			AttributeModifiers.IngredientsPriceModifier(this, -candidate.level*kIngredientsDiscountMultiplier);
		}

		if (!alien_resources.HireEmployee (candidate.name))
			return false;
		finances.Cash -= candidate.HireCosts;
		action_points -= kActionHireCost;
		return true;
	}

	public bool TrainLevel(string name){
		List<Employee> employees_list = alien_resources.GetEmployeesList ();
		Employee employee = employees_list.Find (x => x.name == name);
		if (employee == null)
			return false;
		if(action_points - kActionTrainLevelCost < 0){
			GameLog.Log(GameLog.kTNotEnoughPoints);
			return false;
		}
		if (alien_resources.WasTrained (name)) {
			GameLog.Log(GameLog.kTEmployeeAlreadyTrained);
			return false;
		}
		if (finances.Cash - employee.TrainSkillCosts < 0) {
			GameLog.Log(GameLog.kTNotEnoughtMoney);
			return false;
		}
		//Undo modifiers
		if (employee.type == Employee.Type.marketing) {
			AttributeModifiers.AdvertisementPriceModifier(this,  employee.level*kAdsDiscountMultiplier);
			AttributeModifiers.AdvertisementImpactModifier(this, -employee.level);
		}
		if (employee.type == Employee.Type.finances) {
			AttributeModifiers.IngredientsPriceModifier(this, employee.level*kIngredientsDiscountMultiplier);
		}

		double train_costs = employee.TrainSkillCosts;
		if (!alien_resources.TrainEmployeeLevel (employee.name))
			return false;

		//Redo modifiers
		employees_list = alien_resources.GetEmployeesList ();
		employee = employees_list.Find (x => x.name == name);
		if (employee.type == Employee.Type.marketing) {
			AttributeModifiers.AdvertisementPriceModifier(this,  -employee.level*kAdsDiscountMultiplier);
			AttributeModifiers.AdvertisementImpactModifier(this, employee.level);
		}
		if (employee.type == Employee.Type.finances) {
			AttributeModifiers.IngredientsPriceModifier(this, -employee.level*kIngredientsDiscountMultiplier);
		}

		finances.Cash -= train_costs;
		action_points -= kActionTrainLevelCost;

		return true;
	}

	public bool TrainHappiness(string name){
		List<Employee> employees_list = alien_resources.GetEmployeesList ();
		Employee employee = employees_list.Find (x => x.name == name);
		if (employee == null)
			return false;
		if(action_points - kActionTrainHappinessCost < 0){
			GameLog.Log(GameLog.kTNotEnoughPoints);
			return false;
		}
		if (alien_resources.WasTrained (name)) {
			GameLog.Log(GameLog.kTEmployeeAlreadyTrained);
			return false;
		}
		if (finances.Cash - employee.TrainHappinessCosts < 0) {
			GameLog.Log(GameLog.kTNotEnoughtMoney);
			return false;
		}

		double train_costs = employee.TrainSkillCosts;
		if (!alien_resources.TrainEmployeeHapyness (employee.name))
			return false;
		finances.Cash -= train_costs;
		action_points -= kActionTrainHappinessCost;
		
		return true;
	}

	public bool Dismiss(string name){
		List<Employee> employees_list = alien_resources.GetEmployeesList ();
		Employee employee = employees_list.Find (x => x.name == name);
		if (employee == null)
			return false;
		if(action_points - kActionDismissCost < 0){
			GameLog.Log(GameLog.kTNotEnoughPoints);
			return false;
		}
		if (finances.Cash - employee.DismissCosts < 0) {
			GameLog.Log(GameLog.kTNotEnoughtMoney);
			return false;
		}
		if (!alien_resources.DismissEmployee (employee.name))
			return false;
		finances.Cash -= employee.DismissCosts;

		if (employee.type == Employee.Type.marketing) {
			AttributeModifiers.AdvertisementPriceModifier(this,  employee.level*kAdsDiscountMultiplier);
			AttributeModifiers.AdvertisementImpactModifier(this, -employee.level);
		}
		if (employee.type == Employee.Type.finances) {
			AttributeModifiers.IngredientsPriceModifier(this, employee.level*kIngredientsDiscountMultiplier);
		}
		return true;
	}


	public bool BuyIngredient(string name){
		List<Ingredient> ingredients_list = logistics.GetProviderIngredientsList ();
		Ingredient ingredient = ingredients_list.Find (x => x.name == name);
		if (ingredient == null)
			return false;
		if (action_points - kActionBuyIngredientCost < 0) {
			GameLog.Log(GameLog.kTNotEnoughPoints);
			return false;
		}
		if (finances.Cash - ingredient.cost < 0) {
			GameLog.Log(GameLog.kTNotEnoughtMoney);
			return false;
		}
		if (logistics.InventoryCount >= GetStorageCapacity ()) {
			GameLog.Log(GameLog.kTInventoryOutOfSpace, logistics.StorageCapacity.ToString());
			return false;
		}
		if (!logistics.AquireIngredient (ingredient.name))
			return false;
		finances.Cash -= ingredient.cost;
		action_points -= kActionBuyIngredientCost;

		return true;
	}
	public bool SpendIngredient(string name){
		GameLog.Log(GameLog.kTIngredientSpend, name);
		return logistics.SpendIngredient(name);
	}

	//Finances Options
	public bool IncreasePrices(){
		MenuProvider m_provider = MenuProvider.GetInstance ();
		if (m_provider == null) {
			Debug.LogError("Error getting the menu");
			return false;
		}
		if (changed_prices) {
			GameLog.Log(GameLog.kTPricesAlreadyChanged);
			return false;
		}
		if(action_points - kActionIncreaseDecreasePricesCost < 0){
			GameLog.Log(GameLog.kTNotEnoughPoints);
			return false;
		}

		List<Dish> menu_list = m_provider.GetDishList (); 
		foreach(Dish d in menu_list){
			finances.IncreasePrice (d.name);
		}
		marketing.Satisfaction -= satisfactionIncrementByPrice;
		action_points -= kActionIncreaseDecreasePricesCost;
		changed_prices = true;
		return true;
	}
	public bool DecreasePrices(){
		MenuProvider m_provider = MenuProvider.GetInstance ();
		if (m_provider == null) {
			Debug.LogError("Error getting the menu");
			return false;
		}
		if (changed_prices) {
			GameLog.Log(GameLog.kTPricesAlreadyChanged);
			return false;
		}
		if(action_points - kActionIncreaseDecreasePricesCost < 0){
			GameLog.Log(GameLog.kTNotEnoughPoints);
			return false;
		}
		List<Dish> menu_list = m_provider.GetDishList (); 
		foreach(Dish d in menu_list){
			finances.DecreasePrice (d.name);
		}
		marketing.Satisfaction += satisfactionIncrementByPrice;
		action_points -= kActionIncreaseDecreasePricesCost;
		changed_prices = true;
		return true;
	}
	public bool IsOrderAvailable(int number){

		List<Employee> chefs = alien_resources.GetEmployeesOfType (Employee.Type.chef);
		foreach(Employee e in chefs){
			if(e.dishes.Contains(number))
				return true;
		}
		GameLog.Log (GameLog.kTDishCannotBePrepared);
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
				GameLog.Log (GameLog.kTOutOfIngredients);
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
		changed_prices = false;
		logistics.NextDay ();
		logistics.CleanOutOfDateIngredients ();
		alien_resources.ClearTrained ();
		marketing.ClearHiredAds ();
		infrastructure.DirtnessIncrease ();
		DirtnessDailyEffects ();
		finances.CloseDayBalance ();
		PaySalaries ();
		RestorePoints ();

		CheckGameOverConditions ();
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
		if(action_points - kActionAdvertisementCost < 0){
			return false;
		}
		if (marketing.WasHired (type))
			return false;
		if (finances.Cash - advertisement.price < 0)
			return false;
		if (!marketing.HireAdvertisement (type))
			return false;
		finances.Cash -= advertisement.price;
		action_points -= kActionAdvertisementCost;
		
		return true;
	}

	//Infrastructure
	public bool BuyEquipment(string name){

		List<Equipment> equips_list = infrastructure.GetProviderEquipmentsList ();
		Equipment equipment = equips_list.Find (x => x.name == name);
		if (equipment == null)
			return false;
		if (action_points - kActionBuyEquipmentCost < 0) {
			return false;
		}
		if (finances.Cash - equipment.price < 0)
			return false;
		if (!infrastructure.BuyEquipment (name))
			return false;
		finances.Cash -= equipment.price;
		action_points -= kActionBuyEquipmentCost;

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
	
		return true;
	}
	
	public void DirtnessDailyEffects(){
		if (infrastructure.Dirtiness == 0 ||
			infrastructure.Dirtiness == 1 || 
		    infrastructure.Dirtiness == 2) {
			marketing.Satisfaction += 2;
			GameLog.Log(GameLog.kTSatisfactionIncreased, "2 pontos");
		}
		if (infrastructure.Dirtiness == 6 || 
		    infrastructure.Dirtiness == 7) {
			marketing.Satisfaction -= 1;
			GameLog.Log(GameLog.kTSatisfactionDecreased, "1 ponto");
		}
		if (infrastructure.Dirtiness == 8 || 
		    infrastructure.Dirtiness == 9) {
			marketing.Satisfaction -= 2;
			GameLog.Log(GameLog.kTSatisfactionDecreased, "2 pontos");
		}
		DirtnessTemporaryEffects ();
	}


	public void IncreaseSatisfactionByOrder(){
		GameLog.Log(GameLog.kTSatisfactionIncreased, kIncreaseSatisfactionByOrderRate.ToString());
		marketing.Satisfaction += kIncreaseSatisfactionByOrderRate;
	}
	public void DecreaseSatisfactionByOrder(){
		GameLog.Log(GameLog.kTSatisfactionDecreased, kDecreaseSatisfactionByOrderRate.ToString());
		marketing.Satisfaction += kDecreaseSatisfactionByOrderRate;
	}

	public void DirtnessTemporaryEffects(){
		if (!dirness_temp_effect_active && 
		    (infrastructure.Dirtiness == 8 || 
			 infrastructure.Dirtiness == 9 ||
		 	 infrastructure.Dirtiness == 10)) {
			dirness_temp_effect_active = true;
			original_storage_time = logistics.StorageTime;
			logistics.StorageTime = (int)Mathf.Ceil((float)logistics.StorageTime/2.0f); 
			GameLog.Log(GameLog.kTStorageTimeDecreased, logistics.StorageTime + " dias");
			GameLog.Log(GameLog.kTEmployeesHappinessDecreased, "1 ponto." );
			AttributeModifiers.EmployeeHappinessModifierAll(this, -1);
		} 
		if (dirness_temp_effect_active &&
		   (infrastructure.Dirtiness != 8 && 
		    infrastructure.Dirtiness != 9 &&
		 	infrastructure.Dirtiness != 10)){
			dirness_temp_effect_active = false;
			logistics.StorageTime = original_storage_time; 
			GameLog.Log(GameLog.kTStorageTimeIncreased, logistics.StorageTime + " dias");
			GameLog.Log(GameLog.kTEmployeesHappinessIncreased, "1 ponto." );
			AttributeModifiers.EmployeeHappinessModifierAll(this, 1);
		}
	}
	 
	public bool DoCleaning(){
		if (infrastructure.Dirtiness <= 0)
			return false;
		if (action_points - kActionCleaningCost < 0) {
			GameLog.Log(GameLog.kTNotEnoughPoints);
			return false;
		}
		if (finances.Cash - cleaning_costs < 0) {
			GameLog.Log(GameLog.kTNotEnoughtMoney);
			return false;
		}
		infrastructure.MakeCleaning ();
		finances.Cash -= cleaning_costs;
		action_points -= kActionCleaningCost;

		DirtnessTemporaryEffects ();
	
		return true;
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


	public void PaySalaries(){
		if (logistics.CurrentDay % 5 == 0) {
			double payment = alien_resources.CalculateEmployeesPayment ();
			finances.Cash -= payment;
		}
	}
	public void CheckGameOverConditions(){
		if (infrastructure.Dirtiness >= 10) {
			GameOver();
		}
		if (marketing.Satisfaction <= 0) {
			GameOver ();
		}
		if (finances.Cash <= 0) {
			GameOver();
		}
	}

	public void GameOver(){
		Debug.Log ("Game Over");
	}

	public static bool LoadAttributes(){
		AttributesManager at_m = AttributesManager.GetInstance ();
		if (at_m == null)
			return false;
		initialActionPoints = at_m.IntValue ("action_points");
		initialReactionPoints = at_m.IntValue ("reaction_points");
		actionReationConvertion = at_m.IntValue ("action_reaction_convertion");
		satisfactionIncrementByPrice = at_m.IntValue ("satisfaction_increment");
		cleaning_costs = at_m.IntValue ("cleaning_costs");
		requestCalcFactorRange = at_m.RangeValue ("requests_calc_factor_range");
		return true;
	}

	public void SaveObjectState(){
		EstablishmentManagement.SaveAttribute (action_points);
		EstablishmentManagement.SaveAttribute (reaction_points);
		EstablishmentManagement.SaveAttribute (changed_prices);
		EstablishmentManagement.SaveAttribute (dirness_temp_effect_active);
		EstablishmentManagement.SaveAttribute (original_storage_time);

		alien_resources.SaveObjectState ();
		marketing.SaveObjectState ();
		finances.SaveObjectState ();
		logistics.SaveObjectState ();
		infrastructure.SaveObjectState ();
	}

	public void LoadObjectState(){
		EstablishmentManagement.LoadAttribute (out action_points);
		EstablishmentManagement.LoadAttribute (out reaction_points);
		EstablishmentManagement.LoadAttribute (out changed_prices);
		EstablishmentManagement.LoadAttribute (out dirness_temp_effect_active);
		EstablishmentManagement.LoadAttribute (out original_storage_time);
		
		alien_resources.LoadObjectState ();
		marketing.LoadObjectState ();
		finances.LoadObjectState ();
		logistics.LoadObjectState ();
		infrastructure.LoadObjectState ();
	}

	private static int initialReactionPoints;
	private static int initialActionPoints;
	private static int actionReationConvertion;
	private static int satisfactionIncrementByPrice; //Incremental value of satisfaction over price changes
	private static int cleaning_costs;
	private static int[] requestCalcFactorRange;
	private System.Random generator;

	//States to save
	public int action_points;
	public int reaction_points;
	private bool changed_prices = false;
	private bool dirness_temp_effect_active = false;
	private int original_storage_time = 0;

	public AlianResources alien_resources;
	public Marketing marketing;
	public Finances finances;
	public Logistics logistics;
	public Infrastructure infrastructure;
}
