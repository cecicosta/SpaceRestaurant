using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Equipment{
	public Equipment(){
		available = true;
	}
	public Equipment(Equipment e){
		cost = e.cost;
		name = e.name;
		effect = e.effect;
		description = e.description;
		available = e.available;
	}
	public double cost;
	public string name;
	public string effect;
	public string description;
	public bool available;
}

public class EquipmentsProvider {
	
	public EquipmentsProvider(){
		equipments = new List<Equipment> ();
	}
	public bool Initiate(){
		string equipments_files = System.IO.File.ReadAllText ("Assets/equipments.txt");
		if(equipments_files.CompareTo("") == 0){
			return false;
		}
		string[] lines = equipments_files.Split('\n');
		foreach(string str in lines) {
			string[] fields = str.Split('\t');
			BuildEquipmentsList(fields);
		}
		return true;
	}
	
	private void BuildEquipmentsList(string[] fields){
		Equipment equipment = new Equipment();
		equipment.name = fields [0];
		equipment.effect = fields [1];
		System.Double.TryParse (fields [2], out equipment.cost);
		equipment.description = fields [3];
		equipments.Add (equipment);
	}

	//Returns a copy of the equipments list
	public List<Equipment> GetEquipmentsList(){
		List<Equipment> copy = new List<Equipment>();
		foreach (Equipment e in equipments) {
			Equipment e_cpy = new Equipment(e);
			copy.Add(e_cpy);
		}
		return copy;
	}
	
	public Equipment GetEquipment(string name){
		Equipment equipment = 
			equipments.Find (
				x => x.name == name);
		return equipment;
	}
	
	public bool SetAvailable(string name, bool available){
		Equipment equipment = 
			equipments.Find (
				x => x.name == name);
		if (equipment == null)
			return false;
		
		equipment.available = available;
		return true;
	}

	public void PrettyPrint(){
		foreach (Equipment equip in equipments) {
			Debug.Log (equip.name);
			Debug.Log(equip.effect);
			Debug.Log(equip.cost);
			Debug.Log(equip.description);
		}
	}
	public List<Equipment> equipments;
}
