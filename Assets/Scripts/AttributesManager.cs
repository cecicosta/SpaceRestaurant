using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class GameAttribute{
	public GameAttribute(){
	}
	public GameAttribute(GameAttribute at){
		unit = at.unit;
		affects = at.affects;
		attribute = at.attribute;
		current_value = at.current_value;
		description = at.description;
	}
	public string unit;
	public string affects;
	public string attribute;
	public string current_value;
	public string description;
}

public class AttributesManager  {
	private static AttributesManager attributes_manager;
	private AttributesManager(){
		attributes = new List<GameAttribute> ();
	}
	
	private bool Initiate(){
		string attributes_file = System.IO.File.ReadAllText ("Assets/attributes.txt");
		if(attributes_file.CompareTo("") == 0){
			return false;
		}
		string[] lines = attributes_file.Split('\n');
		foreach(string str in lines) {
			string[] fields = str.Split('\t');
			BuildAttribute(fields);
		}
		return true;
	}
	
	public static AttributesManager GetInstance(){
		if (attributes_manager == null) {
			attributes_manager = new AttributesManager();
			if(!attributes_manager.Initiate()){
				Debug.LogError("Failed to load attributes");
				attributes_manager = null;
			}
		}
		return attributes_manager;
	}
	
	private void BuildAttribute(string[] fields){
		GameAttribute attribute = new GameAttribute();
		attribute.unit = fields [0];
		attribute.affects = fields [1];
		attribute.attribute = fields [2];
		attribute.current_value = fields [3];
		attributes.Add (attribute);
	}

	public List<GameAttribute> GetAttributesList(){
		List<GameAttribute> copy = new List<GameAttribute>();
		foreach (GameAttribute at in attributes) {
			GameAttribute d_cpy = new GameAttribute(at);
			copy.Add(d_cpy);
		}
		return copy;
	}
	
	public GameAttribute GetAttribute(string identifier){
		GameAttribute attribute = 
			attributes.Find (
				x => x.attribute == identifier);
		return attribute;
	}

	public int IntValue(string identifier){
		GameAttribute attribute = attributes.Find (x => x.attribute == identifier);
		int value;
		System.Int32.TryParse (attribute.current_value, out value);
		return value;
	}
	public double DoubleValue(string identifier){
		GameAttribute attribute = attributes.Find (x => x.attribute == identifier);
		double value;
		System.Double.TryParse (attribute.current_value, out value);
		return value;
	}
	public int[] RangeValue(string identifier){
		GameAttribute attribute = attributes.Find (x => x.attribute == identifier);
		int[] range = new int[2];
		string[] values = attribute.current_value.Split ('-');
		System.Int32.TryParse (values[0], out range[0]);
		System.Int32.TryParse (values[1], out range[1]);

		return range;
	}

	public List<GameAttribute> attributes;
}
