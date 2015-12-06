using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;

public class GameTranslator: Singleton<GameTranslator> {
	private static Dictionary<string, string> translations = new Dictionary<string, string> ();
	
	void Awake(){
		LoadTokens ();
	}

	void Start(){
		TranslateGUI ();
	}

	private void LoadTokens(){
		string tokens = System.IO.File.ReadAllText ("Assets/game_tokens.txt");
		if(tokens.CompareTo("") == 0){
			Debug.LogError("Failed to load tokens.");
			return;
		}
		ParseFile (tokens);
	}
	private void ParseFile(string file){
		string[] pairs = file.Split ('\n');
		foreach(string s in pairs){
			string[] pair = s.Split('\t');
			translations.Add(pair[0], pair[1]);
		}
	}

	private void TranslateGUI(){
		Text[] labels = GetComponentsInChildren<Text> (true);
		foreach (Text t in labels) {
			t.text = Translate(t.text);
		}
	}

	public string Translate(string key){
		if (!translations.ContainsKey (key)) {
			Debug.Log("Key not found: " + key);
			return "";
		}
		Debug.Log("Key found: " + key);
		Debug.Log ("token: " + translations [key]);
		return translations[key];
	}
}
