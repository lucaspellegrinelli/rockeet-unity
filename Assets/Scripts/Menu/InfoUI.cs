using UnityEngine;
using System.Collections;

public class InfoUI : MonoBehaviour {

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		if(Input.GetKeyDown(KeyCode.Escape))
			StartCoroutine(GoTo("MainMenu"));
		
		foreach(Touch t in Input.touches){
			if(t.phase == TouchPhase.Began){
				Vector3 pos = Camera.main.ScreenToWorldPoint(Input.GetTouch(0).position);
				TestClick(pos);
			}
		}
	}

	
	void TestClick(Vector3 pos){
		RaycastHit2D hit = Physics2D.Raycast(pos, Vector2.zero);
		if (hit.collider != null) {
			switch(hit.transform.name){
			case "HomeButton":
				StartCoroutine(GoTo("MainMenu"));
				break;
			}
		}
	}
	
	IEnumerator GoTo(string scenename){
		GameObject.Find("WhiteFadeIn").GetComponent<Animator>().SetTrigger("fadeIn");
		yield return new WaitForSeconds(0.6f);
		Application.LoadLevel(scenename);
	}

}