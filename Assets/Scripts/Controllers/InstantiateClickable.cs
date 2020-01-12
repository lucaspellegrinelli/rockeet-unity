using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class InstantiateClickable : MonoBehaviour {

	public List<ClickableItemBase> clickables;
	private float currentTimeToInstantiate;
	private float randomTime;
	public Vector2 minMaxTime;
	public bool canInstFuel = true;

	// Use this for initialization
	void Start () {
		canInstFuel = true;
		randomTime = Random.Range(minMaxTime.x, minMaxTime.y);
	}
	
	// Update is called once per frame
	void Update () {
		currentTimeToInstantiate += Time.deltaTime;
		if(currentTimeToInstantiate > randomTime && !EndGameWritting.GetIsIn()){
			Create();
			currentTimeToInstantiate = 0;
		}
	}

	void Create(){
		int randomPos = Random.Range(0, clickables.Count);
		if(randomPos == 0){
			if(GameObject.FindWithTag("Player").GetComponent<Transform>().position.y > 55.0f || !canInstFuel){
				randomPos = 1;
			}
			else{
				canInstFuel = false;
			}
		}
		float posX = Random.Range(-1.0f, 1.0f);
		Quaternion qt = new Quaternion(0.0f, 0.0f, 0.0f, 1.0f);
		Instantiate(clickables[randomPos].gameObject, new Vector2(posX, GameObject.FindWithTag("Player").transform.position.y+6.5f), qt);
		randomTime = Random.Range(minMaxTime.x, minMaxTime.y);
	}
}
