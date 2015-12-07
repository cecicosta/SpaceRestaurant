using UnityEngine;
using System.Collections;

public class QuitGame : MonoBehaviour {

	private EstablishmentManagement establishmentManager;
	// Use this for initialization
	void Awake () {
		establishmentManager = EstablishmentManagement.GetInstance ();
		if (establishmentManager == null)
			Debug.LogError ("EstablishmentManager object is null");
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	public void SaveAndQuit(){
		establishmentManager.LocalSaveState ();
		Application.Quit ();
	}

	public void Quit(){
		Application.Quit ();
	}
}
