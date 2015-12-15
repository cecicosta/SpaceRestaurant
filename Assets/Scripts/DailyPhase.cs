using UnityEngine;
using System.Collections;

public class DailyPhase : MonoBehaviour {
	
	public GameObject infoPanel;
	public GameObject clientsSpawner;
	public EnableDisableCtrls enableDisableCtrls;
	private EstablishmentManagement establishmentManager;
	private Establishment establishment;
	// Use this for initialization
	void Awake () {
		establishmentManager = EstablishmentManagement.GetInstance ();
		if (establishmentManager == null)
			Debug.LogError ("EstablishmentManager object is null");
		establishment = establishmentManager.establishment;
		if (establishment == null)
			Debug.LogError ("Establishment object is null");
	}

	public void StartDailyPhase(){
		day_phase_started = true;
		enableDisableCtrls.CtrlsDisable ();
		establishmentManager.DayPhaseSetup ();
		clientsSpawner.SetActive (true);
	}

	public void FastPhaseTime(){
		establishmentManager.FastPhaseTime ();
	}

	public void SkipPhaseTime(){
		establishmentManager.SkipPhaseTime ();
	}

	public void Update(){
		if (day_phase_started) {
			Debug.Log("Started Day Phase");
			establishmentManager.RunDayPhase ();
			if(establishmentManager.IsDayPhaseOver()){
				Debug.Log("End Day Phase");
				day_phase_started = false;
				establishmentManager.CloseDayPhase();
				enableDisableCtrls.CtrlsEnable ();
				infoPanel.SetActive(true);
				clientsSpawner.SetActive (false);
			}else{
				Debug.Log("Continue Day Phase");
			}
		}
	}

	private bool day_phase_started = false;
}
