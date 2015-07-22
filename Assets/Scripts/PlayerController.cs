using UnityEngine;
using System.Collections;

public class PlayerController : MonoBehaviour {
	public static PlayerController current;
	public Transform posicionDisparo;
	public Joystick movement;
	public Joystick aim;
	public float maxHealth = 10;
	public float currentHealth;
	public bool puedeDisparar = false;
	public float cadenciaDisparo;
	//skill1,skill2
	public float angle;
	public float speed = 2f;
	private Vector2 shootDirection;
	private Rigidbody2D rig;
	private float cooldownDisparo = 0;
	// Use this for initialization
	void Awake(){
		current = this;

	}

	void Start () {
		cadenciaDisparo = Pool.current.cadenciaDisparo; //Habra q actualizarla cuando se cambie de disparo
		currentHealth = maxHealth;
		rig = GetComponent<Rigidbody2D> ();
	}

	// Update is called once per frame
	void FixedUpdate () {
		Move ();
		if (puedeDisparar) {
			cooldownDisparo += Time.deltaTime;
			if(cooldownDisparo >= cadenciaDisparo){
				cooldownDisparo = 0;
				Disparar ();
			}
		}
	}

	void Move(){
		Vector2 direction = movement.GetDirection ();
		angle = aim.GetAngle ();

		rig.velocity = direction * speed;
		transform.rotation = Quaternion.RotateTowards (transform.rotation, Quaternion.Euler (0, 0, angle), 540 * Time.deltaTime); // Giro con delay
		//rig.MoveRotation(angle); //Giro Instantaneo
	}

	void Disparar(){
		GameObject shoot = Pool.current.Disparar ();
		shoot.transform.position = posicionDisparo.position;
		shoot.transform.rotation = transform.rotation;
		float desviacion = Random.Range (-5f, 5f);


		Vector3 dir = Quaternion.Euler(transform.rotation.eulerAngles + new Vector3(0,0,90+desviacion)) *  new Vector3(1,0.0f,0.0f);
		//float shootAngle = shoot.transform.rotation.eulerAngles.z;
		shoot.GetComponent<Disparo> ().direction = (Vector2)dir.normalized;
		//shoot.GetComponent<Disparo> ().direction = new Vector2 (Mathf.Cos (shootAngle), Mathf.Sin (shootAngle));
		shoot.layer = gameObject.layer;
		shoot.SetActive (true);
	}

	void OnCollisionEnter2D(Collision2D other){
		if (other.gameObject.tag == "Enemy") {
			currentHealth--;
			other.gameObject.SetActive(false);
		}
	}
}
