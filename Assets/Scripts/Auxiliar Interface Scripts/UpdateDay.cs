using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class UpdateDay : MonoBehaviour {

	public Text day;
	private EstablishmentManagement establishmentManager;
	private Establishment establishment;
	// Use this for initialization
	void Awake () {
		establishmentManager = EstablishmentManagement.GetInstance ();
		if (establishmentManager == null) {
			Debug.LogError ("EstablishmentManager object is null");
			return;
		}
		establishment = establishmentManager.establishment;
	}
	
	// Update is called once per frame
	void Update () {
		day.text = establishment.CurrentDay ().ToString ();
	}
}
