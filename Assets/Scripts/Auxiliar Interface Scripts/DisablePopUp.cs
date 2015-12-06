using UnityEngine;
using System.Collections;

public class DisablePopUp : MonoBehaviour {

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}
	void OnDisable(){
		gameObject.SetActive (false);
	}

}
