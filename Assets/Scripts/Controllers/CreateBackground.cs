using UnityEngine;
using System.Collections;

public class CreateBackground : MonoBehaviour {

	public GameObject bg;
	public float height;
	public int howMuch;
	
	// Use this for initialization
	void Start () {

	}
	
	// Update is called once per frame
	void Update () {
		float heightCurrent = height * howMuch - 1;
		if(GameObject.FindWithTag("Player").GetComponent<Transform>().position.y >= (heightCurrent - 2 * height)){
			GameObject temp = Instantiate(bg, new Vector3(GameObject.Find("Background").transform.position.x, heightCurrent, 0), Quaternion.identity) as GameObject;
			temp.transform.parent = GameObject.Find("BG's").transform;
			howMuch++;
		}
	}
}
