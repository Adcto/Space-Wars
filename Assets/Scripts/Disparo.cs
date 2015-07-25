using UnityEngine;
using System.Collections;

public class Disparo : MonoBehaviour {

	public float speed;
	public float cadencia = 0.08f;
	public float damage;
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
			gameObject.SetActive(false);
			GameManager.current.AddScore(other.gameObject.GetComponent<EnemyController>().score);
			other.gameObject.SetActive(false);
			
		}
		
	}
	void OnBecameInvisible(){
		gameObject.SetActive(false);
	}
}
