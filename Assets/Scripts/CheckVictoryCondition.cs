using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class CheckVictoryCondition : MonoBehaviour {

	public Image victoryScreen;
	private EstablishmentManagement establishmentManager;
	public Establishment establishment;
	// Use this for initialization
	void Awake () {
		establishmentManager = EstablishmentManagement.GetInstance ();
		if (establishmentManager == null) {
			Debug.LogError ("EstablishmentManager object is null");
			return;
		}
		establishment = establishmentManager.establishment;
	}

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
		if (establishment.victory_condition) {
			victoryScreen.gameObject.SetActive(true);
		}
	}

	public void BackToMain(){
	}
	
	public void Quit(){
	}
}
