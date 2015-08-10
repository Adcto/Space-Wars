using UnityEngine;
using System.Collections;

public class AgujeroNegro : EnemyController {
	public float baseDist =4;
	public float baseForce = 5;
	public float dist =4;
	public float force = 5;
	// Use this for initialization
	public override void OnEnable ()
	{
		base.OnEnable ();
		dist = baseDist + calidad;
		force = baseForce + calidad*0.5f;
	}
	// Update is called once per frame
	public override void FixedUpdate () {
		if (Vector2.Distance (PlayerController.current.transform.position, transform.position) <= dist) {
			PlayerController.current.transform.position = 
				Vector2.MoveTowards(PlayerController.current.transform.position,
				                    transform.position,force * Time.fixedDeltaTime);
		}
	}
	
}
