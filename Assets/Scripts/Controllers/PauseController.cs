using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class PauseController : MonoBehaviour {

	bool isPaused;
	float currentTimePlaying;
	static PauseController instance;
	float playerVelocity;

	void Awake(){
		instance = this;
	}

	// Use this for initialization
	void Start () {
//		GameObject.Find("PauseBlackIn").GetComponent<SpriteRenderer>().enabled = false;
		SetPausedButtons(false);
	}
	
	// Update is called once per frame
	void Update () {
		currentTimePlaying += Time.deltaTime;
		if(currentTimePlaying >= 0.5f && !EndGameWritting.GetIsIn()){
			foreach(Touch t in Input.touches){
				if(t.phase == TouchPhase.Began){
					TestClick(Camera.main.ScreenToWorldPoint(t.position));
				}
			}
			if(Input.GetMouseButtonDown(0)){
				Vector3 pos = Camera.main.ScreenToWorldPoint (Input.mousePosition);
				TestClick(pos);
			}
		}
	}

	void TestClick(Vector3 pos){
		RaycastHit2D hit = Physics2D.Raycast(pos, Vector2.zero);
		if (hit.collider != null) {
			switch(hit.transform.tag){
			case "Pause":
				if(!isPaused){
					PauseGame();
				}
				break;
			case "GoToMain":
				if(isPaused){
					GoToMainMenu();
				}
				break;
			case "Unpause":
				if(isPaused){
					UnPauseGame();
				}
				break;
			case "Restart":
				if(isPaused){
					RestartGame();
				}
				break;
			}
		}
	}

	void RestartGame(){
		Time.timeScale = 1;
		SetPausedButtons(false);
		isPaused = false;
		StartCoroutine(Restart());
	}

	void PauseGame(){
		playerVelocity = GameObject.FindWithTag("Player").GetComponent<Rigidbody2D>().velocity.y;
		SetPausedButtons(true);
		StartCoroutine(Pause());
		isPaused = true;
	}

	void UnPauseGame(){
		SetPausedButtons(false);
		Time.timeScale = 1;
		isPaused = false;
		GameObject.Find("ColorFadeIn").GetComponent<Animator>().SetTrigger("fadeParcOut");
		GameObject.FindWithTag("Player").GetComponent<Rigidbody2D>().velocity = new Vector2(0, playerVelocity);
	}

	void GoToMainMenu(){
		SetPausedButtons(false);
		isPaused = true;
		StartCoroutine(GoToMenu());
	}

	IEnumerator Pause(){
		GameObject.Find("ColorFadeIn").GetComponent<Animator>().SetTrigger("fadeIn");
		yield return new WaitForSeconds(0.225f);
		Time.timeScale = 0;
	}

	IEnumerator GoToMenu(){
		Time.timeScale = 1;
		GameObject.Find("ColorFadeIn").GetComponent<Animator>().SetTrigger("fadeFullIn");
		yield return new WaitForSeconds(0.25f);
		Application.LoadLevel("MainMenu");
//		GameObject.Find("EndBlackIn").GetComponent<SpriteRenderer>().enabled = true;
	}

	IEnumerator Restart(){
		GameObject.Find("ColorFadeIn").GetComponent<Animator>().SetTrigger("fadeFullIn");
		yield return new WaitForSeconds(0.25f);
		Application.LoadLevel(Application.loadedLevel);
	}

	void SetPausedButtons(bool on){
		GameObject.FindWithTag("Player").GetComponent<Collider2D>().enabled = !on;
		GameObject.FindWithTag("GoToMain").GetComponent<SpriteRenderer>().enabled = on;
		GameObject.FindWithTag("Unpause").GetComponent<SpriteRenderer>().enabled = on;
		GameObject.FindWithTag("Restart").GetComponent<SpriteRenderer>().enabled = on;
		GameObject.FindWithTag("GoToMain").collider2D.enabled = on;
		GameObject.FindWithTag("Unpause").collider2D.enabled = on;
		GameObject.FindWithTag("Restart").collider2D.enabled = on;
	}

	public static bool IsPaused(){
		return instance.isPaused;
	}
}
