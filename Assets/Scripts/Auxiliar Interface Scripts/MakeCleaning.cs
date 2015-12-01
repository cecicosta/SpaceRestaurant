using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class MakeCleaning : MonoBehaviour {
	public Text value;

	private EstablishmentManagement establishmentManager;
	private Establishment establishment;
	
	// Use this for initialization
	void Awake () {
		establishmentManager = EstablishmentManagement.GetInstance ();
		if (establishmentManager == null)
			Debug.LogError ("EstablishmentManager object is null");
		establishment = establishmentManager.establishment;
		if (establishment == null) {
			Debug.LogError("Failed to access establishment");
		}
	}

	void OnEnable(){
		value.text = establishment.CleaningCosts.ToString ();
	}

	public void DoCleaning(){
		establishment.DoCleaning();
		value.text = establishment.CleaningCosts.ToString ();
	}

}
