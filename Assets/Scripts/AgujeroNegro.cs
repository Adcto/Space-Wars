using UnityEngine;
using System.Collections;

public class AgujeroNegro : EnemyController {
	public float dist =4;
	public float force = 5;
	// Use this for initialization

	// Update is called once per frame
	public override void FixedUpdate () {
		if (Vector2.Distance (PlayerController.current.transform.position, transform.position) <= dist) {
			PlayerController.current.transform.position = 
				Vector2.MoveTowards(PlayerController.current.transform.position,
				                    transform.position,force * Time.fixedDeltaTime);
		}
	}
	
}
