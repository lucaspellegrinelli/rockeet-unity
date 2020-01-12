using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class ShowArrows : MonoBehaviour {

	public Transform topArrowBody;
	public Transform topArrowHead;
	public Transform botArrowBody;
	public Transform botArrowHead;
	public Transform speedArrowBody;
	public Transform speedArrowHead;
	private float startX;

	// Use this for initialization
	void Start () {
		startX = topArrowHead.position.x;
	}
	
	// Update is called once per frame
	void Update () {
		if(GameObject.Find("Mesh").GetComponent<SpriteRenderer>().enabled && PlayerPrefsX.GetBool("ShowArrows")){
			ShowArrowPropel();
			ShowArrowGravity();
			ShowArrowSpeed();
		}
	} 

	void ShowArrowPropel(){
		if(GameObject.FindWithTag("Player").GetComponent<RocketBase>().IsPressed()){
			topArrowBody.GetComponent<SpriteRenderer>().enabled = true;
			topArrowHead.GetComponent<SpriteRenderer>().enabled = true;
			float yLenght = topArrowBody.transform.localScale.y;
			float yPos = (3.09f * yLenght) / -0.67f;
			GameObject.Find("TopHead").GetComponent<Transform>().localPosition = new Vector3(topArrowHead.position.x, yPos, topArrowHead.position.z);
		}
		else{
			topArrowBody.GetComponent<SpriteRenderer>().enabled = false;
			topArrowHead.GetComponent<SpriteRenderer>().enabled = false;
		}
	}

	void ShowArrowGravity(){
		if(!GameObject.FindWithTag("Player").GetComponent<RocketBase>().IsPressed()){
			botArrowBody.GetComponent<SpriteRenderer>().enabled = true;
			botArrowHead.GetComponent<SpriteRenderer>().enabled = true;
			float gravityMult = GameObject.FindWithTag("Player").GetComponent<Rigidbody2D>().gravityScale*1.5f;
			botArrowBody.transform.localScale = new Vector3(botArrowBody.transform.localScale.x, gravityMult, botArrowBody.transform.localScale.z);
			float yLenght = botArrowBody.transform.localScale.y;
			float yPos = (-1.25f * yLenght) / 0.22f;
			GameObject.Find("BotHead").GetComponent<Transform>().localPosition = new Vector3(startX, yPos+0.1f, botArrowHead.position.z);
		}
		else{
			botArrowBody.GetComponent<SpriteRenderer>().enabled = false;
			botArrowHead.GetComponent<SpriteRenderer>().enabled = false;
		}
	}

	void ShowArrowSpeed(){
		speedArrowBody.GetComponent<SpriteRenderer>().enabled = true;
		speedArrowHead.GetComponent<SpriteRenderer>().enabled = true;
		float speedY = -GameObject.FindWithTag("Player").GetComponent<Rigidbody2D>().velocity.y/10.0f;
		speedArrowBody.GetComponent<Transform>().localScale = new Vector3(speedArrowBody.GetComponent<Transform>().localScale.x, speedY, speedArrowBody.GetComponent<Transform>().localScale.z);
		float yLenght = speedArrowBody.transform.localScale.y;
		float yPos = (3.09f * yLenght) / -0.67f;
		if(speedY > 0){
			speedArrowHead.transform.localScale = new Vector3(speedArrowHead.transform.localScale.x, -Mathf.Abs(speedArrowHead.transform.localScale.y), speedArrowHead.transform.localScale.z);
		}
		else{
			speedArrowHead.transform.localScale = new Vector3(speedArrowHead.transform.localScale.x, Mathf.Abs(speedArrowHead.transform.localScale.y), speedArrowHead.transform.localScale.z);
		}
		GameObject.Find("TopSpeedHead").GetComponent<Transform>().localPosition = new Vector3(speedArrowHead.position.x, yPos, speedArrowHead.position.z);
	}
}
