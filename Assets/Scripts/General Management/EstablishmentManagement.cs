using UnityEngine;
using System.Collections;

public class EstablishmentManagement{
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

	public const int kMaxActionPoints = 5;
	public const int kConvertionFactor = 2; 
	public Establishment establishment;
}
