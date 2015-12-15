using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class ShipSpawner : MonoBehaviour {

	public GameObject to_emit;
	public float intervalToSpawn;
	public float lifeTime;
	private float lastSpawnTime;
	private List<KeyValuePair<float,GameObject> > activeObjects = new List<KeyValuePair<float,GameObject> >();
	// Use this for initialization
	void Start () {
		lastSpawnTime = 0;
	}
	
	// Update is called once per frame
	void Update () {
		
		if(Time.time - lastSpawnTime > intervalToSpawn){
			GameObject obj = GameObject.Instantiate(to_emit);
			lastSpawnTime = Time.time;
			obj.SetActive(true);
			activeObjects.Add(new KeyValuePair<float, GameObject>(Time.time, obj));
		}
		
		foreach(KeyValuePair<float, GameObject> p in activeObjects ){
			if(Time.time - p.Key > lifeTime){
				GameObject.Destroy(p.Value.gameObject);
			}
		}
		
	}
}
