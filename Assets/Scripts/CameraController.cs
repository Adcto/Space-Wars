using UnityEngine;
using System.Collections;

public class CameraController : MonoBehaviour {
	public BoxCollider2D bounds;
	private Bounds borde;
	// Use this for initialization
	void Start () {
		borde = bounds.bounds;
	}
	
	// Update is called once per frame
	void LateUpdate () {
		Vector2 playerPos =  PlayerController.current.transform.position;
		transform.position = new Vector3 (playerPos.x,playerPos.y,transform.position.z);	
	}

}
