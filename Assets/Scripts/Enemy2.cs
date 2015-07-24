using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Enemy2 : EnemyController {
	public List<Transform> transPositions;
	//public List<Vector2> nextPositions;
	public Vector2 nextPosition;
	private int currentPosition = 0;
	// Use this for initialization
//	public override void Start () {
//		nextPositions = new List<Vector2> ();
//		ActualizarPosiciones ();
//
//	}
//	
	// Update is called once per frame


	void OnEnable(){
		currentPosition = 0;
		nextPosition = transPositions[currentPosition].position;
	}

	public override Vector3 NextPos(){

		//si has llegado a la nueva posicion, siguiente
		if (Vector2.Distance (nextPosition, (Vector2)transform.position) <= Vector2.kEpsilon) {
			currentPosition++;
			if (currentPosition >= transPositions.Count)
				currentPosition = 0;
			nextPosition = transPositions[currentPosition].position;
		}

		if (Vector2.Distance (nextPosition, (Vector2)transform.position) > Vector2.Distance ((Vector2)PlayerController.current.transform.position, (Vector2)transform.position))
			nextPosition = (Vector2)PlayerController.current.transform.position;

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




}
