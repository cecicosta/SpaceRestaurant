using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class HireAdvertisement : MonoBehaviour {

	public Text advertisementName; 
	private EstablishmentManagement establishmentManager;
	private Establishment establishment;
	// Use this for initialization
	void Awake () {
		establishmentManager = EstablishmentManagement.GetInstance ();
		if (establishmentManager == null)
			Debug.LogError ("EstablishmentManager object is null");
		establishment = establishmentManager.establishment;
		if (establishment == null)
			Debug.LogError ("Establishment object is null");
	}
	
	public void Hire(){
		if (!establishment.HireAdvertisement(advertisementName.text)) {
			Debug.Log ("Error to hire ad");
		}
		Debug.Log ("Hire Ad");
		EventManager.UpdateAdvertisements ();
	}
}
