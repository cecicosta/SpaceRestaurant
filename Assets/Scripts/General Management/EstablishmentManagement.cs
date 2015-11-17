using UnityEngine;
using System.Collections;

public class EstablishmentManagement{ 
	public Establishment establishment;

	private EstablishmentManagement(){
	
	}
	private static EstablishmentManagement establishment_man;
	public static EstablishmentManagement GetInstance(){
		if (establishment_man == null) {
			establishment_man = new EstablishmentManagement ();
		}
		return establishment_man;
	}
	
	public void RestoreActionPoints(){
		establishment.action_points = kMaxActionPoints;
	}
	public bool ConvertActionPointsToResponsePoint(){
		if (establishment.action_points - kConvertionFactor < 0)
			return false;
		establishment.action_points -= kConvertionFactor;
		establishment.response_points++;
		return true;
	}

	private const int kMaxActionPoints = 5;
	private const int kConvertionFactor = 2;
}
