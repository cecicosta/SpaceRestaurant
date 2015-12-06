using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class LogMessageScroll : MonoBehaviour {
	public LogTextBox logTextBox;
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
			GameLog.logs.RemoveAt(0);
		}	
	}
}
