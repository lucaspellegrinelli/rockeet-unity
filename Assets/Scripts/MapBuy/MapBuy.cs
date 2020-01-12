using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;

public class MapBuy : MonoBehaviour {

	private int indexItemShowing;
	private int goalIndexItem;
	public float distanceBetween;
	public float timeBetween;
	private float startCameraZ;
	private int coins;
	public Sprite background;
	public Sprite priceShow;
	public Font textFont;
	public Font eraserFont;
	public Sprite showMoney;
	[Space(20)]
	public Sprite select;
	public Sprite deselect;
	public Sprite notBought;
	[Space(20)]
	public Sprite showSold;	

	private List<Map> maps;
	private List<Vector3> positions = new List<Vector3>();

	// Use this for initialization
	void Start () {
		maps = GameObject.Find("MapDatabase").GetComponent<MapsDatabase>().GetAllMaps();
		coins = PlayerPrefs.GetInt("Coins");
		TestPlayerPrefs();

		startCameraZ = Camera.main.transform.position.z;
		indexItemShowing = 0;
		goalIndexItem = indexItemShowing;
		
		CreateMapInstances();
	}
	
	// Update is called once per frame
	void Update () {

		bool[] bought = PlayerPrefsX.GetBoolArray("MapsBought");
		int selected = 0;
		for(int i = 0; i < bought.Length; i++){
			if(maps[i] == GameObject.Find("MapDatabase").GetComponent<MapsDatabase>().GetCurrentMap()){
				selected = i;
				break;
			}
		}

		if(Input.GetKeyDown(KeyCode.A))
			ScrollLeft();
		else if(Input.GetKeyDown(KeyCode.D))
			ScrollRight();

		if(indexItemShowing != goalIndexItem){
			Camera.main.transform.position = Vector3.Lerp(Camera.main.transform.position, positions[goalIndexItem], timeBetween);
			Camera.main.transform.position = new Vector3(Camera.main.transform.position.x, Camera.main.transform.position.y, startCameraZ);
			float currentX = float.Parse(Camera.main.transform.position.x.ToString("F1"));
			float goalX = float.Parse(positions[goalIndexItem].x.ToString("F1"));
			if(currentX == goalX){
				indexItemShowing = goalIndexItem;
			}
		}

		if(bought[goalIndexItem]){
			if(goalIndexItem == selected){ //Esse e o selecionado
				GameObject.FindWithTag("Buy").GetComponent<SpriteRenderer>().sprite = select;
			}
			else{ //Comprado mas nao selecionado
				GameObject.FindWithTag("Buy").GetComponent<SpriteRenderer>().sprite = deselect;
			}
		}
		else{ //Nao comprado
			GameObject.FindWithTag("Buy").GetComponent<SpriteRenderer>().sprite = notBought;
		}
	}

	public void ScrollLeft(){
		float currentX = float.Parse(Camera.main.transform.position.x.ToString("F1"));
		float goalX = float.Parse(positions[goalIndexItem].x.ToString("F1"));
		if(currentX == goalX){
			int goal = goalIndexItem;
			if(--goal >= 0)
				goalIndexItem--;
		}
	}
	
	public void ScrollRight(){
		float currentX = float.Parse(Camera.main.transform.position.x.ToString("F1"));
		float goalX = float.Parse(positions[goalIndexItem].x.ToString("F1"));
		if(currentX == goalX){
			int goal = goalIndexItem;
			if(++goal < maps.Count)
				goalIndexItem++;
		}
	}
	
	public void Buy(){
		int map = goalIndexItem; //So I changed here to goal instead of current
		bool[] bought = PlayerPrefsX.GetBoolArray("MapsBought");
		if(!bought[map] && coins >= maps[map].price){ //Buying a rocket
			coins -= maps[map].price;
			PlayerPrefs.SetInt("Coins", coins);
			SetBoolInArray(true, map);

			GameObject.Find("ShowPrice"+map).GetComponent<TextMesh>().text = "";
			GameObject.Find("Price"+map+"").GetComponent<SpriteRenderer>().sprite = showSold;
			GameObject.Find("Money "+map+"").GetComponent<TextMesh>().text = coins.ToString();
		}
		if(bought[map]){ ///Selecting a rocket
			GameObject.Find("MapDatabase").GetComponent<MapsDatabase>().SetCurrentMap(map);
		}
	}
	
	void SetBoolInArray(bool state, int index){
		bool[] mapsBought = new bool[PlayerPrefsX.GetBoolArray("MapsBought").Length];
		for(int i = 0; i < mapsBought.Length; i++){
			if(i != index)
				mapsBought[i] = PlayerPrefsX.GetBoolArray("MapsBought")[i];
			else
				mapsBought[i] = state;
		}
		
		PlayerPrefsX.SetBoolArray("MapsBought", mapsBought);
	}
	
	void TestPlayerPrefs(){
		if(PlayerPrefsX.GetBoolArray("MapsBought").Length < maps.Count){
			bool[] mapsBought = PlayerPrefsX.GetBoolArray("MapsBought");
			int lenght = PlayerPrefsX.GetBoolArray("MapsBought").Length;
			PlayerPrefsX.SetBoolArray("MapsBought", new bool[maps.Count]);
			
			bool[] result = new bool[maps.Count];
			for(int i = 0; i < lenght; i++){
				result[i] = mapsBought[i];
			}
			for(int i = lenght; i < maps.Count; i++){
				result[i] = false;
			}
			result[0] = true;
			PlayerPrefsX.SetBoolArray("MapsBought", result);
		}
	}
	
	void CreateMapInstances(){
		GameObject holder = new GameObject("AllMaps");
		GameObject bkac = new GameObject("Background");
		GameObject prices = new GameObject("PricesSprite");
		GameObject showPrices = new GameObject("PricesText");
		GameObject showYourMoney = new GameObject("ShowUserMoney");
		GameObject showMoneyImg = new GameObject("ShowUserMoney");
		GameObject showGravity = new GameObject("ShowGravityValue");

		bool[] bought = PlayerPrefsX.GetBoolArray("MapsBought");

		for(int i = 0; i < maps.Count; i++){
			Vector3 tempPos = new Vector3(distanceBetween*i, 0f, 0f);
			positions.Add(tempPos);
			
			//Create Gameobject with sprite
			GameObject tempGo = new GameObject("Map "+i+"");
			tempGo.transform.parent = holder.transform;
			tempGo.AddComponent<SpriteRenderer>();
			tempGo.GetComponent<SpriteRenderer>().sprite = maps[i].sprite;
			tempGo.transform.localScale = new Vector3(0.27f, 0.27f, 0.27f);
			tempGo.transform.position = tempPos;
			tempGo.GetComponent<SpriteRenderer>().sortingOrder = -1;
			
//			//Create background
			GameObject tempBack = new GameObject("Back "+i+"");
			tempBack.transform.parent = bkac.transform;
			tempBack.AddComponent<SpriteRenderer>();
			tempBack.GetComponent<SpriteRenderer>().sprite = background;
			tempBack.transform.localScale = new Vector3(0.56f, 0.56f, 0.56f);
			tempBack.transform.position = tempPos;
			tempBack.GetComponent<SpriteRenderer>().sortingOrder = -100+i;

			//Price
			Vector3 priceTempPos = new Vector3(1.623f + distanceBetween * i, 1.65f, 0f);
			GameObject tempPrice = new GameObject("Price"+i+"");
			tempPrice.transform.parent = prices.transform;
			tempPrice.AddComponent<SpriteRenderer>();
			tempPrice.GetComponent<SpriteRenderer>().sprite = priceShow;
			tempPrice.transform.localScale = new Vector3(0.45f, 0.45f, 0.45f);
			tempPrice.transform.position = priceTempPos;
			tempPrice.transform.Rotate(new Vector3(0f, 0f, -50f));
			tempPrice.GetComponent<SpriteRenderer>().sortingOrder = 0;

			GameObject tempShowPrice = new GameObject("ShowPrice"+i+"");
			tempShowPrice.transform.parent = showPrices.transform;
			tempShowPrice.AddComponent<MeshRenderer>();
			tempShowPrice.AddComponent<TextMesh>();

			if(!bought[i]){
				tempShowPrice.GetComponent<TextMesh>().text = "$"+maps[i].price.ToString();
				tempShowPrice.transform.position = new Vector3(1.54f + distanceBetween * i, 1.493f, -0.02f);
			}
			else{
				tempShowPrice.GetComponent<TextMesh>().text = "";
				tempPrice.GetComponent<SpriteRenderer>().sprite = showSold;
				tempShowPrice.transform.position = new Vector3(1.505f + distanceBetween * i, 1.444f, -0.02f);
			}

			tempShowPrice.GetComponent<TextMesh>().fontSize = 25;
			tempShowPrice.GetComponent<TextMesh>().font = textFont;
			tempShowPrice.GetComponent<TextMesh>().renderer.material = textFont.material;
			tempShowPrice.GetComponent<TextMesh>().alignment = TextAlignment.Center;
			tempShowPrice.GetComponent<TextMesh>().anchor = TextAnchor.MiddleLeft;
			tempShowPrice.GetComponent<TextMesh>().color = Color.black;
			tempShowPrice.transform.rotation.eulerAngles.Set(0f, 0f, 79.15f);
			tempShowPrice.transform.Rotate(new Vector3(0f, 0f, 27.5f));
			tempShowPrice.transform.localScale = new Vector3(0.115f, 0.115f, 0.115f);
			
			//Show user money
			GameObject userMoney = new GameObject("Money "+i+"");
			userMoney.transform.parent = showYourMoney.transform;
			userMoney.transform.position = new Vector3(0f + distanceBetween * i, 0f, -0.02f);
			userMoney.transform.localScale = new Vector3(0.115f, 0.115f, 0.115f);
			userMoney.transform.position = new Vector3(-0.871f + distanceBetween * i, 2.24f, -0.02f);
			
			userMoney.AddComponent<TextMesh>();
			userMoney.GetComponent<TextMesh>().fontSize = 20;
			userMoney.GetComponent<TextMesh>().font = eraserFont;
			userMoney.GetComponent<TextMesh>().renderer.material = eraserFont.material;
			userMoney.GetComponent<TextMesh>().alignment = TextAlignment.Center;
			userMoney.GetComponent<TextMesh>().anchor = TextAnchor.MiddleLeft;
			userMoney.GetComponent<TextMesh>().color = Color.white;
			userMoney.GetComponent<TextMesh>().text = ""+PlayerPrefs.GetInt("Coins");

			//Show user money image
			GameObject userMoneyImg = new GameObject("MoneyIMG "+i+"");
			userMoneyImg.transform.parent = showMoneyImg.transform;
			userMoneyImg.transform.position = new Vector3(-1.135f + distanceBetween * i, 2.281f, -0.02f);
			userMoneyImg.transform.localScale = new Vector3(0.534819f, 0.534819f, 0.534819f);

			userMoneyImg.AddComponent<SpriteRenderer>();
			userMoneyImg.GetComponent<SpriteRenderer>().sprite = showMoney;

			//Show gravity
			GameObject showGravityObj = new GameObject("Gravity "+i+"");
			showGravityObj.transform.parent = showGravity.transform;
			showGravityObj.transform.localScale = new Vector3(0.1f, 0.1f, 0.1f);
			showGravityObj.transform.position = new Vector3(-1.329f + distanceBetween * i, 1.83f, -0.02f);
			
			showGravityObj.AddComponent<TextMesh>();
			showGravityObj.GetComponent<TextMesh>().fontSize = 20;
			showGravityObj.GetComponent<TextMesh>().font = eraserFont;
			showGravityObj.GetComponent<TextMesh>().renderer.material = eraserFont.material;
			showGravityObj.GetComponent<TextMesh>().alignment = TextAlignment.Center;
			showGravityObj.GetComponent<TextMesh>().anchor = TextAnchor.MiddleLeft;
			showGravityObj.GetComponent<TextMesh>().color = Color.white;
			showGravityObj.GetComponent<TextMesh>().text = "G = "+maps[i].gravity+ " m/s²";
		}
	}
}
