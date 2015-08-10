using UnityEngine;
using System.Collections;

public class EnemyRecto : EnemyController {

	public bool girar = false;
	public bool girando = false;
	public Vector2 direction;
	public int angle;
	Rigidbody2D rig;

	public override void Awake ()
	{
		rig = GetComponent<Rigidbody2D> ();
		base.Awake ();
	}
	public override void OnEnable ()
	{
		direction = Quaternion.Euler(transform.rotation.eulerAngles + new Vector3(0,0,90)) *  new Vector3(1,0.0f,0.0f);
		angle =(int)( Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg -90);
		girando = false;
		girar = false;
		base.OnEnable ();
	}

	public override void FixedUpdate ()
	{
		if (girando) {
			Rotate ();	
		}
		else if (!anim.GetCurrentAnimatorStateInfo(0).IsName("Spawn")) {
			if(girar){
				girando = true;
				direction*= -1;
				if(angle < 180)
					angle +=180;
				else 
					angle -=180;
				Rotate ();
			}
			else 
				rig.velocity = direction * speed;
		}

	}

	public override void Rotate(){
		girar = false;
		transform.rotation = Quaternion.RotateTowards(transform.rotation, Quaternion.Euler(0,0,angle), rotationTime *Time.fixedDeltaTime);
		if (Quaternion.Angle(transform.rotation,  Quaternion.Euler(0,0,angle)) <= Quaternion.kEpsilon)
			girando = false;
	}

	void OnCollisionEnter2D(Collision2D other){
		if (other.gameObject.tag == "Finish") {
			girar = true;
		}
	}
	

}
