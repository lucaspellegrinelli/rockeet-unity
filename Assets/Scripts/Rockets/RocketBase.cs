using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class RocketBase : MonoBehaviour {

	private float pushForce;
	private bool isPressed = false;
	[HideInInspector]
	public List<float> hitGroundVelocity;
	private int hitGroundTimes;
	public Animator fireAnimator;
	private Sprite rocketSprite;
	private bool hasFlag;
	private float maxFuel;
	private float currentFuel;
	public float fuelDowngradePerFrame;
	public GameObject explosion;
	public float explosionVelocity;
	private float projectedToGravity;

	// Use this for initialization
	protected void Start () {
		hasFlag = false;
		hitGroundVelocity = new List<float>();
		fireAnimator.SetBool("isFiring", false);
	}
	
	// Update is called once per frame
	protected void Update () {
		if(InstructionsController.isMessageOnScreen()){
			GetComponent<RocketBase>().SetPlayerVelocity(0.0001f);
		}
	}


	protected void FixedUpdate () {

		if(GetPlayerVelocity() == 0f)
			isPressed = false;
		
		if(!EndGameTrigger.GetGameIsGoing())
			return;

		
		// ---------------------------------------------------
		if(currentFuel > 0){
			if(InputController.IsPressed())
				isPressed = true;
			else
				isPressed = false;
		}
		else{
			isPressed = false;
		}
		// ----------------------------------------------------


		if(isPressed){
			GetComponent<Rigidbody2D>().AddForce(new Vector2(0, pushForce * 10 * Time.deltaTime));
			RemoveFuel(fuelDowngradePerFrame);
		}
		
		GameObject.Find("Fire").GetComponent<Animator>().SetBool("isFiring", isPressed);
	}

	void OnCollisionEnter2D(Collision2D c){
		if(c.transform.tag == "Floor"){
			float h = c.relativeVelocity.y;
			if(h <= explosionVelocity){ //Hit with too much force
				hasFlag = true;
				GetComponent<Rigidbody2D>().velocity = new Vector2(0, 0);
				hitGroundVelocity.Add(99999); //High value to test later
				hitGroundTimes++;
				HitWithTooMuchVelocity();
			}
			else{
				hitGroundVelocity.Add(h);
				hitGroundTimes++;
			}
		}
	}

	void HitWithTooMuchVelocity(){
		GameObject.Find("Mesh").GetComponent<SpriteRenderer>().enabled = false;
		Instantiate(explosion, transform.position, Quaternion.identity);
		GameObject.Find("TopSpeedBody").GetComponent<SpriteRenderer>().enabled = false;
		GameObject.Find("TopSpeedHead").GetComponent<SpriteRenderer>().enabled = false;
		GameObject.Find("TopSpeedHead").SetActive(false);
		GameObject.Find("BotBody").GetComponent<SpriteRenderer>().enabled = false;
		GameObject.Find("BotHead").GetComponent<SpriteRenderer>().enabled = false;
		GameObject.Find("TopBody").GetComponent<SpriteRenderer>().enabled = false;
		GameObject.Find("TopHead").GetComponent<SpriteRenderer>().enabled = false;
	}

	void OnTriggerEnter2D(Collider2D other) {
		if(other.transform.tag == "Flag"){
			hasFlag = true;
			Destroy(other.gameObject);
		}
	}

	public void SetPushForce(float _pushForce){
		pushForce = _pushForce;
	}
	
	public float GetPushForce(){
		return pushForce;
	}
	
	public float GetPlayerVelocity(){
		return GetComponent<Rigidbody2D>().velocity.y;
	}
	
	public void SetPlayerVelocity(float velocity){
		GetComponent<Rigidbody2D>().velocity = new Vector2(0, velocity);
	}
	
	public bool IsPressed(){
		return isPressed;
	}
	
	public int HowManyTimesHitGround(){
		return hitGroundTimes;
	}
	
	public float TotalHitVelocities(){
		float all = 0.0f;
		for(int i = 0; i < hitGroundVelocity.Count; i++){
			all += hitGroundVelocity[i];
		}
		if(all < 99990){
			return all;
		}
		else{
			return -1;

		}
	}

	public bool HasFlag(){
		return hasFlag;
	}

	public float GetMaxFuel(){
		return maxFuel;
	}

	public void SetMaxFuel(float max){
		maxFuel = max;
		currentFuel = maxFuel;
	}

	public void SetCurrentFuel(float curr){
		currentFuel = curr;
	}

	public void SetProjectedTo(float grav){
		projectedToGravity = grav;
	}

	public float GetProjectedTo(){
		return projectedToGravity;
	}

	public void AddFuel(float howMuch){
		float sum = (maxFuel*howMuch)/100.0f;
		currentFuel += sum;
		currentFuel = Mathf.Clamp(currentFuel, 0f, maxFuel);
	}

	public void RemoveFuel(float howMuch){
		currentFuel -= howMuch;
		currentFuel = Mathf.Clamp(currentFuel, 0f, maxFuel);
	}

	public float GetCurrentFuel(){
		return currentFuel;
	}


	bool TestClick(Vector3 pos){
		bool clicked = false;
		RaycastHit2D hit = Physics2D.Raycast(pos, Vector2.zero);
		if(hit.collider != null){
			if((hit.transform.gameObject.GetComponent("ClickableItemBase") as ClickableItemBase) != null){
				clicked = true;
			}
		}
		return clicked;
	}
}
