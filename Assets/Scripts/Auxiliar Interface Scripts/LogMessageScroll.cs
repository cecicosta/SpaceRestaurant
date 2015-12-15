using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using System.Collections.Generic;

public class LogMessageScroll : MonoBehaviour {
	public LogTextBox logTextBox;
	List<LogTextBox> logs = new List<LogTextBox>();
	public int maxLogSize;
	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
		if(GameLog.logs.Count > 0){
			LogTextBox text = Instantiate(logTextBox);
			text.transform.SetParent(this.transform);
			text.transform.localScale = new Vector3(1,1,1);
			text.textLog.text = GameLog.logs[0];
			logs.Add(text);
			GameLog.logs.RemoveAt(0);
		}	
		if(logs.Count > maxLogSize){
			logs.Remove(logs[0]);
		}
	}
}
