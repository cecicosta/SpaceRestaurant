using UnityEngine;
using System.Collections;

public class ResetGame : MonoBehaviour {
	
	// Update is called once per frame
	void Update () {
	
	}

	public void Reset(){
		Application.LoadLevel (Application.loadedLevelName);
		EstablishmentManagement.EraseInstance ();
		EstablishmentManagement.GetInstance ();

	}
}
