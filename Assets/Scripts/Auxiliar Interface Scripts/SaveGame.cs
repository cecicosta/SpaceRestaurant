using UnityEngine;
using System.Collections;

public class SaveGame : MonoBehaviour {

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
	
	public void Save() {
		establishmentManager.LocalSaveState();
	}
}
