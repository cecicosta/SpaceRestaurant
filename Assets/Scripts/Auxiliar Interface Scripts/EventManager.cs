using UnityEngine;
using System.Collections;

public class EventManager : MonoBehaviour {
	public delegate void UpdateEmployeesAction();
	public static event UpdateEmployeesAction OnUpdateEmployees;

	public delegate void UpdateCandidatesAction();
	public static event UpdateEmployeesAction OnUpdateCandidates;

	public delegate void UpdateIngredientsAction();
	public static event UpdateEmployeesAction OnUpdateIngredients;
	
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
}
