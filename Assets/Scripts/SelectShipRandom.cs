using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class SelectShipRandom : MonoBehaviour {
	static System.Random rand = new System.Random ();
	public List<GameObject> ships =  new List<GameObject>();
	// Use this for initialization
	void Start () {
		if (ships.Count == 0)
			return;
		Animator anim;
		
		foreach(GameObject obj in ships){
			obj.SetActive(false);
		}

		ships [rand.Next (0, ships.Count)].SetActive (true);
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
