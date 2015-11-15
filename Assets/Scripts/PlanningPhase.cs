using UnityEngine;
using System.Collections;
using System.Collections.Generic;


public class PlanningPhase : MonoBehaviour {

	// Use this for initialization
	void Start () {
		/*EmployeesProvider agency = new EmployeesProvider ();
		if (agency.Initiate ())
			//agency.PrettyPrint ();
			Debug.Log ("File readed");

		IngredientsProvider ingre = new IngredientsProvider ();
		if (ingre.Initiate ()) {
			//ingre.PrettyPrint();
			Debug.Log ("Ingredients read with success");
		}
		EquipmentsProvider equip = new EquipmentsProvider ();
		if (equip.Initiate ()) {
			//equip.PrettyPrint();
			Debug.Log ("Equipments read with success");
		}

		MenuProvider menu = new MenuProvider ();
		if (menu.Initiate ()) {
			menu.PrettyPrint();
			Debug.Log ("Menu read with success");
		}*/

		AlianResources ar = new AlianResources ();
		if (ar.Initiate ()) {
			List<Candidate> candidates = ar.GetCandidatesList ();
			foreach (Candidate c in candidates) {
				c.Print ();
				ar.HireEmployee(c.name);
			}

			List<Employee> employees = ar.GetEmployeesList();
			foreach(Employee e in employees){
				Debug.Log(e.name);
				Debug.Log(e.type);
				Debug.Log(e.level);
				Debug.Log(e.hapyness);
				e.hapyness = 10;
			}
			Debug.Log ("Candidates loaded");
		
			employees = ar.GetEmployeesList();
			foreach(Employee e in employees){
				Debug.Log(e.name);
				Debug.Log(e.type);
				Debug.Log(e.level);
				Debug.Log(e.hapyness);

			}
			Debug.Log ("Candidates loaded");


		} else
			Debug.Log ("Candidates not loades");

	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
