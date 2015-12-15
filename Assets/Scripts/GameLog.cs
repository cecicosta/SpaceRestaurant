using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class GameLog {

	public static string kGameName = "GAME_NAME";
	
	public static string kProgressLoaded = "PROGRESS_LOADED";
	public static string kProgressSaved = "PROGRESS_SAVED";
	public static string kTDayIsOver = "DAY_IS_OVER";
	public static string kTGameOver = "GAME_OVER";
	public static string kTNoCashBuyIngredient = "NO_CASH_BUY_INGREDIENT";
	public static string kTNoCashBuyEquipment = "NO_CASH_BUY_EQUIPMENT";
	public static string kTNoCashHireAdvertisement = "NO_CASH_HIRE_ADVERTISEMENT";
	public static string kTNoCashHireEmployee = "NO_CASH_HIRE_EMPLOYEE";
	public static string kTNoCashDismissEmployee = "NO_CASH_DISMISS_EMPLOYEE";
	public static string kTNoCashTrainEmployee = "NO_CASH_TRAIN_EMPLOYEE";
	public static string kTNoCashDoCleaning = "NO_CASH_DO_CLEANING";	
	
	public static string kTNoPointsBuyIngredient = "NO_POINTS_BUY_INGREDIENT";
	public static string kTNoPointsHireEquipment = "NO_POINTS_HIRE_EQUIPMENT";
	public static string kTNoPointsHireAdvertisement = "NO_POINTS_HIRE_ADVERTISEMENT";
	public static string kTNoPointsHireEmployee = "NO_POINTS_HIRE_EMPLOYEE";
	public static string kTNoPointsDismissEmployee = "NO_POINTS_DISMISS_EMPLOYEE";
	public static string kTNoPointsTrainEmployee = "NO_POINTS_TRAIN_EMPLOYEE";
	public static string kTNoPointsDoCleaning = "NO_POINTS_DO_CLEANING";
	
	public static string kTEmployeeAlreadyTrained = "EMPLOYEE_ALREADY_TRAINED";
	
	public static string kTIngredientAquired = "INGREDIENT_AQUIRED";
	public static string kTEquipmentAquired = "EQUIPMENT_AQUIRED";
	public static string kTEmployeeHired = "EMPLOYEE_HIRED";
	public static string kTEmployeeTrained = "EMPLOYEE_TRAINED";
	public static string kTEmployeeDismissed = "EMPLOYEE_DISMISSED";
	public static string kTCleaningDone = "CLEANING_DONE";
	
	public static string kTClientOrder = "CLIENT_ORDER";
	public static string kTClientOrderAttended = "CLIENT_ORDER_ATTENDED";
	public static string kTClientOrderNotAttended = "CLIENT_ORDER_NOT_ATTENDED";
	public static string kTDishCannotBePrepared = "DISH_CAN_NOT_BE_PREPARED";
	public static string kTClientsLeft = "CLIENT_SLEFT";
	public static string kTOutOfIngredients = "OUT_OF_INGREDIENTS";
	public static string kTClientWaitTooLong = "CLIENT_WAIT_TOO_LONG";
	
	public static string kTDirtnessIncreased = "DIRTNESS_INCREASED";
	public static string kTDirtnessDecreased = "DIRTNESS_DECREASED";
	public static string kTSatisfactionIncreased = "SATISFACTION_INCREASED";
	public static string kTSatisfactionDecreased = "SATISFACTION_DECREASED";
	public static string kTEmployeesHappinessIncreased = "EMPLOYEES_HAPPINESS_INCREASED";
	public static string kTEmployeesHappinessDecreased = "EMPLOYEES_HAPPINESS_DECREASED";
	public static string kTEmployeesSkillIncreasedForType = "EMPLOYEES_SKILL_INCREASED_FOR_TYPE";
	
	public static string kTRottenIngredientsDiscarted = "ROTTEN_INGREDIENTS_DISCARTED";
	public static string kTAdvertisementsOver = "ADVERTISEMENTS_OVER";
	public static string kTSalariesPayment = "SALARIES_PAY_MENT";
	public static string kTIngredientSpend = "INGREDIENTS_PEND";
	public static string kTPricesAlreadyChanged = "PRICES_ALREADY_CHANGED";
	
	public static string kTNotEnoughPoints = "NOT_ENOUGH_POINTS";
	public static string kTNotEnoughtMoney = "NOT_ENOUGH_MONEY";
	public static string kTInventoryOutOfSpace = "INVENTORY_OUT_OF_SPACE";
	public static string kTStorageTimeDecreased = "STORAGE_TIME_DECREASED";
	public static string kTStorageTimeIncreased = "STORAGE_TIME_INCREASED";
	public static string kNoCashToPaySalaries = "NO_CASH_TO_PAY";


	public static string kTTooDirt = "TOO_DIRT";
	public static string kTLowSatisfaction = "LOW_SATISFACTION";
	public static string kTBankrupt = "BANKROUPT";

	public static string kTDays = "DAYS_UNIT";
	public static string kTPoints = "POINTS_UNIT";

	public static void Log(string token){
		logs.Add (GameTranslator.Instance.Translate(token));
	}
	public static void Log(string token, string value){
		logs.Add (GameTranslator.Instance.Translate(token) + value);
	}
	public static void LogValueToken(string value, string token){
		logs.Add (value + GameTranslator.Instance.Translate(token));
	}
	public static void Log(string token, string value, string unit){
		logs.Add (GameTranslator.Instance.Translate(token) + value + GameTranslator.Instance.Translate(unit));
	}
	public static void Log(int value, string token){
		logs.Add (value.ToString() + GameTranslator.Instance.Translate(token));
	}

	public static List<string> logs = new List<string>(); 
}
