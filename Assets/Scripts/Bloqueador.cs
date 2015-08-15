using UnityEngine;
using System.Collections;

public class Bloqueador : MonoBehaviour {
	public float radio = 2;
	public float rotationTime = 360;
	private Vector3 relativeDistance;
	void Awake(){
		transform.position +=  Vector3.right* radio;
		relativeDistance = (transform.position - PlayerController.current.transform.position);
	}
	
	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void FixedUpdate () {

		transform.position = PlayerController.current.transform.position + relativeDistance;
		transform.RotateAround(PlayerController.current.transform.position, Vector3.forward, rotationTime * Time.deltaTime);
		relativeDistance = (transform.position - PlayerController.current.transform.position);

	}

	void OnCollisionEnter2D(Collision2D other){
		if (other.gameObject.tag == "Enemy" ||other.gameObject.tag == "Asteroide" ) {
			
			other.gameObject.GetComponent<EnemyController>().QuitarVida(-1);
		}
	}
	

}
