using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Enemy2 : EnemyController {
	public List<Transform> transPositions;
	public List<Vector2> nextPositions;
	private int currentPosition = 0;
	// Use this for initialization
	public override void Start () {
		nextPositions = new List<Vector2> ();
		ActualizarPosiciones ();
		rotationTime *= 2;
	}
	
	// Update is called once per frame


	public override Vector3 NextPos(){

		//si has llegado a la nueva posicion, siguiente
		if ( Vector2.Distance(nextPositions [currentPosition], (Vector2)transform.position) <= Vector2.kEpsilon) 
			currentPosition++;

		//si has recorrido todas las posiciones, actualiza
		if (currentPosition >= transPositions.Count) {	
			ActualizarPosiciones();
		}

		//siguiente posicion
		return nextPositions [currentPosition];	
	}
	
	void ActualizarPosiciones(){
		nextPositions.Clear ();
		currentPosition = 0;
		foreach (Transform pos in transPositions) {
			nextPositions.Add (pos.position);
		}
	}




}
