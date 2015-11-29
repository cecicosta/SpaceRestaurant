using UnityEngine;
using System.Collections;

public class EventManager : MonoBehaviour {
	public delegate void UpdateEmployeesAction();
	public static event UpdateEmployeesAction OnUpdateEmployees;

	public delegate void UpdateCandidatesAction();
	public static event UpdateEmployeesAction OnUpdateCandidates;

	public delegate void UpdateIngredientsAction();
	public static event UpdateEmployeesAction OnUpdateIngredients;

	public delegate void UpdateAdvertisementsAction();
	public static event UpdateEmployeesAction OnUpdateAdvertisements;

	public delegate void UpdateEquipmentsAction();
	public static event UpdateEmployeesAction OnUpdateEquipments;

	public delegate void UpdateMenuAction();
	public static event UpdateEmployeesAction OnUpdateMenu;
	
	public static void UpdateEmployees(){
		if(OnUpdateEmployees != null)
			OnUpdateEmployees ();
	}

	public static void UpdateCandidates(){
		if(OnUpdateCandidates != null)
			OnUpdateCandidates ();
	}

	public static void UpdateIngredients(){
		if (OnUpdateIngredients != null)
			OnUpdateIngredients ();
	}

	public static void UpdateAdvertisements(){
		if (OnUpdateAdvertisements != null)
			OnUpdateAdvertisements ();
	}

	public static void UpdateEquipments(){
		if (OnUpdateEquipments != null)
			OnUpdateEquipments ();
	}

	public static void UpdateMenu(){
		if (OnUpdateMenu != null)
			OnUpdateMenu ();
	}
}
