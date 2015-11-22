using UnityEngine;
using System.Collections;

public class ToggleActive : MonoBehaviour {
	public bool initialActieveState;
	public void Start(){
		gameObject.SetActive (initialActieveState);
	}
	public void ToggleActiveState(){
		initialActieveState = ! initialActieveState;
		gameObject.SetActive (initialActieveState);
	}
}
