using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class CheckLoginInfo : MonoBehaviour {
	public InputField eMailInput;
	public InputField nameInput;
	public Button button;
	// Use this for initialization
	void Start () {
	
	}

	// Update is called once per frame
	void Update () {
		if (eMailInput.text == "" || nameInput.text == "")
			button.interactable = false;
		else
			button.interactable = true;
	}
}
