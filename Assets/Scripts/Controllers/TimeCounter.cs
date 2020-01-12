using UnityEngine;
using System.Collections;

public class TimeCounter : MonoBehaviour {

	private bool startCount = false;
	private float currentTimePlaying;


	// Use this for initialization
	void Start () {
		StartCount();
	}
	
	// Update is called once per frame
	void Update () {
//		print("startCount: "+startCount);
		if(startCount)
			currentTimePlaying += Time.deltaTime;
	}

	public float GetCurrentTimePlaying(){
		return currentTimePlaying;
	}

	public void StartCount(){
		startCount = true;
	}

	public void StopCount(){
		startCount = false;
	}

	public bool IsCounting(){
		return startCount;
	}
}
