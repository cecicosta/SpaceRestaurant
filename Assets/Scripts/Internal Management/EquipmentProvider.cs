using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Text.RegularExpressions;

public class Equipment{
	public Equipment(){
		available = true;
		variable_modifiers = new List<Modifier> ();
		constant_modifiers = new List<Modifier> ();
	}
	public Equipment(Equipment e){
		price = e.price;
		name = e.name;
		effect = e.effect;
		description = e.description;
		available = e.available;
		variable_modifiers = new List<Modifier> ();
		constant_modifiers = new List<Modifier> ();
		foreach (Modifier mod in e.variable_modifiers) {
			variable_modifiers.Add(new Modifier(mod));
		}
		foreach (Modifier mod in e.constant_modifiers) {
			constant_modifiers.Add(new Modifier(mod));
		}
	}

	public void SaveObjectState(){
		EstablishmentManagement.SaveAttribute (name);
		EstablishmentManagement.SaveAttribute (price);
		EstablishmentManagement.SaveAttribute (description);
		EstablishmentManagement.SaveAttribute (effect);
		EstablishmentManagement.SaveAttribute (available);
		EstablishmentManagement.SaveAttribute (variable_modifiers.Count);
		foreach(Modifier m in variable_modifiers){
			m.SaveObjectState();
		}
		EstablishmentManagement.SaveAttribute (constant_modifiers.Count);
		foreach(Modifier m in constant_modifiers){
			m.SaveObjectState();
		}
	}
	
	public void LoadObjectState(){
		EstablishmentManagement.LoadAttribute (out name);
		EstablishmentManagement.LoadAttribute (out price);
		EstablishmentManagement.LoadAttribute (out description);
		EstablishmentManagement.LoadAttribute (out effect);
		EstablishmentManagement.LoadAttribute (out available);
		int size;
		EstablishmentManagement.LoadAttribute (out size);
		for(int i=0; i<size; i++){
			Modifier m = new Modifier();
			m.LoadObjectState();
			variable_modifiers.Add(m);
		}
		EstablishmentManagement.LoadAttribute (out size);
		for(int i=0; i<size; i++){
			Modifier m = new Modifier();
			m.LoadObjectState();
			variable_modifiers.Add(m);
		}
	}

	public double price;
	public string name;
	public string effect;
	public List<Modifier> variable_modifiers;
	public List<Modifier> constant_modifiers;
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
		//TODO:String validation
		equipment.variable_modifiers = GetModifier(fields[2].Split(','));
		equipment.constant_modifiers = GetModifier(fields[3].Split(','));


		System.Double.TryParse (fields [4], out equipment.price);
		equipment.description = fields [5];
		equipments.Add (equipment);
	}

	public List<Modifier> GetModifier(string[] modifier_descriptor){
		List<Modifier> modifiers = new List<Modifier>();

		if (modifier_descriptor == null)
			return modifiers;

		foreach(string s in modifier_descriptor){
			string[] fields = s.Split('/');
			if (s.Length < 2) //Must have at least a pair (value, attribute)
				continue;
			Modifier mod = new Modifier();
			mod.value = fields[0];
			mod.attribute = fields[1];
			if(fields.Length > 2){
				for(int i=2; i<fields.Length; i++){
					mod.arguments.Add(fields[i]);
				}
			}
			modifiers.Add(mod);
		}
		return modifiers;
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

	public bool RemoveEquipment(Equipment equip){
		Equipment existing = equipments.Find(x => x.name == equip.name);
		if (existing == null)
			return false;
		return equipments.Remove (existing);
	}
	
	public bool AddEquipment(Equipment equip){
		Equipment existing = equipments.Find(x => x.name == equip.name);
		if (existing != null)
			return false;
		equipments.Add (equip);
		return true;
	}

	public void PrettyPrint(){
		foreach (Equipment equip in equipments) {
			Debug.Log (equip.name);
			Debug.Log(equip.effect);
			Debug.Log(equip.price);
			Debug.Log(equip.description);
		}
	}
	public List<Equipment> equipments;
}
