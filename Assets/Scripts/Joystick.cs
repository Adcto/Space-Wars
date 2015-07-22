using UnityEngine;
using System.Collections;
using UnityEngine.EventSystems;

public class Joystick : MonoBehaviour, IPointerDownHandler, IDragHandler, IPointerUpHandler {
	
	public float smoothing;
	
	private Vector2 origin;
	private Vector2 direction;
	private Vector2 smoothDirection;
	private float angle;
	private bool touched;
	private int pointerID;

	void Awake () {
		direction = Vector2.zero;
		touched = false;
	}
	
	public void OnPointerDown (PointerEventData data) {
		if (!touched) {
			touched = true;
			pointerID = data.pointerId;
			origin = data.position;
		}
	}
	
	public void OnDrag (PointerEventData data) {
		if (data.pointerId == pointerID ) {		//BUG: Todos los pointers son el mismo, y aunque sale fuera cuenta igual

			Vector2 currentPosition = data.position;
	
			Vector2 directionRaw = currentPosition - origin;

			direction = directionRaw.normalized;
			angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg -90;
		}
	}
	
	public void OnPointerUp (PointerEventData data) {
		if (data.pointerId == pointerID) {
			direction = Vector2.zero;
			touched = false;
		}
	}
	
	public Vector2 GetDirection () {
		smoothDirection = Vector2.MoveTowards (smoothDirection, direction, smoothing);
		return smoothDirection;
	}

	public float GetAngle(){
		return angle;
	}
}