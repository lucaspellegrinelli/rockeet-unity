using UnityEngine;
using System.Collections;
using System.Collections.Generic;

[System.Serializable]
public class Rocket{
	public Sprite sprite;
	public int price;
	public int fuel;
	public int weight;
	public int propelForce;
	public float projectedToGravity;
}

public class RocketsDatabase : MonoBehaviour {

	public List<Rocket> rockets;
	private int currentRocket;

	private static RocketsDatabase instance = null;
	public static RocketsDatabase Instance {
		get{ 
			return instance; 
		}
	}

	void Awake() {
		if (instance != null && instance != this) {
			Destroy(this.gameObject);
			return;
		} 
		else {
			instance = this;
		}
		DontDestroyOnLoad(this.gameObject);
	}

	void Start(){
		currentRocket = PlayerPrefs.GetInt("CurrentRocket");
	}

	public void SetCurrentRocket(int index){
		currentRocket = index;
		PlayerPrefs.SetInt("CurrentRocket", index);
	}
	
	public Rocket GetCurrentRocket(){
		return rockets[currentRocket];
	}	

	public List<Rocket> GetAllRockets(){
		return rockets;
	}
}
