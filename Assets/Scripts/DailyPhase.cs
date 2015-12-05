using UnityEngine;
using System.Collections;

public class DailyPhase : MonoBehaviour {

	public GameObject infoPanel;
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

	public void StartDailyPhase(){
		establishmentManager.RunDailyPhase ();
		infoPanel.SetActive (true);
	}

}
