using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class ListAdvertisementsCards : MonoBehaviour {

	public AdvertisementCard advertisementCard;
	private List<AdvertisementCard> cards = new List<AdvertisementCard> ();
	private EstablishmentManagement establishmentManager;
	private Establishment establishment;
	public enum AdvertisementViwer {Available, Active};
	public AdvertisementViwer viwerType;
	// Use this for initialization
	void Awake () {
		establishmentManager = EstablishmentManagement.GetInstance ();
		if (establishmentManager == null) {
			Debug.LogError ("EstablishmentManager object is null");
			return;
		}
		establishment = establishmentManager.establishment;
	}
	
	void OnEnable() {
		CreateCards ();
		EventManager.OnUpdateAdvertisements += CreateCards;
	}
	
	void OnDisable(){
		EventManager.OnUpdateAdvertisements -= CreateCards;
	}
	
	void CreateCards(){
		if(cards.Count != 0){
			foreach(AdvertisementCard ad in cards){
				Destroy(ad.gameObject);
			}
			cards.Clear();
		}
		List<Advertising> advertisement_list = null;
		if (viwerType == AdvertisementViwer.Available) {
			advertisement_list = establishment.marketing.GetAdvertisementsList ();
		} else if(viwerType == AdvertisementViwer.Active) {
			advertisement_list = establishment.marketing.GetActiveAdvertisementsList ();
		}
		if (advertisement_list == null) {
			Debug.LogError("Error getting the ingredients list");
			return;
		}
		foreach(Advertising ad in advertisement_list){
			AdvertisementCard card = Instantiate(advertisementCard);
			card.transform.SetParent(this.transform);
			//TODO: find image by candidate name
			card.type.text = ad.type;
			card.range.text = ad.min_reach.ToString() + "-" + ad.max_reach.ToString();
			card.price.text = ad.price.ToString();
			cards.Add(card);
		}
	}	
}
