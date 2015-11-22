using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Marketing {
	public Marketing(){
	}	

	public bool Initiate(){
		return true;
	}

	public List<Promotion> QueryAvailablePromotion(){
		return null;
	}
	public List<Advertisement> QueryAvailableAdvertisements(){
		return null;
	}
	
	private int satisfaction;
	public int Satisfaction {
		get{
			return satisfaction;
		}
		set{
			satisfaction = value;
		}
	}

	private List<Advertisement> advertisements;
	private List<Promotion> promotions;
}
