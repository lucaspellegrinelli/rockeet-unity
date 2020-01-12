using UnityEngine;
using System.Collections;
public enum TypeOfClickable{
	FUEL,
	GOLD,
}

public class ClickableItemBase : MonoBehaviour {

	public float moveSpeed;
	public TypeOfClickable typeOfItem;
	public float howMuchThisContains;
	private float currentTimeAlive;

	// Use this for initialization
	void Start () {
		transform.parent = Camera.main.transform;
		transform.eulerAngles = new Vector3(transform.eulerAngles.x, transform.eulerAngles.y, Random.Range(-170, -190));
	}
	
	// Update is called once per frame
	void Update () {
		currentTimeAlive += Time.deltaTime;
		if(currentTimeAlive >= 10f)
			Destroy(gameObject);
		transform.Translate(Vector2.up * moveSpeed * Time.deltaTime);

		if(EndGameWritting.IsGameEnded()){
			Destroy(gameObject);
		}
	}

	public TypeOfClickable ClickableType(){
		return typeOfItem;
	}
}
