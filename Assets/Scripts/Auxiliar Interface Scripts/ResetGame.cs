using UnityEngine;
using System.Collections;

public class ResetGame : MonoBehaviour {
	
	// Update is called once per frame
	void Update () {
	
	}

	public void Reset(){
		EstablishmentManagement.EraseInstance ();
		EstablishmentManagement.GetInstance ();
		Application.LoadLevel (Application.loadedLevelName);
	}
}
