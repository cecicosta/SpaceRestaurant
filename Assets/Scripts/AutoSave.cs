using UnityEngine;
using System.Collections;

public class AutoSave : Singleton<AutoSave> {

	private float lastSaveTime = 0;
	public int minutesBetweenSave;

	private EstablishmentManagement establishmentManager;
	// Use this for initialization
	void Awake () {
		establishmentManager = EstablishmentManagement.GetInstance ();
		if (establishmentManager == null)
			Debug.LogError ("EstablishmentManager object is null");
	}
	
	// Update is called once per frame
	void Update () {
		if (Time.time - lastSaveTime > minutesBetweenSave*60) {
			lastSaveTime = Time.time;
			SaveRoutine();
		}
	}

	void SaveRoutine(){
		establishmentManager.SaveCacheGameData();
		string saveDataBase64 = establishmentManager.SaveDataToBase64();
		UserService.Instance.metaData = saveDataBase64;
		UserService.Instance.CallSendScore();
		establishmentManager.LocalSaveState();
	}

	void LoadRoutine(){

	}
}
