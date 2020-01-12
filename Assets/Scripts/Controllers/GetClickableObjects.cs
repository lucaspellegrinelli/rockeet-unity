using UnityEngine;
using System.Collections;

public class GetClickableObjects : MonoBehaviour {

	// Use this for initialization
	void Start () {
	}
	
	// Update is called once per frame
	void Update () {
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

	void TestClick(Vector3 pos){
		RaycastHit2D hit = Physics2D.Raycast(pos, Vector2.zero);
		if (hit.collider != null) {
			if((hit.transform.gameObject.GetComponent("ClickableItemBase") as ClickableItemBase) != null){
				Destroy(hit.transform.gameObject);
				switch(hit.transform.gameObject.GetComponent<ClickableItemBase>().ClickableType()){
				case TypeOfClickable.FUEL:
					RocketBase player = GameObject.FindObjectOfType(typeof(RocketBase)) as RocketBase;
					player.AddFuel(hit.transform.gameObject.GetComponent<ClickableItemBase>().howMuchThisContains);
					break;
					
				case TypeOfClickable.GOLD:
					int coins = PlayerPrefs.GetInt("Coins");
					PlayerPrefs.SetInt("Coins", ++coins);
					if(PlayerPrefs.GetInt("Coins") > 99)
						PlayerPrefs.SetInt("Coins", 99);
					break;
				}
			}
		}
	}
}
