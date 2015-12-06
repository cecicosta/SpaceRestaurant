using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Advertising{
	public Advertising(){
		
	}
	public Advertising(Advertising ad){
		type = ad.type;
		min_reach = ad.min_reach;
		max_reach = ad.max_reach;
		price = ad.price;
	}

	public void SaveObjectState(){
		EstablishmentManagement.SaveAttribute (type);
		EstablishmentManagement.SaveAttribute (min_reach);
		EstablishmentManagement.SaveAttribute (max_reach);
		EstablishmentManagement.SaveAttribute (price);
	}
	
	public void LoadObjectState(){
		EstablishmentManagement.LoadAttribute (out type);
		EstablishmentManagement.LoadAttribute (out min_reach);
		EstablishmentManagement.LoadAttribute (out max_reach);
		EstablishmentManagement.LoadAttribute (out price);
	}

	public string type;
	public int min_reach;
	public int max_reach;
	public double price;
}
public class AdvertisementsProvider {

	private static AdvertisementsProvider advertisement_provider;
	private AdvertisementsProvider(){
		advertisements = new List<Advertising> ();
	}
	
	private bool Initiate(){
		string ads_file = System.IO.File.ReadAllText ("Assets/advertisement.txt");
		if(ads_file.CompareTo("") == 0){
			return false;
		}
		string[] lines = ads_file.Split('\n');
		foreach(string str in lines) {
			string[] fields = str.Split('\t');
			BuildAdsList(fields);
		}
		return true;
	}
	
	public static AdvertisementsProvider GetInstance(){
		if (advertisement_provider == null) {
			advertisement_provider = new AdvertisementsProvider();
			if(!advertisement_provider.Initiate()){
				Debug.LogError("Failed to load ads");
				advertisement_provider = null;
			}
		}
		return advertisement_provider;
	}
	
	private void BuildAdsList(string[] fields){
		Advertising advertisement = new Advertising();
		advertisement.type = fields [0];
		string[] range = fields [1].Split ('-');
		System.Int32.TryParse (range[0], out advertisement.min_reach);
		System.Int32.TryParse (range[1], out advertisement.max_reach);
		System.Double.TryParse(fields [2], out advertisement.price);
		advertisements.Add (advertisement);
	}
	
	//Returns a copy of the advertisements list
	public List<Advertising> GetAdsList(){
		List<Advertising> copy = new List<Advertising>();
		foreach (Advertising ad in advertisements) {
			copy.Add(new Advertising(ad));
		}
		return copy;
	}
	
	public Advertising GetAd(string type){
		Advertising ad = 
			advertisements.Find (
				x => x.type == type);
		return ad;
	}

	private List<Advertising> advertisements;
}
