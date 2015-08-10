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


	public override void OnEnable(){
		currentPosition = 0;
		nextPosition = transPositions[0].position;
		prevPos = Vector3.forward;
		quieto = false;
		base.OnEnable (); //reset stats
	}

	public override Vector3 NextPos(){
		if (transform.position == prevPos)
			quieto = true;

		if (Vector2.Distance (nextPosition, (Vector2)transform.position) > Vector2.Distance ((Vector2)PlayerController.current.transform.position, (Vector2)transform.position))
			return (Vector2)PlayerController.current.transform.position;

		//si has llegado a la nueva posicion, siguiente
		if (Vector2.Distance (nextPosition, (Vector2)transform.position) <= Vector2.kEpsilon || hit || quieto) {
			if(hit) 
				hit = false;
			if(quieto)
				quieto = false;
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

		/*
		Bugs:
		-Se quedan atascados en las paredes x un tiempo, mientras encuentran la nextPosition adecuada
		-Cuando spawnean oleadas, algunas veces se juntan varios y se quedan atascados -> depende de la pos del player, por casualidad se da el angulo justo para q ocurra
		*/
	

		prevPos = transform.position;
		return nextPosition;	
	}
	


	void OnCollisionEnter2D(Collision2D other){
		if (other.gameObject.tag == "Finish") {
			hit = true;
		}
	}




}
