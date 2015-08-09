using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Enemy2 : EnemyController {
	public List<Transform> transPositions;
	//public List<Vector2> nextPositions;
	public Vector2 nextPosition;
	private int currentPosition = 0;
	private Vector3 prevPos;
	private bool quieto = false;
	// Use this for initialization
//	public override void Start () {
//		nextPositions = new List<Vector2> ();
//		ActualizarPosiciones ();
//
//	}
//	
	// Update is called once per frame


	public override void OnEnable(){
		currentPosition = 0;
		nextPosition = transPositions[0].position;
		prevPos = Vector3.forward;
		quieto = false;
		base.OnEnable (); //reset hp
	}

	public override Vector3 NextPos(){
		if (transform.position != prevPos)
			quieto = false;

		if (Vector2.Distance (nextPosition, (Vector2)transform.position) > Vector2.Distance ((Vector2)PlayerController.current.transform.position, (Vector2)transform.position))
			return (Vector2)PlayerController.current.transform.position;

		//si has llegado a la nueva posicion, siguiente
		if (Vector2.Distance (nextPosition, (Vector2)transform.position) <= Vector2.kEpsilon || hit || quieto) {
			if(hit) 
				hit = false;
			currentPosition++;
			if (currentPosition >= transPositions.Count)
				currentPosition = 0;
//			while(transPositions[currentPosition].position.x <GameManager.current.min.x-0.5f
//			      && transPositions[currentPosition].position.y < GameManager.current.min.y-0.5f 
//			      && transPositions[currentPosition].position.x > GameManager.current.max.x +0.5f
//			      && transPositions[currentPosition].position.y > GameManager.current.max.y+0.5f){
//				currentPosition++;
//				if (currentPosition >= transPositions.Count)
//					currentPosition = 0;
//			}
			nextPosition = transPositions[currentPosition].position;
		}

		if (transform.position == prevPos)
			quieto = true;





		prevPos = transform.position;

		//si has recorrido todas las posiciones, actualiza
//		if (currentPosition >= transPositions.Count) {	
//			ActualizarPosiciones();
//		}
		//siguiente posicion
		return nextPosition;	
	}
	
//	void ActualizarPosiciones(){
//		nextPositions.Clear ();
//		currentPosition = 0;
//		foreach (Transform pos in transPositions) {
//			nextPositions.Add (pos.position);
//		}
//	}

	void OnCollisionEnter2D(Collision2D other){
		if (other.gameObject.tag == "Finish") {
			hit = true;
		}
	}




}
