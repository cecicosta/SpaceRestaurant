using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class StatusPanel : MonoBehaviour {

	public Text cash;
	public Text satisfaction;
	public Text dirtness;

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

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
		cash.text = establishment.Cash().ToString();
		satisfaction.text = establishment.Satisfaction().ToString();
		dirtness.text = establishment.Dirtiness().ToString();
	}
}
