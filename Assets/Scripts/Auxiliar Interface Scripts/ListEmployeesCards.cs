using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class ListEmployeesCards : MonoBehaviour {
	public EmployeeCard employeeCard;
	private List<EmployeeCard> cards = new List<EmployeeCard> ();
	private EstablishmentManagement establishmentManager;
	// Use this for initialization
	void Awake () {
		establishmentManager = EstablishmentManagement.GetInstance ();
		if (establishmentManager == null)
			Debug.LogError ("EstablishmentManager object is null");
	}
	
	void OnEnable() {
		CreateCards ();
		EventManager.OnUpdateEmployees += CreateCards;
	}

	void OnDisable(){
		EventManager.OnUpdateEmployees -= CreateCards;
	}
	
	void CreateCards(){
		if(cards.Count != 0){
			foreach(EmployeeCard c in cards){
				Destroy(c.gameObject);
			}
			cards.Clear();
		}
		List<Employee> employees = establishmentManager.
			establishment.alien_resources.GetEmployeesList ();
		foreach(Employee e in employees){
			EmployeeCard card = Instantiate(employeeCard);
			card.transform.SetParent(this.transform);
			card.transform.localScale = new Vector3(1,1,1);
			//TODO: find image by candidate name
			card.name.text = e.name.ToString();
			card.profession.text = e.type.ToString();
			card.skill.text = e.level.ToString();
			card.happiness.text = e.happiness.ToString();
			card.salary.text = e.Salary.ToString();
			card.cost.text = e.DismissCosts.ToString();
			cards.Add(card);
		}
	}
}
