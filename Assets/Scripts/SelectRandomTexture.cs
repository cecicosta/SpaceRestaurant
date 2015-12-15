using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class SelectRandomTexture : MonoBehaviour {

	public List<Texture> textures = new List<Texture>();
	static System.Random rand = new System.Random ();
	public MeshRenderer render;
	// Use this for initialization
	void Start () {
		if (textures.Count == 0)
			return;
		render.material.SetTexture("_MainTex", textures[rand.Next(0,textures.Count)]);
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
