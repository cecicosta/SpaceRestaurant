﻿using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Marketing {
	public Marketing(){
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
	}

	private List<Advertisement> advertisements;
	private List<Promotion> promotions;
}
