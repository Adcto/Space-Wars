using UnityEngine;
using System.Collections;

public class CameraController : MonoBehaviour {

	public Vector2 minPos, maxPos;
	public float verticalOffset;
	public float lookAheadDstX;
	public float lookAheadDstY;
	public float lookSmoothTimeX;
	public float verticalSmoothTime;
	public Vector2 focusAreaSize;
	
	FocusArea focusArea;
	
	float currentLookAheadX;
	float targetLookAheadX;
	float lookAheadDirX;
	float currentLookAheadY;
	float targetLookAheadY;
	float lookAheadDirY;
	float smoothLookVelocityX;
	float smoothVelocityY;
	
	bool lookAheadStoppedY;
	bool lookAheadStoppedX;
	
	void Start() {
		focusArea = new FocusArea (PlayerController.current.collider.bounds, focusAreaSize);
	}
	
	void LateUpdate() {
		focusArea.Update (PlayerController.current.collider.bounds);
		
		Vector2 focusPosition = focusArea.centre + Vector2.up * verticalOffset;
		
		if (focusArea.velocity.x != 0) {
			lookAheadDirX = Mathf.Sign (focusArea.velocity.x);
			if (Mathf.Sign(PlayerController.current.direction.x) == Mathf.Sign(focusArea.velocity.x) && PlayerController.current.direction.x != 0) {
				lookAheadStoppedX = false;
				targetLookAheadX = lookAheadDirX * lookAheadDstX;
			}
			else {
				if (!lookAheadStoppedX) {
					lookAheadStoppedX = true;
					targetLookAheadX = currentLookAheadX + (lookAheadDirX * lookAheadDstX - currentLookAheadX)/4f;
				}
			}
		}
		if (focusArea.velocity.y != 0) {
			lookAheadDirY = Mathf.Sign (focusArea.velocity.y);
			if (Mathf.Sign(PlayerController.current.direction.y) == Mathf.Sign(focusArea.velocity.y) && PlayerController.current.direction.y != 0) {
				lookAheadStoppedY = false;
				targetLookAheadY = lookAheadDirY * lookAheadDstY;
			}
			else {
				if (!lookAheadStoppedY) {
					lookAheadStoppedY = true;
					targetLookAheadY = currentLookAheadY + (lookAheadDirY * lookAheadDstY - currentLookAheadY)/4f;
				}
			}
		}

		
		
		currentLookAheadX = Mathf.SmoothDamp (currentLookAheadX, targetLookAheadX, ref smoothLookVelocityX, lookSmoothTimeX);
		currentLookAheadY = Mathf.SmoothDamp (currentLookAheadY, targetLookAheadY, ref smoothVelocityY, verticalSmoothTime);
		
		//focusPosition.y = Mathf.SmoothDamp (transform.position.y, focusPosition.y, ref smoothVelocityY, verticalSmoothTime);
		focusPosition += Vector2.right * currentLookAheadX;
		focusPosition += Vector2.up * currentLookAheadY;
		Vector3 nextPos = (Vector3)focusPosition + Vector3.forward * -10;
		transform.position = new Vector3 (Mathf.Clamp (nextPos.x, minPos.x, maxPos.x), Mathf.Clamp (nextPos.y,minPos.y, maxPos.y),-10);
	}
	
	void OnDrawGizmos() {
		Gizmos.color = new Color (1, 0, 0, .5f);
		Gizmos.DrawCube (focusArea.centre, focusAreaSize);
	}
	
	struct FocusArea {
		public Vector2 centre;
		public Vector2 velocity;
		float left,right;
		float top,bottom;
		
		
		public FocusArea(Bounds targetBounds, Vector2 size) {
			left = targetBounds.center.x - size.x/2;
			right = targetBounds.center.x + size.x/2;
			bottom = targetBounds.center.y - size.y/2;
			top = targetBounds.center.y + size.y/2;
			
			velocity = Vector2.zero;
			centre = targetBounds.center;
		}
		
		public void Update(Bounds targetBounds) {
			float shiftX = 0;
			if (targetBounds.min.x < left) {
				shiftX = targetBounds.min.x - left;
			} else if (targetBounds.max.x > right) {
				shiftX = targetBounds.max.x - right;
			}
			left += shiftX;
			right += shiftX;
			
			float shiftY = 0;
			if (targetBounds.min.y < bottom) {
				shiftY = targetBounds.min.y - bottom;
			} else if (targetBounds.max.y > top) {
				shiftY = targetBounds.max.y - top;
			}
			top += shiftY;
			bottom += shiftY;
			centre = new Vector2((left+right)/2,(top +bottom)/2);
			velocity = new Vector2 (shiftX, shiftY);
		}
	}





}
