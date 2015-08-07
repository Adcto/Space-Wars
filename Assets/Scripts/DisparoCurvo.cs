using UnityEngine;
using System.Collections;

public class DisparoCurvo : Disparo {
	public float angle = 0;
	private Quaternion nextRotation;
	public float smooth;
	public int sentido;
	public override void OnEnable ()
	{
		startPos = transform.localPosition;
		startRot = transform.localRotation;
		Invoke ("Desactivate", TimeToDestroy);
		desviacion = 0;
		if (dispersion >0) {
			desviacion = Random.Range (-dispersion, dispersion);
		}
		Vector3 dir = Quaternion.Euler(transform.rotation.eulerAngles + new Vector3(0,0,90 + desviacion)) *  new Vector3(invertido,0.0f,0.0f);
		direction = (Vector2) dir.normalized;
		angle = Mathf.Abs (angle);
		if (sentido == -1)
			angle *= -1;
		nextRotation = Quaternion.Euler(transform.rotation.eulerAngles + new Vector3 (0, 0, angle / 2));
		angle *= -1; 
	}
	
	// Update is called once per frame
	public override void FixedUpdate () {

		transform.rotation = Quaternion.Slerp (transform.rotation, nextRotation, smooth * Time.deltaTime);
		if (Quaternion.Angle (transform.rotation,nextRotation) <= Quaternion.kEpsilon) {
			nextRotation = Quaternion.Euler(transform.rotation.eulerAngles + new Vector3 (0, 0, angle));
			angle *= -1;

		}
		Vector3 dir = Quaternion.Euler(transform.rotation.eulerAngles + new Vector3 (0, 0, 90 + desviacion)) *  new Vector3(1,0.0f,0.0f);
		direction = (Vector2) dir.normalized;

		base.FixedUpdate ();
	}
}
