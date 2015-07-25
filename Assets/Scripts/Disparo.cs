using UnityEngine;
using System.Collections;

public class Disparo : MonoBehaviour {

	public float speed;
	public float cadencia = 0.08f;
	public float damage = 10;
	public Vector2 direction;
	private Rigidbody2D rig;

	// Use this for initialization
	void Start () {
		rig = GetComponent<Rigidbody2D> ();
	}

	// Update is called once per frame
	void Update () {
		rig.velocity = direction * speed;
	}

	void OnCollisionEnter2D(Collision2D other){
		if (other.gameObject.tag == "Enemy") {
			other.gameObject.GetComponent<EnemyController>().currentHealth-=damage;
			gameObject.SetActive(false);	
		}
	}

	void OnBecameInvisible(){
		gameObject.SetActive(false);
	}
}
