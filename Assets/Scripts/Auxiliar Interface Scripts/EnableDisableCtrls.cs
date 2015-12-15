using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class EnableDisableCtrls : MonoBehaviour {

	// Use this for initialization
	void Start () {
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	public void CtrlsDisable(){
		Button[] buttons = GetComponentsInChildren<Button> ();
		foreach (Button b in buttons) {
			b.interactable = false;
		}
	}
	public void CtrlsEnable(){
		Button[] buttons = GetComponentsInChildren<Button> ();
		foreach (Button b in buttons) {
			b.interactable = true;
		}
	}


}
