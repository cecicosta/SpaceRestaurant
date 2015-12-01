using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Infrastructure{

	public Infrastructure(){
		equip_provider = new EquipmentsProvider ();
		equipments = new List<Equipment> ();
	}

	public bool Initiate(){
		if (!LoadAttributes ()) {
			return false;
		}
		dirtiness = initial_dirtness;

		return equip_provider.Initiate ();
	}

	public bool MakeCleaning(){
		dirtiness -=dirtness_decrement_clean;
		return true;
	}

	public bool DirtnessIncrease(){
		dirtiness += dirtness_increment_day;
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
		set{
			dirtiness = value;
		}
	}

	public static bool LoadAttributes(){
		AttributesManager at_m = AttributesManager.GetInstance ();
		if (at_m == null)
			return false;
		initial_dirtness = at_m.IntValue ("initial_dirtness");
		dirtness_increment_day = at_m.IntValue ("dirtness_increment_day");
		dirtness_decrement_clean = at_m.IntValue ("dirtness_decrement_clean");
		return true;
	}
	private static int initial_dirtness;
	private static int dirtness_increment_day;
	private static int dirtness_decrement_clean;
}
