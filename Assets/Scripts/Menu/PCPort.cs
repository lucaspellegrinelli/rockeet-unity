using UnityEngine;
using System.Collections;

public class PCPort : MonoBehaviour {
	
	// Use this for initialization
	void Start () {
		GameObject x = Instantiate(gameObject, transform.position, transform.rotation) as GameObject;
		x.transform.parent = Camera.main.transform;
		x.GetComponent<PCPort>().enabled = false;
		x.GetComponent<Transform>().FindChild("Body1").GetComponent<SpriteRenderer>().enabled = true;
		x.GetComponent<Transform>().FindChild("Body2").GetComponent<SpriteRenderer>().enabled = true;
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
