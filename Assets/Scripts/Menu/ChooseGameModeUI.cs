using UnityEngine;
using System.Collections;

public class ChooseGameModeUI : MonoBehaviour {

	// Use this for initialization
	void Start () {
	
	}
	
	void Update () {

		if(Input.GetKeyDown(KeyCode.Escape))
			StartCoroutine(GoTo("MainMenu"));

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

	IEnumerator GoTo(string scenename){
		GameObject.Find("WhiteFadeIn").GetComponent<Animator>().SetTrigger("fadeIn");
		yield return new WaitForSeconds(0.6f);
		Application.LoadLevel(scenename);
	}
	
	void TestClick(Vector3 pos){
		RaycastHit2D hit = Physics2D.Raycast(pos, Vector2.zero);
		if (hit.collider != null) {
			switch(hit.transform.name){
			case "CatchAndLandButton":
				StartCoroutine(GoTo("CatchAndLand"));
				break;
			case "AgainstTimeButton":
				StartCoroutine(GoTo("AgainstTime"));
				break;
			case "HomeButton":
				StartCoroutine(GoTo("MainMenu"));
				break;
			}
		}
	}
}
