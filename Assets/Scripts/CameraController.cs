using UnityEngine;
using System.Collections;

public class CameraController : MonoBehaviour {
	//public BoxCollider2D bounds;
	public float smooth = 0.1f;
	public Vector3 min,max;
	// Use this for initialization
	void Start () {
		//borde = bounds.bounds;
	}
	
	// Update is called once per frame
	void FixedUpdate () {
		//Vector2 playerPos =  PlayerController.current.transform.position;

		Vector3 nextPos= Vector3.Lerp(transform.position,PlayerController.current.transform.position,smooth * Time.deltaTime); //+ new Vector3(0,0,-10);	
		transform.position = new Vector3 (Mathf.Clamp (nextPos.x, min.x, max.x), Mathf.Clamp (nextPos.y,min.y, max.y),-10); 
	}

}
