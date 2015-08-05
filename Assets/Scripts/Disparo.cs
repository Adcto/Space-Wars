using UnityEngine;
using System.Collections;

public class Disparo : MonoBehaviour {

	public float speed;
	public float cadencia = 0.08f;
	public float damage = 10;
	public Vector2 direction;
	public float dispersion;
	public DesactivarEmpty padre;
	private Vector3 startPos;
	private Quaternion startRot;
	private Rigidbody2D rig;
	public float TimeToDestroy = 1;

	// Use this for initialization
	void Start () {
		rig = GetComponent<Rigidbody2D> ();


	}

	void OnEnable(){
		startPos = transform.localPosition;
		startRot = transform.localRotation;
		Invoke ("Desactivate", TimeToDestroy);
		float desviacion = 0;
		if (dispersion >0) {
			desviacion = Random.Range (-dispersion, dispersion);
		}
		Vector3 dir = Quaternion.Euler(transform.rotation.eulerAngles + new Vector3(0,0,90+desviacion)) *  new Vector3(1,0.0f,0.0f);
		direction = (Vector2) dir.normalized;
	}

	void Desactivate(){
		if (gameObject.activeInHierarchy) {
			if (padre != null) {
				padre.Desactivar ();
				transform.localPosition = startPos;
				transform.localRotation = startRot;
			}
			gameObject.SetActive (false);
		}
	}

	// Update is called once per frame
	void Update () {
		rig.velocity = direction * speed;
	}

	void OnCollisionEnter2D(Collision2D other){
		if (other.gameObject.tag == "Enemy") {
			other.gameObject.GetComponent<EnemyController>().currentHealth-=damage;
			CancelInvoke();
			Desactivate();

		}
	}

	void OnBecameInvisible(){
		CancelInvoke ();
		Desactivate ();
		
	}
	
}
