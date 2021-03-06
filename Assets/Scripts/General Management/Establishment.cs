﻿using UnityEngine;
using System.Collections;
using System.Collections.Generic;

//The stablishment adds a layer to the functionalities of the basic structure.
//Some operation as hiring an employee may depends on information from other
//structures as the finances to evaluate if there is budget enough to hire
//the employee.

[System.Serializable]
public class Establishment{
	
	private static int kActionCleaningCost = 1;
	private static int kActionBuyEquipmentCost = 1;
	private static int kActionAdvertisementCost = 1;
	private static int kActionIncreaseDecreasePricesCost = 1;
	private static int kActionDismissCost = 1;
	private static int kActionTrainHappinessCost = 1;
	private static int kActionTrainLevelCost = 1;
	private static int kActionHireCost = 1;
	private static int kAdsDiscountMultiplier = 4;
	private static int kIngredientsDiscountMultiplier = 2;
	private static int kMaxScore = 1000000;

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
		if (finances.Cash - ingredient.Cost < 0) {
			GameLog.Log(GameLog.kTNotEnoughtMoney);
			return false;
		}
		if (logistics.InventoryCount >= GetStorageCapacity ()) {
			GameLog.Log(GameLog.kTInventoryOutOfSpace, logistics.StorageCapacity.ToString());
			return false;
		}
		if (!logistics.AquireIngredient (ingredient.name))
			return false;
		finances.Cash -= ingredient.Cost;

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
		finances.Cash += dish.Price;
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
		CalculateScore ();
		CheckGameOverConditions ();
		CheckVictoryConditions ();
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
			GameLog.Log(GameLog.kTNotEnoughPoints);
			return false;
		}
		if (marketing.WasHired (type))
			return false;
		if (finances.Cash - advertisement.Price < 0) {
			GameLog.Log(GameLog.kTNotEnoughtMoney);
			return false;
		}
		if (!marketing.HireAdvertisement (type))
			return false;
		finances.Cash -= advertisement.Price;
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
			GameLog.Log(GameLog.kTNotEnoughPoints);
			return false;
		}
		if (finances.Cash - equipment.Price < 0) {
			GameLog.Log(GameLog.kTNotEnoughtMoney);
			return false;
		}
		if (!infrastructure.BuyEquipment (name)) {
			return false;
		}
		finances.Cash -= equipment.Price;
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
			GameLog.Log(GameLog.kTSatisfactionIncreased, "2 ", GameLog.kTPoints);
		}
		if (infrastructure.Dirtiness == 6 || 
		    infrastructure.Dirtiness == 7) {
			marketing.Satisfaction -= 1;
			GameLog.Log(GameLog.kTSatisfactionDecreased, "1 ",  GameLog.kTPoints);
		}
		if (infrastructure.Dirtiness == 8 || 
		    infrastructure.Dirtiness == 9) {
			marketing.Satisfaction -= 2;
			GameLog.Log(GameLog.kTSatisfactionDecreased, "2 ", GameLog.kTPoints);
		}
		DirtnessTemporaryEffects ();
	}


	public void IncreaseSatisfactionByOrder(){
		GameLog.Log(GameLog.kTSatisfactionIncreased, increaseSatisfactionByOrderRate.ToString());
		marketing.Satisfaction += increaseSatisfactionByOrderRate;
	}
	public void DecreaseSatisfactionByOrder(){
		GameLog.Log(GameLog.kTSatisfactionDecreased, decreaseSatisfactionByOrderRate.ToString());
		marketing.Satisfaction += decreaseSatisfactionByOrderRate;
	}

	public void DirtnessTemporaryEffects(){
		if (!dirness_temp_effect_active && 
		    (infrastructure.Dirtiness == 8 || 
			 infrastructure.Dirtiness == 9 ||
		 	 infrastructure.Dirtiness == 10)) {
			dirness_temp_effect_active = true;
			original_storage_time = logistics.StorageTime;
			logistics.StorageTime = (int)Mathf.Ceil((float)logistics.StorageTime/2.0f); 
			GameLog.Log(GameLog.kTStorageTimeDecreased, logistics.StorageTime + " ", GameLog.kTDays);
			GameLog.Log(GameLog.kTEmployeesHappinessDecreased, "1 ", GameLog.kTPoints );
			AttributeModifiers.EmployeeHappinessModifierAll(this, -1);
		} 
		if (dirness_temp_effect_active &&
		   (infrastructure.Dirtiness != 8 && 
		    infrastructure.Dirtiness != 9 &&
		 	infrastructure.Dirtiness != 10)){
			dirness_temp_effect_active = false;
			logistics.StorageTime = original_storage_time; 
			GameLog.Log(GameLog.kTStorageTimeIncreased, logistics.StorageTime + " ", GameLog.kTDays);
			GameLog.Log(GameLog.kTEmployeesHappinessIncreased, "1 ", GameLog.kTPoints );
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
	public void CalculateScore(){
		score = marketing.Satisfaction * 10000;
		if (score > 1000000)
			score = 1000000;
	}


	public void PaySalaries(){
		if (logistics.CurrentDay % 5 == 0) {
			double payment = alien_resources.CalculateEmployeesPayment ();

			if (finances.Cash - payment > 0){	
				GameLog.Log (GameLog.kTSalariesPayment);
			finances.Cash -= payment;
			}else{
				GameLog.Log(GameLog.kNoCashToPaySalaries);
				GameLog.Log(GameLog.kTEmployeesHappinessDecreased, "1 ", GameLog.kTPoints);
				AttributeModifiers.EmployeeHappinessModifierAll(this, -1);
			}
		}
	}

	public void CheckEmployesSelfDismiss(){
		foreach(Employee e in alien_resources.GetEmployeesList()){
			if(e.happiness == 0)
				alien_resources.DismissEmployee(e.name);
		}
	}

	public void CheckGameOverConditions(){
		if (infrastructure.Dirtiness >= 10) {
			game_over_message = GameTranslator.Instance.Translate(GameLog.kTTooDirt);
			GameOver();
		}
		if (marketing.Satisfaction <= 0) {
			game_over_message = GameTranslator.Instance.Translate(GameLog.kTLowSatisfaction);
			GameOver ();
		}
	}

	public void CheckVictoryConditions(){
		if(score == kMaxScore ){
			victory_condition = true;
		}
	}

	public void GameOver(){
		GameLog.Log (GameLog.kTGameOver);
		game_over_condition = true;
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
		
		increaseSatisfactionByOrderRate = at_m.IntValue ("increase_satisfaction_by_order_rate");
		decreaseSatisfactionByOrderRate = at_m.IntValue ("decrease_satisfaction_by_order_rate");
		daysBetweenPayment = at_m.IntValue ("days_between_payment");

		return true;
	}

	public void SaveObjectState(){
		EstablishmentManagement.SaveAttribute (action_points);
		EstablishmentManagement.SaveAttribute (reaction_points);
		EstablishmentManagement.SaveAttribute (changed_prices);
		EstablishmentManagement.SaveAttribute (dirness_temp_effect_active);
		EstablishmentManagement.SaveAttribute (original_storage_time);
		EstablishmentManagement.SaveAttribute (victory_condition);
		EstablishmentManagement.SaveAttribute (game_over_condition);
		EstablishmentManagement.SaveAttribute (game_over_message);
		EstablishmentManagement.SaveAttribute (score);

		alien_resources.SaveObjectState ();
		marketing.SaveObjectState ();
		finances.SaveObjectState ();
		logistics.SaveObjectState ();
		infrastructure.SaveObjectState ();
		MenuProvider.GetInstance ().SaveObjectState ();
	}

	public void LoadObjectState(){
		EstablishmentManagement.LoadAttribute (out action_points);
		EstablishmentManagement.LoadAttribute (out reaction_points);
		EstablishmentManagement.LoadAttribute (out changed_prices);
		EstablishmentManagement.LoadAttribute (out dirness_temp_effect_active);
		EstablishmentManagement.LoadAttribute (out original_storage_time);
		EstablishmentManagement.LoadAttribute (out victory_condition);
		EstablishmentManagement.LoadAttribute (out game_over_condition);
		EstablishmentManagement.LoadAttribute (out game_over_message);
		EstablishmentManagement.LoadAttribute (out score);
		
		alien_resources.LoadObjectState ();
		marketing.LoadObjectState ();
		finances.LoadObjectState ();
		logistics.LoadObjectState ();
		infrastructure.LoadObjectState ();
		MenuProvider.GetInstance ().LoadObjectState ();
	}

	private static int initialReactionPoints;
	private static int initialActionPoints;
	private static int actionReationConvertion;
	private static int satisfactionIncrementByPrice; //Incremental value of satisfaction over price changes
	private static int cleaning_costs;
	private static int[] requestCalcFactorRange;
	private static int increaseSatisfactionByOrderRate = 2;
	private static int decreaseSatisfactionByOrderRate = -5;
	public static int daysBetweenPayment = 5;

	private System.Random generator = new System.Random();


	//States to save
	public int action_points;
	public int reaction_points;
	public int score = 0;
	private bool changed_prices = false;
	private bool dirness_temp_effect_active = false;
	private int original_storage_time = 0;
	public bool victory_condition = false;
	public bool game_over_condition = false;
	public string game_over_message = "";

	public AlianResources alien_resources;
	public Marketing marketing;
	public Finances finances;
	public Logistics logistics;
	public Infrastructure infrastructure;
}
