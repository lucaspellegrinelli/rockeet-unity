using UnityEngine;
using System.Collections;

public class FuelBarController : MonoBehaviour {

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
		float localScaleX = (GameObject.FindWithTag("Player").GetComponent<RocketBase>().GetCurrentFuel()*2.552289f)/GameObject.FindWithTag("Player").GetComponent<RocketBase>().GetMaxFuel();
		transform.localScale = new Vector3(transform.localScale.x, localScaleX, transform.localScale.z);
		Input.GetMouseButtonDown(0);
	}
}
