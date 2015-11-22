using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class HireEmployee : MonoBehaviour {

	public Text candidateName; 
	private EstablishmentManagement establishmentManager;
	
	// Use this for initialization
	void Awake () {
		establishmentManager = EstablishmentManagement.GetInstance ();
		if (establishmentManager == null)
			Debug.LogError ("EstablishmentManager object is null");
	}
	
	public void Hire(){
		if (!establishmentManager.establishment.Hire(candidateName.text)) {
			Debug.Log ("Error to hire employee");
		}
		EventManager.UpdateCandidates ();
	}
}
