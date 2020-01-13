using UnityEngine;
using System.Collections;

public class EndGameWritting : MonoBehaviour {

	private bool isIn;
	private float currentTime;
	bool hasFinished;
	bool hasDied;
	static EndGameWritting instance;

	void Awake(){
		instance = this;
	}

	// Use this for initialization
	void Start () {
		isIn = false;
		currentTime = 0f;
		hasDied = false;
		hasFinished = false;

//		GetComponent<Animator>().SetBool("IsIn", false);
	}
	
	// Update is called once per frame
	void Update () {
		currentTime += Time.deltaTime;
		if(currentTime > 1f){
			hasFinished = ScoreController.Finished();//(!GameObject.Find("Controllers").GetComponent<TimeCounter>().IsCounting() && !isIn && GameObject.FindWithTag("Player").GetComponent<RocketBase>().HasFlag());
			hasDied = ScoreController.Finished();//(!GameObject.Find("Controllers").GetComponent<TimeCounter>().IsCounting() && !isIn && GameObject.FindWithTag("Player").GetComponent<RocketBase>().GetCurrentFuel() == 0 && !GameObject.FindWithTag("Player").GetComponent<RocketBase>().HasFlag());
			if((hasFinished || hasDied) && Time.timeScale > 0){
				GetComponent<Animator>().SetTrigger("In");
				GameObject.Find("EndBlackIn").GetComponent<SpriteRenderer>().enabled = true;
				Color c = GameObject.Find("EndBlackIn").GetComponent<SpriteRenderer>().color;
				GameObject.Find("EndBlackIn").GetComponent<SpriteRenderer>().color = new Color(c.r, c.g, c.b, 0.75f);
				StartCoroutine(IsInTimer());
			}
		}

		if(isIn){
			SetPausedButtons(true);
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

	public static bool IsGameEnded(){
		return instance.hasFinished || instance.hasDied;
	}

	
	public void LoadInstaBack(){
		Application.LoadLevel(Application.loadedLevel);
		isIn = false;
	}

	public void OnContinue(){
		if(isIn){
			GameObject.Find("ColorFadeIn").GetComponent<Animator>().SetTrigger("fadeFullIn");
			GetComponent<Animator>().SetTrigger("Out");
			isIn = false;
			StartCoroutine(Continue());
		}
	}

	public void OnGoToMain(){
		if(isIn){
			GetComponent<Animator>().SetTrigger("Out");
			isIn = false;
			StartCoroutine(GoToMenu());
		}
	}

	public static bool GetIsIn(){
		return instance.isIn;
	}

	void TestClick(Vector3 pos){
		RaycastHit2D hit = Physics2D.Raycast(pos, Vector2.zero);
		if (hit.collider != null) {
			switch(hit.transform.name){
			case "UnpauseButtonEnd":
				OnContinue();
				Time.timeScale = 1;
				break;
			case "HomeButtonEnd":
				OnGoToMain();
				Time.timeScale = 1;
				break;
			}
		}
	}

	IEnumerator GoToMenu(){
		GameObject.Find("ColorFadeIn").GetComponent<Animator>().SetTrigger("fadeFullIn");
		yield return new WaitForSeconds(0.25f);
		Application.LoadLevel("MainMenu");
	}

	IEnumerator Continue(){
		GameObject.Find("ColorFadeIn").GetComponent<Animator>().SetTrigger("fadeFullIn");
		yield return new WaitForSeconds(0.25f);
		LoadInstaBack();
	}
	
	IEnumerator IsInTimer(){
		yield return new WaitForSeconds(0.5f);
		isIn = true;
	}

	
	void SetPausedButtons(bool on){
		GameObject.FindWithTag("Player").GetComponent<Rigidbody2D>().velocity = new Vector2(0, 0.001f);
		GameObject.FindWithTag("Player").GetComponent<Collider2D>().enabled = !on;
		GameObject.Find("HomeButtonEnd").GetComponent<SpriteRenderer>().enabled = on;
		GameObject.Find("UnpauseButtonEnd").GetComponent<SpriteRenderer>().enabled = on;
		GameObject.Find("HomeButtonEnd").GetComponent<Collider2D>().enabled = on;
		GameObject.Find("UnpauseButtonEnd").GetComponent<Collider2D>().enabled = on;
//		GameObject.Find("EndButtons").GetComponent<Transform>().position = new Vector2(0, -2);
	}
}
