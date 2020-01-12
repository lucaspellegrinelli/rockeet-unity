using UnityEngine;
using System.Collections;
using System;

public class InstructionsController : MonoBehaviour {

	public AnimationClip fadeIn;
	private bool debug;
	private static InstructionsController instance;
	private bool messageIn;

	void Awake(){
		instance = this;
	}

	// Use this for initialization
	void Start () {
		messageIn = true;
		debug = false;
	}
	
	// Update is called once per frame
	void Update () {
		if(GameObject.Find("Controllers").GetComponent<TimeCounter>().GetCurrentTimePlaying() > fadeIn.length+0.15f){
			if(!debug)
				Time.timeScale = 0;

			if(InputController.IsPressed())
				FadeMessageOff();
		}
	}

	void FadeMessageOff(){
		Time.timeScale = 1;
		GetComponent<Animator>().SetTrigger("fadeOut");
		GameObject.Find("BlackOut").GetComponent<Animator>().SetTrigger("Out");
		debug = true;
		messageIn = false;
	}
	
	public static bool IsReady(){
		return GameObject.Find("Controllers").GetComponent<TimeCounter>().GetCurrentTimePlaying() > instance.fadeIn.length+0.15f;
	}

	public void Destroy(){
		Destroy(gameObject);
	}

	public static bool isMessageOnScreen(){
		bool message = false;
		try{
			message = instance.messageIn;
		}
		catch(NullReferenceException ex){
			Debug.LogError("O gameobject 'Instructions' no gameplay foi destivado. Ative-o. " + ex.Message);
		}
		return message;
	}
}
