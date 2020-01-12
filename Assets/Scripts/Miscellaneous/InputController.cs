using UnityEngine;
using System.Collections;

public class InputController : MonoBehaviour {

	public static bool IsClicking(){
		bool touch = false;
		foreach (Touch t in Input.touches){
			if(t.phase == TouchPhase.Began)
				touch = true;
		}
		return Input.GetMouseButtonDown(0) || touch;
	}

	public static bool IsPressed(){
		bool pressed = false;
		foreach (Touch t in Input.touches){
			if(t.phase == TouchPhase.Began || t.phase == TouchPhase.Stationary || t.phase == TouchPhase.Moved)
				pressed = true;
		}
		return Input.GetMouseButton(0) || pressed;
	}
}
