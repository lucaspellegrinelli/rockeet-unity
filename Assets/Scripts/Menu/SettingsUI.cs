using UnityEngine;
using System.Collections;

public class SettingsUI : MonoBehaviour {

	private bool showArrows;
	public GameObject showVectors;
	public Sprite[] showVectorsButtons;
	//	[Space(20)]

	// Use this for initialization
	void Start () {
		showArrows = PlayerPrefsX.GetBool("ShowArrows");
		showVectors.GetComponent<SpriteRenderer>().sprite = showVectorsButtons[showArrows ? 1 : 0];
	}

	// Update is called once per frame
	void Update () {

		if(Input.GetKeyDown(KeyCode.Escape))
			StartCoroutine(MainMenu());

		foreach(Touch t in Input.touches){
			if(t.phase == TouchPhase.Began){
				Vector3 pos = Camera.main.ScreenToWorldPoint(t.position);
				TestClick(pos);
			}
		}
	}

	void TestClick(Vector3 pos){
		RaycastHit2D hit = Physics2D.Raycast(pos, Vector2.zero);
		if (hit.collider != null) {
			switch(hit.transform.name){
			case "TurnOnOffVectors":
				showArrows = !showArrows;
				showVectors.GetComponent<SpriteRenderer>().sprite = showVectorsButtons[showArrows ? 1 : 0];
				PlayerPrefsX.SetBool("ShowArrows", showArrows);
				break;
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
