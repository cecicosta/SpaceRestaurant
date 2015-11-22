using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;

public class DismissEmployee : MonoBehaviour {
	
	public Text candidateName; 
	private EstablishmentManagement establishmentManager;

	// Use this for initialization
	void Awake () {
		establishmentManager = EstablishmentManagement.GetInstance ();
		if (establishmentManager == null)
			Debug.LogError ("EstablishmentManager object is null");
	}

	public void Dismiss(){
		if (!establishmentManager.establishment.Dismiss(candidateName.text)) {
			Debug.Log ("Error to dismiss employee");
		}
		EventManager.UpdateEmployees ();
	}
}
