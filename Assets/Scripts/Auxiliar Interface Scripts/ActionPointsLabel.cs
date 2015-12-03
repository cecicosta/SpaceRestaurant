using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class ActionPointsLabel : MonoBehaviour {

	public Text actionPointsField;

	private EstablishmentManagement establishmentManager;
	// Use this for initialization
	void Awake () {
		establishmentManager = EstablishmentManagement.GetInstance ();
		if (establishmentManager == null)
			Debug.LogError ("EstablishmentManager object is null");
	}
	
	// Update is called once per frame
	void Update () {
		actionPointsField.text = establishmentManager.establishment.action_points.ToString ();
	}
}
