using UnityEngine;
using System.Collections;

public class EstablishmentManagement{ 
	public Establishment establishment;

	private EstablishmentManagement(){
		establishment = new Establishment ();
	
	}
	private static EstablishmentManagement establishment_man = null;
	public static EstablishmentManagement GetInstance(){
		if (establishment_man == null) {
			establishment_man = new EstablishmentManagement ();

			if(!establishment_man.establishment.Initiate()){
				Debug.LogError("Could not initiate some essencial resources.");
				return null;
			}
		}
		return establishment_man;
	}
}
