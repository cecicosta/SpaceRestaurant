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
		//Load texture from disk
		TextAsset bindata= Resources.Load("game_tokens") as TextAsset;
		string tokens = bindata.text;
		//string tokens = System.IO.File.ReadAllText ("Assets/game_tokens.txt");
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
			if (translations.ContainsKey (pair[0])) {
				Debug.Log("Key already exists: " + pair[0]);
				continue;
			}

			translations.Add(pair[0], pair[1]);
		}
	}

	private void TranslateGUI(){
		Text[] labels = GetComponentsInChildren<Text> (true);
		foreach (Text t in labels) {
			string trans = Translate(t.text);
			if(trans != "")
				t.text = trans;
		}
	}

	public string Translate(string key){
		if (!translations.ContainsKey (key)) {
			Debug.Log("Key not found: " + key);
			return "";
		}
		return translations[key];
	}
}
