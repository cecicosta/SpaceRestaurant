using UnityEngine;
using System.Collections;

public class PauseGame : MonoBehaviour {

	private EstablishmentManagement establishmentManager;
	// Use this for initialization
	void Awake () {
	}
	
	// Update is called once per frame
	public void Pause () {
		establishmentManager = EstablishmentManagement.GetInstance ();
		if (establishmentManager == null) {
			Debug.LogError ("EstablishmentManager object is null");
			return;
		}
		establishmentManager.Pause ();
	}
	public void Resume(){
		establishmentManager = EstablishmentManagement.GetInstance ();
		if (establishmentManager == null) {
			Debug.LogError ("EstablishmentManager object is null");
			return;
		}
		establishmentManager.Resume ();
	}
}
