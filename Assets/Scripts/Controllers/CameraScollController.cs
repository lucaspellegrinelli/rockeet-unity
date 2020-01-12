using UnityEngine;
using System.Collections;

public class CameraScollController: MonoBehaviour {

	public float speed = 1F;
	private float startx = 0.0f;

	void Start(){
		startx = transform.position.x;
	}
	void Update() {
		if (Input.touchCount > 0 && Input.GetTouch(0).phase == TouchPhase.Moved) {
			Vector2 touchDeltaPosition = Input.GetTouch(0).deltaPosition;
			transform.Translate(-touchDeltaPosition.x * speed * Time.deltaTime, -touchDeltaPosition.y * speed * Time.deltaTime, 0);
			transform.position = new Vector3(startx, Mathf.Clamp(transform.position.y, -4.8f, 0), -10);
		}
	}
}