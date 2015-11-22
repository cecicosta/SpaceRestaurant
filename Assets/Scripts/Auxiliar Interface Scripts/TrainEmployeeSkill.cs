using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class TrainEmployeeSkill : MonoBehaviour {

	public Text candidateName; 
	private EstablishmentManagement establishmentManager;
	
	// Use this for initialization
	void Awake () {
		establishmentManager = EstablishmentManagement.GetInstance ();
		if (establishmentManager == null)
			Debug.LogError ("EstablishmentManager object is null");
	}
	
	public void TrainSkill(){
		if (!establishmentManager.establishment.TrainLevel(candidateName.text)) {
			Debug.Log ("Error to train employee");
		}
		EventManager.UpdateEmployees ();
	}
}
