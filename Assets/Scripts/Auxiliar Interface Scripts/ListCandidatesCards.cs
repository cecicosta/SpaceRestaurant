using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;

public class ListCandidatesCards : MonoBehaviour {
	public CandidateCard candidateCard;
	public List<RuntimeAnimatorController> images = new List<RuntimeAnimatorController>();
	private List<CandidateCard> cards = new List<CandidateCard> ();
	private EstablishmentManagement establishmentManager;
	// Use this for initialization
	void Awake () {
		establishmentManager = EstablishmentManagement.GetInstance ();
		if (establishmentManager == null)
			Debug.LogError ("EstablishmentManager object is null");
	}

	void OnEnable() {
		CreateCards ();
		EventManager.OnUpdateCandidates += CreateCards;
	}

	void OnDisable(){
		EventManager.OnUpdateCandidates -= CreateCards;
	}

	void CreateCards(){
		if(cards.Count != 0){
			foreach(CandidateCard c in cards){
				Destroy(c.gameObject);
			}
			cards.Clear();
		}
		List<Employee> employees = establishmentManager.
			establishment.alien_resources.GetCandidatesList ();
		foreach(Employee e in employees){
			CandidateCard card = Instantiate(candidateCard);
			card.transform.SetParent(this.transform);
			card.transform.localScale = new Vector3(1,1,1);

			if(images.Count > e.id)
				card.anim.runtimeAnimatorController = images[e.id];
		
			card.name.text = e.name.ToString();
			card.profession.text = e.type.ToString();
			card.skill.text = e.level.ToString();
			card.happiness.text = e.happiness.ToString();
			card.cost.text = e.HireCosts.ToString();
			card.salary.text = e.Salary.ToString();
			card.description.text = e.description;
	
			cards.Add(card);
		}
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
