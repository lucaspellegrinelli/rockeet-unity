using UnityEngine;
using System.Collections;

public class CreditsUI : MonoBehaviour {

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {

		if(Input.GetKeyDown(KeyCode.Escape))
			StartCoroutine(MainMenu());

		foreach(Touch t in Input.touches){
			if(t.phase == TouchPhase.Began){
				Vector3 pos = Camera.main.ScreenToWorldPoint(Input.GetTouch(0).position);
				TestClick(pos);
			}
		}
		if(Input.GetMouseButtonDown(0)){
			Vector3 pos = Camera.main.ScreenToWorldPoint (Input.mousePosition);
			TestClick(pos);
		}

	}

	void TestClick(Vector3 pos){
		RaycastHit2D hit = Physics2D.Raycast(pos, Vector2.zero);
		if (hit.collider != null) {
			switch(hit.transform.name){
			case "HomeButton":
				StartCoroutine(MainMenu());
				break;
			}
		}
	}

	public IEnumerator MainMenu(){
		GameObject.Find("WhiteFadeIn").GetComponent<Animator>().SetTrigger("fadeIn");
		yield return new WaitForSeconds(0.6f);
		Application.LoadLevel("MainMenu");
	}
}
