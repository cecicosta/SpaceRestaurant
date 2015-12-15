using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class PlayTutorial : MonoBehaviour {
	public List<GameObject> pages;
	GameObject currentPage;

	// Use this for initialization
	void Start () {
		if (pages.Count > 0) {
			currentPage = pages [0];
			currentPage.SetActive(true);
		}
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	void OnEnable(){
		if (pages.Count > 0) {
			currentPage = pages [0];
			currentPage.SetActive(true);
		}
	}

	public void Next(){
		int currentIndex = pages.IndexOf (currentPage);
		if (currentIndex + 1 < pages.Count) {
			currentPage.SetActive (false);
			currentPage = pages [currentIndex + 1];
			currentPage.SetActive (true);
		} else {
			gameObject.SetActive(false);
			currentPage.SetActive (false);
			currentPage = pages [0];
		}
	}
	
	public void Back(){
		int currentIndex = pages.IndexOf (currentPage);
		if (currentIndex - 1 >= 0) {
			currentPage.SetActive(false);
			currentPage = pages[currentIndex-1];
			currentPage.SetActive(true);
		}
	}

	public void Skip(){
		if (currentPage != null) {
			currentPage.SetActive(false);
			currentPage = pages[0];
			gameObject.SetActive(false);
		}
	}

}
