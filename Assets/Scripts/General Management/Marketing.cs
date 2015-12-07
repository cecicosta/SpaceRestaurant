using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Marketing {
	public Marketing(){
		advertisements = new List<Advertising> ();

	}	

	public bool Initiate(){
		if (!LoadAttributes ()) {
			return false;
		}

		satisfaction = initialSatisfaction;
		adsProvider = AdvertisementsProvider.GetInstance ();
		if (adsProvider == null) {
			Debug.LogError("Ads is null");
			return false;
		}
		return true;
	}
	
	public List<Advertising> AvailableAdvertisements(){
		return adsProvider.GetAdsList();
	}

	public bool HireAdvertisement(string type){
		Advertising ad = adsProvider.GetAd (type);
		if (ad == null)
			return false;
		Advertising ad_cpy = new Advertising (ad);
		advertisements.Add (ad_cpy);
		return true;
	}

	public List<Advertising> GetAdvertisementsList(){
		return adsProvider.GetAdsList ();
	}
							 
	public List<Advertising> GetActiveAdvertisementsList(){
		List<Advertising> cpy = new List<Advertising> ();;
		foreach(Advertising ad in advertisements){
			cpy.Add(new Advertising(ad));
		}
		return cpy;
	}

	public AdvertisementsProvider GetProvider(){
		return adsProvider;
	}

	public Advertising GetAd(string type){
		return advertisements.Find (x => x.type == type);
	}

	public bool WasHired(string type){
		return advertisements.Find(x => x.type == type) != null;
	}

	public void ClearHiredAds(){
		advertisements.Clear ();
	}
	
	private int satisfaction;
	public int Satisfaction {
		get{
			return satisfaction;
		}
		set{
			if(value < 0)
				satisfaction = 0;
			else if (value > 100)
				satisfaction = 100;
			else 
				satisfaction = value;
		}
	}

	public static bool LoadAttributes(){
		AttributesManager at_m = AttributesManager.GetInstance ();
		if (at_m == null)
			return false;
		initialSatisfaction = at_m.IntValue ("initial_satisfaction");
		return true;
	}

	public void SaveObjectState(){
		EstablishmentManagement.SaveAttribute (advertisements.Count);
		foreach(Advertising ad in advertisements){
			ad.SaveObjectState();
		}
	}

	public void LoadObjectState(){
		int count;
		advertisements.Clear ();
		EstablishmentManagement.LoadAttribute (out count);
		for(int i=0; i<count; i++){
			Advertising ad = new Advertising();
			ad.LoadObjectState();
			advertisements.Add(ad);
		}
	}

	public static int kMaxSatisfaction = 100;
	private static int initialSatisfaction;
	private AdvertisementsProvider adsProvider;
	private List<Advertising> advertisements;
}
