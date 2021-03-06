﻿using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class ChangePrices : MonoBehaviour {
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

	public void IncreasePrices(){
		establishment.IncreasePrices ();
	}

	public void DecreasePrices(){
		establishment.DecreasePrices ();
	}
}
