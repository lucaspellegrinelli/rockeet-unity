using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class HUD : MonoBehaviour {

	// Use this for initialization
	void Start () {

	}
	
	// Update is called once per frame
	void Update () {
		if(GameObject.Find("Controllers").GetComponent<TimeCounter>().IsCounting()){
//			ShowHeightText();
//			ShowVelocityText();
		}
		else{
			SetAllTextsOff();
		}
	}

	public void SetAllTextsOff(){
//		GameObject.Find("TextHeight").GetComponent<Text>().text = "";
//		GameObject.Find("TextVelocity").GetComponent<Text>().text = "";
	}

	private void ShowHeightText(){
		string heightText;
		float height;
		height = GameObject.FindWithTag("Player").GetComponent<Transform>().position.y;
		heightText = ((height + 2.872187f)*5).ToString("####0.0");
		if(!InstructionsController.isMessageOnScreen())
			GameObject.Find("TextHeight").GetComponent<Text>().text = ""+heightText+"\n"+"m";
		else
			GameObject.Find("TextHeight").GetComponent<Text>().text = "";

		if(Time.timeScale <= 0.98f){
			GameObject.Find("TextHeight").GetComponent<Text>().text = "";
		}
	}

	private void ShowVelocityText(){
		string velocityText;
		float velocity;
		velocity = Mathf.Abs(GameObject.FindWithTag("Player").GetComponent<Rigidbody2D>().velocity.y);
		velocityText = (velocity*10).ToString("####0.0");
		if(!InstructionsController.isMessageOnScreen())
			GameObject.Find("TextVelocity").GetComponent<Text>().text = ""+velocityText+"\n"+"km/h";
		else
			GameObject.Find("TextVelocity").GetComponent<Text>().text = "";

		if(Time.timeScale <= 0.98f){
			GameObject.Find("TextVelocity").GetComponent<Text>().text = "";
		}
	}
}
