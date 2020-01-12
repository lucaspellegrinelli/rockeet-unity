using UnityEngine;
using System.Collections;

public class SwipeScript : MonoBehaviour {

	private float fingerStartTime  = 0.0f;
	private Vector2 fingerStartPos = Vector2.zero;
	
	private bool isSwipe = false;
	private float minSwipeDist  = 50.0f;
	private float maxSwipeTime = 0.5f;
	
	
	// Update is called once per frame
	void Update () {
		if (Input.touchCount > 0 && !InfoShow.IsOpen()){
			foreach (Touch touch in Input.touches){
				switch (touch.phase){
				case TouchPhase.Began :
					isSwipe = true;
					fingerStartTime = Time.time;
					fingerStartPos = touch.position;
					break;
					
				case TouchPhase.Canceled :
					isSwipe = false;
					break;
					
				case TouchPhase.Ended :		
					float gestureTime = Time.time - fingerStartTime;
					float gestureDist = (touch.position - fingerStartPos).magnitude;
					
					if (isSwipe && gestureTime < maxSwipeTime && gestureDist > minSwipeDist){
						Vector2 direction = touch.position - fingerStartPos;
						Vector2 swipeType = Vector2.zero;
						
						if (Mathf.Abs(direction.x) > Mathf.Abs(direction.y)){
							swipeType = Vector2.right * Mathf.Sign(direction.x);
						}
						else{
							swipeType = Vector2.up * Mathf.Sign(direction.y);
						}
						RocketBuy rocketBuy = GetComponent<RocketBuy>();
						MapBuy mapBuy = GetComponent<MapBuy>();
						if(swipeType.x != 0.0f){
							if(swipeType.x < 0.0f){
								if(Application.loadedLevelName == "BuyRocket")
									rocketBuy.ScrollRight();
								else if(Application.loadedLevelName == "BuyMap")
									mapBuy.ScrollRight();
							}
							else{
								if(Application.loadedLevelName == "BuyRocket")
									rocketBuy.ScrollLeft();
								else if(Application.loadedLevelName == "BuyMap")
									mapBuy.ScrollLeft();
							}
						}
					}
					break;
				}
			}
		}
	}
}