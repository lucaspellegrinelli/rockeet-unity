using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;

public class RocketBuy : MonoBehaviour {
	
	private int indexItemShowing;
	private int goalIndexItem;
	public float distanceBetween;
	public Vector2 distanceBetweenStats;
	public Vector2 startPositionStats;
	public float timeBetween;
	private float startCameraZ;
	private int coins;
	public Sprite background;
	public Sprite priceShow;
	public Font textFont;
	public Font eraserFont;
	[Space(20)]
	public Sprite select;
	public Sprite deselect;
	public Sprite notBought;
	public Sprite statsImage;
	[Space(20)]
	public Sprite showSold;

	private List<Rocket> rockets;
	private List<Vector3> positions = new List<Vector3>();

	// Use this for initialization
	void Start () {
		rockets = GameObject.Find("RocketDatabase").GetComponent<RocketsDatabase>().GetAllRockets();
		coins = PlayerPrefs.GetInt("Coins");
		TestPlayerPrefs();

		startCameraZ = Camera.main.transform.position.z;
		indexItemShowing = 0;
		goalIndexItem = indexItemShowing;

		CreateRocketInstances();
	}
	
	// Update is called once per frame
	void Update () {

		bool[] bought = PlayerPrefsX.GetBoolArray("RocketsBought");
		int selected = 0;
		for(int i = 0; i < bought.Length; i++){
			if(rockets[i] == GameObject.Find("RocketDatabase").GetComponent<RocketsDatabase>().GetCurrentRocket()){
				selected = i;
				break;
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
			if(++goal < rockets.Count)
				goalIndexItem++;
		}
	}

	public void Buy(){
		int rocket = goalIndexItem; //So I changed here to goal instead of current
		bool[] bought = PlayerPrefsX.GetBoolArray("RocketsBought");
		if(!bought[rocket] && coins >= rockets[rocket].price){ //Buying a rocket
			coins -= rockets[rocket].price;
			PlayerPrefs.SetInt("Coins", coins);
			SetBoolInArray(true, rocket);

			GameObject.Find("ShowPrice"+rocket).GetComponent<TextMesh>().text = "";
			GameObject.Find("Price"+rocket+"").GetComponent<SpriteRenderer>().sprite = showSold;
			GameObject.Find("Money "+rocket+"").GetComponent<TextMesh>().text = coins.ToString();

		}
		if(bought[rocket]){ ///Selecting a rocket
			GameObject.Find("RocketDatabase").GetComponent<RocketsDatabase>().SetCurrentRocket(rocket);
		}
	}

	void SetBoolInArray(bool state, int index){
		bool[] rocketsBought = new bool[PlayerPrefsX.GetBoolArray("RocketsBought").Length];
		for(int i = 0; i < rocketsBought.Length; i++){
			if(i != index)
				rocketsBought[i] = PlayerPrefsX.GetBoolArray("RocketsBought")[i];
			else
				rocketsBought[i] = state;
		}

		PlayerPrefsX.SetBoolArray("RocketsBought", rocketsBought);
	}

	void TestPlayerPrefs(){
		if(PlayerPrefsX.GetBoolArray("RocketsBought").Length < rockets.Count){
			bool[] rocketsBought = PlayerPrefsX.GetBoolArray("RocketsBought");
			int lenght = PlayerPrefsX.GetBoolArray("RocketsBought").Length;
			PlayerPrefsX.SetBoolArray("RocketsBought", new bool[rockets.Count]);
			
			bool[] result = new bool[rockets.Count];
			for(int i = 0; i < lenght; i++){
				result[i] = rocketsBought[i];
			}
			for(int i = lenght; i < rockets.Count; i++){
				result[i] = false;
			}
			result[0] = true;
			
			PlayerPrefsX.SetBoolArray("RocketsBought", result);
		}
	}

	void CreateRocketInstances(){
		GameObject holder = new GameObject("AllRockets");
		GameObject bkac = new GameObject("Background");
		GameObject prices = new GameObject("PricesSprite");
		GameObject showPrices = new GameObject("PricesText");
		GameObject showYourMoney = new GameObject("ShowUserMoney");
		GameObject projectedTo = new GameObject("RocketProjectedTo");

		bool[] bought = PlayerPrefsX.GetBoolArray("RocketsBought");

		for(int i = 0; i < rockets.Count; i++){
			Vector3 tempPos = new Vector3(distanceBetween*i, 0f, 0f);
			positions.Add(tempPos);
			
			//Create Gameobject with sprite
			GameObject tempGo = new GameObject("Rocket "+i+"");
			tempGo.transform.parent = holder.transform;
			tempGo.AddComponent<SpriteRenderer>();
			tempGo.GetComponent<SpriteRenderer>().sprite = rockets[i].sprite;
			tempGo.transform.localScale = new Vector3(0.45f, 0.45f, 0.45f);
			tempGo.transform.position = tempPos;
			tempGo.GetComponent<SpriteRenderer>().sortingOrder = -1;

			//Create background
			GameObject tempBack = new GameObject("Back "+i+"");
			tempBack.transform.parent = bkac.transform;
			tempBack.AddComponent<SpriteRenderer>();
			tempBack.GetComponent<SpriteRenderer>().sprite = background;
			tempBack.transform.localScale = new Vector3(0.56f, 0.56f, 0.56f);
			tempBack.transform.position = tempPos;
			tempBack.GetComponent<SpriteRenderer>().sortingOrder = -100+i;

			//Prices
			Vector3 priceTempPos = new Vector3(2.103f + distanceBetween*i, 1.841f, 0f);
			GameObject tempPrice = new GameObject("Price"+i+"");
			tempPrice.transform.parent = prices.transform;
			tempPrice.AddComponent<SpriteRenderer>();
			tempPrice.GetComponent<SpriteRenderer>().sprite = priceShow;
			tempPrice.transform.localScale = new Vector3(0.45f, 0.45f, 0.45f);
			tempPrice.transform.position = priceTempPos;
			tempPrice.transform.Rotate(new Vector3(0f, 0f, -63.5f));
			tempPrice.GetComponent<SpriteRenderer>().sortingOrder = 0;

			GameObject tempShowPrice = new GameObject("ShowPrice"+i+"");
			tempShowPrice.transform.parent = showPrices.transform;
			tempShowPrice.AddComponent<MeshRenderer>();
			tempShowPrice.AddComponent<TextMesh>();

			if(!bought[i]){
				tempShowPrice.GetComponent<TextMesh>().text = "$"+rockets[i].price.ToString();
				tempShowPrice.transform.position = new Vector3(1.985f + distanceBetween * i, 1.683f, -0.02f);
			}
			else{
				tempShowPrice.GetComponent<TextMesh>().text = "";
				tempPrice.GetComponent<SpriteRenderer>().sprite = showSold;
				tempShowPrice.transform.position = new Vector3(1.948f + distanceBetween * i, 1.649f, -0.02f);
			}

			tempShowPrice.GetComponent<TextMesh>().fontSize = 25;
			tempShowPrice.GetComponent<TextMesh>().font = textFont;
			tempShowPrice.GetComponent<TextMesh>().renderer.material = textFont.material;
			tempShowPrice.GetComponent<TextMesh>().alignment = TextAlignment.Center;
			tempShowPrice.GetComponent<TextMesh>().anchor = TextAnchor.MiddleLeft;
			tempShowPrice.GetComponent<TextMesh>().color = Color.black;
			tempShowPrice.transform.Rotate(new Vector3(0f, 0f, 14f));
			tempShowPrice.transform.localScale = new Vector3(0.115f, 0.115f, 0.115f);

			//Show user money
			GameObject userMoney = new GameObject("Money "+i+"");
			userMoney.transform.parent = showYourMoney.transform;
			userMoney.transform.localScale = new Vector3(0.115f, 0.115f, 0.115f);
			userMoney.transform.position = new Vector3(-1.53f + distanceBetween * i, 2.4f, -0.02f);

			userMoney.AddComponent<TextMesh>();
			userMoney.GetComponent<TextMesh>().fontSize = 20;
			userMoney.GetComponent<TextMesh>().font = eraserFont;
			userMoney.GetComponent<TextMesh>().renderer.material = eraserFont.material;
			userMoney.GetComponent<TextMesh>().alignment = TextAlignment.Center;
			userMoney.GetComponent<TextMesh>().anchor = TextAnchor.MiddleLeft;
			userMoney.GetComponent<TextMesh>().color = Color.white;
			userMoney.GetComponent<TextMesh>().text = ""+PlayerPrefs.GetInt("Coins");

			//Show rocket project for
			GameObject tempProjectTo = new GameObject("Money "+i+"");
			tempProjectTo.transform.parent = projectedTo.transform;
			tempProjectTo.transform.localScale = new Vector3(0.115f, 0.115f, 0.115f);
			tempProjectTo.transform.position = new Vector3(-2.028f + distanceBetween * i, 1.93f, -0.02f);
			
			tempProjectTo.AddComponent<TextMesh>();
			tempProjectTo.GetComponent<TextMesh>().fontSize = 20;
			tempProjectTo.GetComponent<TextMesh>().font = eraserFont;
			tempProjectTo.GetComponent<TextMesh>().renderer.material = eraserFont.material;
			tempProjectTo.GetComponent<TextMesh>().alignment = TextAlignment.Center;
			tempProjectTo.GetComponent<TextMesh>().anchor = TextAnchor.MiddleLeft;
			tempProjectTo.GetComponent<TextMesh>().color = Color.white;
			tempProjectTo.GetComponent<TextMesh>().text = ""+rockets[i].projectedToGravity;
		}
		CreateStats();
	}

	void CreateStats(){
		for(int z = 0; z < rockets.Count; z++){

			int[] completed = {rockets[z].propelForce, rockets[z].weight, rockets[z].fuel};
			startPositionStats.x = startPositionStats.x + (distanceBetween * z);
			if(z > 0)
				startPositionStats.x -= distanceBetween * (z-1);
			GameObject stats = new GameObject("Stats"+z);
			stats.transform.position = new Vector2(distanceBetween * z, 0);
			GameObject fuel = new GameObject("FuelStats");
			fuel.transform.parent = stats.transform;
			GameObject weight = new GameObject("WeightStats");
			weight.transform.parent = stats.transform;
			GameObject force = new GameObject("ForceStats");
			force.transform.parent = stats.transform;
			Transform[] fathers = {fuel.transform, weight.transform, force.transform};

			for(int j = 0; j < fathers.Length; j++){
				for(int i = 0; i < 10; i++){
					GameObject temp = new GameObject("Box" + i);
					temp.AddComponent<SpriteRenderer>();
					temp.transform.position = new Vector2(startPositionStats.x + distanceBetweenStats.x * i, startPositionStats.y+distanceBetweenStats.y * j);
					temp.GetComponent<SpriteRenderer>().sprite = statsImage;
					temp.transform.parent = fathers[j].transform;
					if(i < completed[j]){
						temp.GetComponent<SpriteRenderer>().color = Color.green;
					}
				}
			}
		}
	}
}
