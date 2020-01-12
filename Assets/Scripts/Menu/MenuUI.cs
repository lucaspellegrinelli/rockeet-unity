using UnityEngine;
using System.Collections;

public class MenuUI : MonoBehaviour {

	// Use this for initialization
	void Start () {
		PlayerPrefsX.SetBool("ShowArrows", true);
	}
	
	// Update is called once per frame
	void Update () {
		if(Input.touchCount >= 3 || Input.GetKeyDown(KeyCode.M)){
			PlayerPrefs.SetInt("Coins", PlayerPrefs.GetInt("Coins")+10);
			if(PlayerPrefs.GetInt("Coins") > 99)
				PlayerPrefs.SetInt("Coins", 99);
		}

		if(Input.GetMouseButtonDown(1)){
			PlayerPrefs.DeleteAll();
			PlayerPrefsX.SetBoolArray("MapsBought", new bool[]{true, false});
			PlayerPrefsX.SetBoolArray("RocketsBought", new bool[]{true, false});
		}
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
			case "PlayButton":
				StartCoroutine(GoTo("ChooseGameMode"));
				break;
			case "BuyButton":
				StartCoroutine(GoTo("BuyRocket"));
				break;
			case "MapsButton":
				StartCoroutine(GoTo("BuyMap"));
				break;
			case "SettingsButton":
				StartCoroutine(GoTo("Settings"));
				break;
			case "CreditsButton":
				StartCoroutine(GoTo("Credits"));
				break;
			case "InfoButton":
				StartCoroutine(GoTo("Info"));
				break;
			}
		}
	}
}
