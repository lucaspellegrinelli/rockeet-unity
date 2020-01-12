using UnityEngine;
using System.Collections;
using System.Collections.Generic;

[System.Serializable]
public class Map{
	public Sprite sprite;
	public Sprite floorSprite;
	public int price;
	public float gravity;
}

public class MapsDatabase : MonoBehaviour {

	public List<Map> maps;
	private int currentMap;

	private static MapsDatabase instance = null;
	public static MapsDatabase Instance {
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

	// Use this for initialization
	void Start () {
		currentMap = PlayerPrefs.GetInt("CurrentMap");
	}
	
	public void SetCurrentMap(int index){
		currentMap = index;
		PlayerPrefs.SetInt("CurrentMap", index);
	}
	
	public Map GetCurrentMap(){
		return maps[currentMap];
	}	
	
	public List<Map> GetAllMaps(){
		return maps;
	}
}
