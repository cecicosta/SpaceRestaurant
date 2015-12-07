using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class CheckGameOverCondition : MonoBehaviour {

	public Image gameOverScreen;
	public Text gameOverMessage;
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
	
	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		if (establishment.game_over_condition) {
			gameOverScreen.gameObject.SetActive(true);
			gameOverMessage.text = establishment.game_over_message;
		}
	}

	public void BackToMain(){
	}

	public void Quit(){
	}
}
