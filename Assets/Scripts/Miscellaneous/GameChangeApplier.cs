using UnityEngine;
using System.Collections;

public class GameChangeApplier : MonoBehaviour {

	// Use this for initialization
	void Start () {
		Rocket current = GameObject.Find("RocketDatabase").GetComponent<RocketsDatabase>().GetCurrentRocket();
		Map currentMap = GameObject.Find("MapDatabase").GetComponent<MapsDatabase>().GetCurrentMap();
		SetRocket(current);
		SetMap(currentMap);

		if(PlayerPrefsX.GetBool("ShowArrows") || InstructionsController.isMessageOnScreen()){
			GameObject.Find("BotBody").GetComponent<SpriteRenderer>().enabled = false;
			GameObject.Find("BotHead").GetComponent<SpriteRenderer>().enabled = false;
			GameObject.Find("TopBody").GetComponent<SpriteRenderer>().enabled = false;
			GameObject.Find("TopHead").GetComponent<SpriteRenderer>().enabled = false;
			GameObject.Find("TopSpeedBody").GetComponent<SpriteRenderer>().enabled = false;
			GameObject.Find("TopSpeedHead").GetComponent<SpriteRenderer>().enabled = false;
		}
	}

	public void SetRocket(Rocket rocket){
		float maxFuel = GameObject.FindWithTag("Player").GetComponent<RocketBase>().GetMaxFuel();
		float pushForce = GameObject.FindWithTag("Player").GetComponent<RocketBase>().GetPushForce();

		GameObject.FindWithTag("Mesh").GetComponent<SpriteRenderer>().sprite = rocket.sprite;
		
		maxFuel = 90+rocket.fuel*9.65f; //Se tiver 4 pontos em fuel, vai ter 130%

		pushForce = (rocket.propelForce*40)/6; // 6 pontos = 40 push

//		print(maxFuel + pushForce);
		
		GameObject.FindWithTag("Player").GetComponent<Rigidbody2D>().mass = (2.5f+(rocket.weight*0.5f)); //4 pontos - 0.925     5 pontos - 1   6 pontos - 1.075
		GameObject.FindWithTag("Player").GetComponent<RocketBase>().SetPushForce(pushForce);
		GameObject.FindWithTag("Player").GetComponent<RocketBase>().SetMaxFuel(maxFuel);
		GameObject.FindWithTag("Player").GetComponent<RocketBase>().SetCurrentFuel(maxFuel);
		GameObject.FindWithTag("Player").GetComponent<RocketBase>().SetProjectedTo(rocket.projectedToGravity);
	}
	
	public void SetMap(Map map){
		GameObject[] allBgs = GameObject.FindGameObjectsWithTag("Background") as GameObject[];
		for(int i = 0; i < allBgs.Length; i++){
			GameObject.FindGameObjectsWithTag("Background")[i].GetComponent<SpriteRenderer>().sprite = map.sprite;
		}

		GameObject.Find("Floor").GetComponent<SpriteRenderer>().sprite = map.floorSprite;

		Physics2D.gravity = new Vector2(0, -map.gravity);
	}
}
