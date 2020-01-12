using UnityEngine;
using System.Collections;

public class InfoShow : MonoBehaviour {

	public string key_name;
	public Sprite infoImage;
	private bool isOpen;
	private static InfoShow instance;
	public float fadeRate;

	void Awake(){
		instance = this;
	}

	// Use this for initialization
	void Start () {
		PlayerPrefs.DeleteKey(key_name);
		if(!PlayerPrefs.HasKey(key_name)){
			GameObject tempBack = new GameObject("Info_Show");
			tempBack.AddComponent<SpriteRenderer>();
			tempBack.GetComponent<SpriteRenderer>().sprite = infoImage;
			tempBack.transform.localScale = new Vector3(0.565f, 0.565f, 0.565f);
			tempBack.transform.position = new Vector3(0f, 0f, 0f);
			tempBack.GetComponent<SpriteRenderer>().sortingOrder = 100;
			PlayerPrefs.SetInt(key_name, 0);
			isOpen = true;
		}
		else{
			isOpen = false;
		}
	}

	IEnumerator IFadeOut(){
		GameObject go = GameObject.Find("Info_Show");
		SpriteRenderer sprRend = go.GetComponent<SpriteRenderer>();
		while(sprRend.color.a > 0){
			sprRend.color = new Color(sprRend.color.r, sprRend.color.g, sprRend.color.b, sprRend.color.a - fadeRate);
			yield return null;
		}
		yield return null;
	}

	void Update(){
		if(InputController.IsClicking()){
			isOpen = false;
			StartCoroutine(IFadeOut());
		}
	}
	
	public static bool IsOpen(){
		return instance.isOpen;
	}
}
