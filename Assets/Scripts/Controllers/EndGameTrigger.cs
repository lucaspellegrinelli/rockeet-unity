using UnityEngine;
using System.Collections;

public enum GameModes{
	CatchAndLand,
	AgainstTime
}

public class EndGameTrigger : MonoBehaviour {

	private GameObject player;
	private bool gameIsGoing;
	public GameModes gameMode;

	private static EndGameTrigger instance;

	void Awake(){
		instance = this;
	}

	// Use this for initialization
	void Start () {
		player = GameObject.FindWithTag("Player");
		gameIsGoing = false;
	}
	
	// Update is called once per frame
	void Update () {
		switch(gameMode){
		case GameModes.CatchAndLand:
			CatchAndLandGameMode();
			break;

		case GameModes.AgainstTime:
			AgainstTimeGameMode();
			break;
		}
	}

	public static bool GetGameIsGoing(){
		return instance.gameIsGoing;
	}

	void CatchAndLandGameMode(){
		gameIsGoing = GameObject.Find("Controllers").GetComponent<TimeCounter>().IsCounting();

		bool isPaused = PauseController.IsPaused();
		Vector2 playerVelocity = player.GetComponent<Rigidbody2D>().velocity;
		bool hasFlag = GameObject.FindWithTag("Player").GetComponent<RocketBase>().HasFlag();
		float currentFuel = GameObject.FindWithTag("Player").GetComponent<RocketBase>().GetCurrentFuel();

		bool hasFinished = (playerVelocity == Vector2.zero && hasFlag);
		bool hasDied = (playerVelocity == Vector2.zero && currentFuel == 0.0f && !hasFlag);

		if((hasFinished || hasDied) && !isPaused){
			EndGame();
		}
	}

	void AgainstTimeGameMode(){
		gameIsGoing = GameObject.Find("Controllers").GetComponent<TimeCounter>().IsCounting();
		
		bool isPaused = PauseController.IsPaused();
		Vector2 playerVelocity = player.GetComponent<Rigidbody2D>().velocity;
		bool hasFlag = GameObject.FindWithTag("Player").GetComponent<RocketBase>().HasFlag();
		float currentFuel = GameObject.FindWithTag("Player").GetComponent<RocketBase>().GetCurrentFuel();
		
		bool hasFinished = (playerVelocity == Vector2.zero && hasFlag);
		bool hasDied = (playerVelocity == Vector2.zero && currentFuel == 0.0f && !hasFlag);

		if((hasFinished || hasDied) && !isPaused){
			EndGame();
		}
	}

	void EndGame(){
		gameIsGoing = false;
		GameObject.Find("Controllers").GetComponent<TimeCounter>().StopCount();
//		Camera.main.transform.position = new Vector3(Camera.main.transform.position.x, 0.23f, Camera.main.transform.position.z);
	}
}
