using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Infrastructure{

	public Infrastructure(){
		equip_provider = new EquipmentsProvider ();
		equipments = new List<Equipment> ();
		dirtiness = initial_dirtness;
	}

	public bool Initiate(){
		return equip_provider.Initiate ();
	}

	public bool MakeCleaning(){
		dirtiness--;
		return true;
	}
		
	public bool BuyEquipment(string name){
		Equipment equipment = equip_provider.GetEquipment(name);
		if (equipment == null)
			return false;
		if (!equip_provider.RemoveEquipment (equipment)) {
			return false;
		}
		Equipment e_cpy = new Equipment (equipment);
		equipments.Add (e_cpy);
		return true;
	}

	public List<Equipment> GetAquiredEquipmentsList(){
		List<Equipment> copy = new List<Equipment>();
		foreach (Equipment e in equipments) {
			Equipment e_cpy = new Equipment(e);
			copy.Add(e_cpy);
		}
		return copy;
	}

	public List<Equipment> GetProviderEquipmentsList(){
		return equip_provider.GetEquipmentsList ();
	}

	private EquipmentsProvider equip_provider;
	private List<Equipment> equipments;
	private int dirtiness;

	public int Dirtiness {
		get{
			return dirtiness;
		}
	}
	private int initial_dirtness = 2;

}
